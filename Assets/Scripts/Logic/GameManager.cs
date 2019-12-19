using System.Collections.Generic;
using UnityEngine;
using WhaleAppTapGame.DI;
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
        [Inject] private iInputManager m_InputManager;

        [Inject(Id = BindIDs.INJECT_ID_PlayerEntity)]
        private iUnitEntity m_PlayerEntity;

        private List<UnitView> m_UnitViews;
        private UnitFactory[] m_UnitFactories;

        private bool m_IsActive = false;
        private Vector2 m_ScreenBounds;


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
            //Get screen bounds
            m_ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

            //Init spawn timer
            m_UnitSpawnTimer.OnUnitShouldBeSpawned += UnitSpawnTimer_UnitShouldBeSpawnedHandler;

            //Init input manager
            m_InputManager.OnInputExecuted += InputManager_InputExecutedHandler;

            //Init score
            m_ScoreController.OnScoreChanged += m_UIManager.PlayerScoreController.UpdateScore;

            //Init list of enemies
            m_UnitViews = new List<UnitView>();

            //Init factories
            m_UnitFactories = new UnitFactory[]
            {
                new SimpleEnemyUnitFactory(m_PrefabsLibrary.UnitViewPrefab, m_ScreenBounds,  new Vector2(0.7f, 1.5f), 1, 1, UnitView_UnitOutOfBottomBoundHandler, UnitView_UnitDestroyedHandler),
                new DiagonalEnemyUnitFactory(m_PrefabsLibrary.UnitViewPrefab, m_ScreenBounds, new Vector2(0.7f, 1.5f), 1, 1, 50, UnitView_UnitOutOfBottomBoundHandler, UnitView_UnitDestroyedHandler),
                new FriendlyUnitFactory(m_PrefabsLibrary.UnitViewPrefab, m_ScreenBounds, new Vector2(0.7f, 1.5f), 1, 50, UnitView_UnitOutOfBottomBoundHandler, UnitView_FriendlyUnitDestroyedHandler)
            };

            //Init player
            m_PlayerEntity.OnEntityDestroyed += PlayerEntity_EntityDestroyedHandler;
            m_PlayerEntity.OnHPChanged += (int currentHP) => m_UIManager.PlayerHealthBarController.UpdateHealthBar(currentHP);

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

        void UnitView_UnitDestroyedHandler(UnitView unitView)
        {
            RemoveUnitView_Explosion(unitView);
            m_ScoreController.IncrementScore();
        }

        void UnitView_FriendlyUnitDestroyedHandler(UnitView unitView)
        {
            RemoveUnitView_Explosion(unitView);
            HandleGameOver();
        }

        void UnitView_UnitOutOfBottomBoundHandler(UnitView unitView)
        {
            m_PlayerEntity.TakeDamage(unitView.Damage);
            RemoveUnitView_Silent(unitView);
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

