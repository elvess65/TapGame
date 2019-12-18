using System.Collections.Generic;
using UnityEngine;
using WhaleAppTapGame.Logic.Entities;
using WhaleAppTapGame.Logic.Input;
using WhaleAppTapGame.Logic.View;
using WhaleAppTapGame.UI;

namespace WhaleAppTapGame.Logic
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => m_Instance;

        private static GameManager m_Instance;

        public PrefabsLibrary PrefabsLibrary;
        public UIManager UIManager;

        private UnitSpawnTimer m_UnitSpawnTimer;
        private iInputManager m_InputManager;

        private iUnitEntity m_PlayerEntity;
        private List<UnitView> m_UnitViews;
        private UnitFactory[] m_UnitFactories;

        private bool m_IsActive = false;
        private Vector2 m_ScreenBounds;
        private int m_Score;

        private const int m_PLAYER_MAX_HP = 3;
        private const int m_PLAYER_DAMAGE = 1;


        void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
        }

        void Start()
        {
            InitializeComponents();
            CreatePlayer();
            StartMainLoop();
        }

        void Update()
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
            m_UnitSpawnTimer = new UnitSpawnTimer(1, 1);
            m_UnitSpawnTimer.OnUnitShouldBeSpawned += UnitSpawnTimer_UnitShouldBeSpawnedHandler;

            //Init input manager
            m_InputManager = new TapInputManager();
            m_InputManager.OnInputExecuted += InputManager_InputExecutedHandler;

            //Init list of enemies
            m_UnitViews = new List<UnitView>();

            //Init factories
            m_UnitFactories = new UnitFactory[]
            {
                new SimpleEnemyUnitFactory(PrefabsLibrary.UnitViewPrefab, m_ScreenBounds,  new Vector2(0.7f, 1.5f), 1, 1, UnitView_UnitOutOfBottomBoundHandler, UnitView_UnitDestroyedHandler),
                new DiagonalEnemyUnitFactory(PrefabsLibrary.UnitViewPrefab, m_ScreenBounds, new Vector2(0.7f, 1.5f), 1, 1, 50, UnitView_UnitOutOfBottomBoundHandler, UnitView_UnitDestroyedHandler),
                new FriendlyUnitFactory(PrefabsLibrary.UnitViewPrefab, m_ScreenBounds, new Vector2(0.7f, 1.5f), 1, 50, UnitView_UnitOutOfBottomBoundHandler, UnitView_FriendlyUnitDestroyedHandler)
            };

            //Init UI
            UIManager.Init();
            UIManager.PlayerHealthBarController.Init(m_PLAYER_MAX_HP);
        }

        void CreatePlayer()
        {
            m_PlayerEntity = new UnitEntity(m_PLAYER_MAX_HP, m_PLAYER_DAMAGE, Color.white);
            m_PlayerEntity.OnEntityDestroyed += PlayerEntity_EntityDestroyedHandler;
            m_PlayerEntity.OnHPChanged += (int currentHP) => UIManager.PlayerHealthBarController.UpdateHealthBar(currentHP);
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
            UnitFactory randomUnitFactory = m_UnitFactories[Random.Range(0, m_UnitFactories.Length)];
            UnitView unitView = randomUnitFactory.CreateUnit();
            
            //Add view to processing
            m_UnitViews.Add(unitView);
        }

        void UnitView_UnitDestroyedHandler(UnitView unitView)
        {
            RemoveUnitView_Explosion(unitView);
            IncrementScore();
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
            GameObject explosion = Instantiate(PrefabsLibrary.ExplosionPrefab, unitView.transform.position, Quaternion.identity);
            Destroy(explosion, 2);

            RemoveUnitView_Silent(unitView);
        }

        void RemoveUnitView_Silent(UnitView unitView)
        {
            m_UnitViews.Remove(unitView);
            Destroy(unitView.gameObject);
        }


        void InputManager_InputExecutedHandler(GameObject hitObject)
        {
            UnitView hitUnitVIew = hitObject.GetComponent<UnitView>();
            if (hitUnitVIew != null)
                hitUnitVIew.TakeDamage(m_PlayerEntity.Damage);
        }

        void PlayerEntity_EntityDestroyedHandler(iUnitEntity entity) => HandleGameOver();

        void IncrementScore()
        {
            m_Score += 10;
            UIManager.PlayerScoreController.UpdateScore(m_Score);
        }

        void HandleGameOver()
        {
            m_IsActive = false;
            UIManager.ShowUIWindow_GameOver(m_Score);
        }
    }
}

