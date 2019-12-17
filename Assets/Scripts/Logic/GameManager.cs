using System.Collections.Generic;
using UnityEngine;
using WhaleAppTapGame.Logic.Entities;
using WhaleAppTapGame.Logic.Input;
using WhaleAppTapGame.Logic.View;

namespace WhaleAppTapGame.Logic
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => m_Instance;
       
        private static GameManager m_Instance;

        public PrefabsLibrary PrefabsLibrary;

        private UnitSpawnTimer m_UnitSpawnTimer;
        private iInputManager m_InputManager;

        private DestroyableEntity m_PlayerEntity;
        private List<DestroyableEntity> m_EnemyEntities;

        private bool m_IsActive = false;


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
            }
        }


        void InitializeComponents()
        {
            //Init spawn timer
            m_UnitSpawnTimer = new UnitSpawnTimer(1, 1);
            m_UnitSpawnTimer.OnUnitShouldBeSpawned += UnitSpawnTimer_UnitShouldBeSpawnedHandler;

            //Init input manager
            m_InputManager = new TapInputManager();
            m_InputManager.OnInputExecuted += InputManager_InputExecutedHandler;

            //Init list of enemies
            m_EnemyEntities = new List<DestroyableEntity>();
        }

        void CreatePlayer()
        {
            m_PlayerEntity = new DestroyableEntity(3);
            m_PlayerEntity.OnEntityDestroyed += PlayerEntity_EntityDestroyedHandler;
        }

        void StartMainLoop() => m_IsActive = true;


        void InputManager_InputExecutedHandler()
        {
            m_PlayerEntity.TakeDamage();
        }

        void UnitSpawnTimer_UnitShouldBeSpawnedHandler()
        {
            Debug.Log("Create enemy");

            DestroyableEntity entity = new DestroyableEntity(1);

            UnitView view = Instantiate(PrefabsLibrary.UnitViewPrefab) as UnitView;
            view.Init(entity);

            
            //m_EnemyEntities.Add(enemyPlayer);
        }

        void PlayerEntity_EntityDestroyedHandler(DestroyableEntity sender)
        {
            Debug.Log("Game over");
            m_IsActive = false;
        }
    }
}
