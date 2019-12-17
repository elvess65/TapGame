using System.Collections.Generic;
using UnityEngine;
using WhaleAppTapGame.Logic.Entities;
using WhaleAppTapGame.Logic.Input;
using WhaleAppTapGame.Logic.Movement;
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
        private List<UnitView> m_UnitViews;

        private bool m_IsActive = false;
        private Vector2 m_ScreenBounds;


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
        }

        void CreatePlayer()
        {
            m_PlayerEntity = new DestroyableEntity(3, 1, Color.white);
            m_PlayerEntity.OnEntityDestroyed += PlayerEntity_EntityDestroyedHandler;
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
            //Create entity
            iDestroyableEntity enemyEntity = new DestroyableEntity(1, 1, Color.red);
            iDestroyableEntity frindlyEntity = new FriendlyEntity(1, Color.green);
            iDestroyableEntity entity = Random.Range(0, 100) > 50 ? enemyEntity : frindlyEntity;

            //Create view
            UnitView view = Instantiate(PrefabsLibrary.UnitViewPrefab) as UnitView;

            float minSpawnPosX = -m_ScreenBounds.x - view.SpriteHalfWidth + 1;
            float maxSpawnPosX = m_ScreenBounds.x + view.SpriteHalfWidth - 1;
            float spawnPosX = Random.Range(minSpawnPosX, maxSpawnPosX);
            float spawnPosY = m_ScreenBounds.y + view.SpriteHalfHeight;

            view.transform.position = new Vector3(spawnPosX, spawnPosY, 0);

            //Create movement
            Vector2 viewSpriteBounds = new Vector2(view.SpriteHalfWidth, view.SpriteHalfHeight);
            float randomSpeed = Random.Range(0.7f, 1.5f);
            iMoveStrategy simpleMoveStrategy = new SimpleMoveStrategy(view.transform, randomSpeed, m_ScreenBounds, viewSpriteBounds);
            iMoveStrategy diagonalMoveStrategy = new DiagonalMoveStrategy(view.transform, randomSpeed, m_ScreenBounds, viewSpriteBounds);
            iMoveStrategy randomMoveStrategy = Random.Range(0, 100) >= 50 ? simpleMoveStrategy : diagonalMoveStrategy;

            //Initialize view
            view.OnUnitOutOfBottomBound += UnitView_UnitOutOfBottomBoundHandler;
            view.OnUnitDestroyed += UnitView_UnitDestroyedHandler;
            view.Init(entity, randomMoveStrategy);
            
            //Add view to processing
            m_UnitViews.Add(view);
        }

        void UnitView_UnitDestroyedHandler(UnitView unitView)
        {
            Debug.Log("Unit was destroyed at: " + unitView.transform.position);
            //TODO: Create explosion

            RemoveUnitView(unitView);
        }

        void UnitView_UnitOutOfBottomBoundHandler(UnitView unitView) => m_PlayerEntity.TakeDamage(unitView.Damage);

        void RemoveUnitView(UnitView unitView)
        {
            m_UnitViews.Remove(unitView);
            Destroy(unitView.gameObject);
        }


        void InputManager_InputExecutedHandler()
        {
            m_UnitViews[0].TakeDamage(m_PlayerEntity.Damage);
        }

        void PlayerEntity_EntityDestroyedHandler(iDestroyableEntity entity)
        {
            m_IsActive = false;
            Debug.Log("Game over");
        }
    }
}
