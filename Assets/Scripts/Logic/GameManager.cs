using System.Collections.Generic;
using UnityEngine;
using WhaleAppTapGame.DI;
using WhaleAppTapGame.Logic.Effects;
using WhaleAppTapGame.Logic.Entities;
using WhaleAppTapGame.Logic.Input;
using WhaleAppTapGame.Logic.View;
using WhaleAppTapGame.UI;
using Zenject;

namespace WhaleAppTapGame.Logic
{
    public class GameManager : IInitializable, ITickable
    {
        [Inject] private UIManager m_UIManager;
        [Inject] private PrefabsLibrary m_PrefabsLibrary;
        [Inject] private UnitSpawnTimer m_UnitSpawnTimer;
        [Inject] private ScoreController m_ScoreController;
        [Inject] private CameraShakeEffect m_CameraShakeEffect;
        [Inject] private iInputManager m_InputManager;

        [Inject(Id = InjectIDs.PLAYER_ENTITY)]
        private iUnitEntity m_PlayerEntity;

        [Inject(Id = InjectIDs.SIMPLE_UNIT_FACTORY)]
        private UnitFactory m_SimpleEnemyFactory;

        [Inject(Id = InjectIDs.DIAGONAL_UNIT_FACTORY)]
        private UnitFactory m_DiagonalEnemyUnitFactory;

        [Inject(Id = InjectIDs.FRIENDLY_UNITY_FACTORY)]
        private UnitFactory m_FriendlyUnitFactory;

        private bool m_IsActive = false;
        private List<UnitView> m_UnitViews;
        private UnitFactory[] m_UnitFactories;


        public void Initialize()
        {
            InitializeComponents();
            StartMainLoop();
        }

        public void Tick()
        {
            if (m_IsActive)
            {
                m_UnitSpawnTimer.UpdateComponent(Time.deltaTime);
                m_InputManager.UpdateComponent(Time.deltaTime);

                ProcessUnitViews();
            }
        }


        void InitializeComponents()
        {
            //Init spawn timer
            m_UnitSpawnTimer.OnUnitShouldBeSpawned += UnitSpawnTimer_UnitShouldBeSpawnedHandler;

            //Init input manager
            m_InputManager.OnInputExecuted += InputManager_InputExecutedHandler;

            //Init score
            m_ScoreController.OnScoreChanged += m_UIManager.PlayerScoreController.UpdateScore;

            //Init list of enemies
            m_UnitViews = new List<UnitView>();

            //Init factories
            m_SimpleEnemyFactory.OnUnitOutOfBottomBound += UnitView_EnemyUnit_OutOfBottomBoundHandler;
            m_SimpleEnemyFactory.OnUnitDestroyed += UnitView_EnemyUnit_DestroyedHandler;

            m_DiagonalEnemyUnitFactory.OnUnitOutOfBottomBound += UnitView_EnemyUnit_OutOfBottomBoundHandler;
            m_DiagonalEnemyUnitFactory.OnUnitDestroyed += UnitView_EnemyUnit_DestroyedHandler;

            m_FriendlyUnitFactory.OnUnitOutOfBottomBound += UnitView_FriendlyUnit_OutOfBottomBoundHandler;
            m_FriendlyUnitFactory.OnUnitDestroyed += UnitView_FriendlyUnit_DestroyedHandler;

            m_UnitFactories = new UnitFactory[] { m_SimpleEnemyFactory, m_DiagonalEnemyUnitFactory, m_FriendlyUnitFactory };

            //Init player
            m_PlayerEntity.OnEntityDestroyed += PlayerEntity_EntityDestroyedHandler;
            m_PlayerEntity.OnHPChanged += (int currentHP) =>
            {
                m_CameraShakeEffect.TriggerShake();
                m_UIManager.PlayerHealthBarController.UpdateHealthBar(currentHP);
            };

            //Init UI
            m_UIManager.Init();
            m_UIManager.PlayerHealthBarController.Init(3);
        }

        void StartMainLoop() => m_IsActive = true;

        void ProcessUnitViews()
        {
            if (m_UnitViews.Count > 0)
            {
                for (int i = 0; i < m_UnitViews.Count; i++)
                    m_UnitViews[i].Move();
            }
        }


        void UnitSpawnTimer_UnitShouldBeSpawnedHandler()
        {
            //Get random unit factory
            UnitFactory randomUnitFactory = m_UnitFactories[Random.Range(0, m_UnitFactories.Length)];
 
            //Add view to processing
            m_UnitViews.Add(randomUnitFactory.CreateUnit());
        }


        void UnitView_EnemyUnit_OutOfBottomBoundHandler(UnitView unitView)
        {
            m_PlayerEntity.TakeDamage(unitView.Damage);
            RemoveUnitView_Silent(unitView);
        }

        void UnitView_EnemyUnit_DestroyedHandler(UnitView unitView)
        {
            RemoveUnitView_Explosion(unitView);
            m_ScoreController.IncrementScore();
        }

        void UnitView_FriendlyUnit_OutOfBottomBoundHandler(UnitView unitView) => RemoveUnitView_Silent(unitView);

        void UnitView_FriendlyUnit_DestroyedHandler(UnitView unitView)
        {
            RemoveUnitView_Explosion(unitView);
            HandleGameOver();
        }


        void RemoveUnitView_Explosion(UnitView unitView)
        {
            GameObject explosion = MonoBehaviour.Instantiate(m_PrefabsLibrary.ExplosionPrefab, unitView.transform.position, Quaternion.identity);
            MonoBehaviour.Destroy(explosion, 2);

            RemoveUnitView_Silent(unitView);
        }

        void RemoveUnitView_Silent(UnitView unitView)
        {
            m_UnitViews.Remove(unitView);
            MonoBehaviour.Destroy(unitView.gameObject);
        }


        void InputManager_InputExecutedHandler(GameObject hitObject)
        {
            UnitView hitUnitVIew = hitObject.GetComponent<UnitView>();
            if (hitUnitVIew != null)
                hitUnitVIew.TakeDamage(m_PlayerEntity.Damage);
        }

        void PlayerEntity_EntityDestroyedHandler(iUnitEntity entity) => HandleGameOver();

        void HandleGameOver()
        {
            m_IsActive = false;
            m_UIManager.ShowUIWindow_GameOver(m_ScoreController.Score);
        }
    }
}

