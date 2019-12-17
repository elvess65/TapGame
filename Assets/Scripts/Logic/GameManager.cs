﻿using System.Collections.Generic;
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
            m_PlayerEntity = new DestroyableEntity(3);
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


        void InputManager_InputExecutedHandler()
        {
            //m_UnitViews[0].TakeDamage();
        }

        void UnitSpawnTimer_UnitShouldBeSpawnedHandler()
        {
            //Create data
            iDestroyableEntity entity = new DestroyableEntity(1);

            //Create view
            UnitView view = Instantiate(PrefabsLibrary.UnitViewPrefab) as UnitView;

            float minSpawnPosX = -m_ScreenBounds.x - view.SpriteHalfWidth + 1;
            float maxSpawnPosX = m_ScreenBounds.x + view.SpriteHalfWidth - 1;
            float spawnPosX = Random.Range(minSpawnPosX, maxSpawnPosX);
            float spawnPosY = m_ScreenBounds.y + view.SpriteHalfHeight;

            view.transform.position = new Vector3(spawnPosX, spawnPosY, 0);
            view.OnUnitDestroyed += UnitView_UnitDestroyedHandler;
            view.OnUnitOutOfBottomBound += UnitView_UnitOutOfBottomBoundHandler;

            Vector2 viewSpriteBounds = new Vector2(view.SpriteHalfWidth, view.SpriteHalfHeight);
            iMoveStrategy simpleMoveStrategy = new SimpleMoveStrategy(view.transform, Random.Range(0.7f, 1.5f), m_ScreenBounds, viewSpriteBounds);
            iMoveStrategy diagonalMoveStrategy = new DiagonalMoveStrategy(view.transform, Random.Range(0.7f, 1.5f), m_ScreenBounds, viewSpriteBounds);
            iMoveStrategy randomMoveStrategy = Random.Range(0, 100) >= 50 ? simpleMoveStrategy : diagonalMoveStrategy;
                                                                            
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

        void UnitView_UnitOutOfBottomBoundHandler(UnitView unitView) => m_PlayerEntity.TakeDamage(unitView.DealDamage());

        void PlayerEntity_EntityDestroyedHandler(iDestroyableEntity entity)
        {
            m_IsActive = false;
            Debug.Log("Game over");
        }

        void RemoveUnitView(UnitView unitView)
        {
            m_UnitViews.Remove(unitView);
            Destroy(unitView.gameObject);
        }
    }
}
