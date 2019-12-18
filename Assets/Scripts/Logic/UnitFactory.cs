using UnityEngine;
using WhaleAppTapGame.Logic.Entities;
using WhaleAppTapGame.Logic.Movement;
using WhaleAppTapGame.Logic.View;

namespace WhaleAppTapGame.Logic
{
    public abstract class UnitFactory
    {
        protected int m_HP;
        protected int m_Damage;
        protected Vector2 m_ScreenBounds;

        private UnitView m_UnitSource;
        private Vector2 m_SpeedRange;

        private System.Action<UnitView> m_OnUnitDestroyed;
        private System.Action<UnitView> m_OnUnitOutOfBottomBound;


        public UnitFactory(UnitView unitSource, Vector2 screenBounds, Vector2 speedRange, int hp, int damage, System.Action<UnitView> onUnitOutOfBottomBound, System.Action<UnitView> onUnitDestroyed)
        {
            m_UnitSource = unitSource;
            m_ScreenBounds = screenBounds;
            m_SpeedRange = speedRange;
            m_HP = hp;
            m_Damage = damage;
            m_OnUnitDestroyed = onUnitDestroyed;
            m_OnUnitOutOfBottomBound = onUnitOutOfBottomBound;
        }

        public UnitView CreateUnit()
        {
            UnitView unitView = MonoBehaviour.Instantiate(m_UnitSource);

            float minSpawnPosX = -m_ScreenBounds.x - unitView.SpriteHalfWidth + 1;
            float maxSpawnPosX = m_ScreenBounds.x + unitView.SpriteHalfWidth - 1;
            float spawnPosX = Random.Range(minSpawnPosX, maxSpawnPosX);
            float spawnPosY = m_ScreenBounds.y + unitView.SpriteHalfHeight;

            unitView.transform.position = new Vector3(spawnPosX, spawnPosY, 0);
            unitView.Init(CreateEntity(), CreateMovement(unitView.transform,
                                                         new Vector2(unitView.SpriteHalfWidth, unitView.SpriteHalfHeight),
                                                         Random.Range(m_SpeedRange.x, m_SpeedRange.y)));

            unitView.OnUnitOutOfBottomBound += m_OnUnitOutOfBottomBound;
            unitView.OnUnitDestroyed += m_OnUnitDestroyed;

            return unitView;
        }

        protected abstract iUnitEntity CreateEntity();
        protected abstract iMoveStrategy CreateMovement(Transform controlledTransform, Vector2 viewSpriteBounds, float speed);
    }

    public class SimpleEnemyUnitFactory : UnitFactory
    {
        public SimpleEnemyUnitFactory(UnitView unitSource, Vector2 screenBounds, Vector2 speedRange, int hp, int damage, System.Action<UnitView> onUnitOutOfBottomBound, System.Action<UnitView> onUnitDestroyed) : 
            base(unitSource, screenBounds, speedRange, hp, damage, onUnitOutOfBottomBound, onUnitDestroyed)
        { }

        protected override iUnitEntity CreateEntity() => new UnitEntity(m_HP, m_Damage, Color.red);

        protected override iMoveStrategy CreateMovement(Transform controlledTransform, Vector2 viewSpriteBounds, float speed) =>
            new SimpleMoveStrategy(controlledTransform, speed, m_ScreenBounds, viewSpriteBounds);
    }

    public class DiagonalEnemyUnitFactory : SimpleEnemyUnitFactory
    {
        private int m_ChanceToMoveDiagonal;

        public DiagonalEnemyUnitFactory(UnitView unitSource, Vector2 screenBounds, Vector2 speedRange, int hp, int damage, int chanceToMoveDiagonal, System.Action<UnitView> onUnitOutOfBottomBound, System.Action<UnitView> onUnitDestroyed) : 
            base(unitSource, screenBounds, speedRange, hp, damage, onUnitOutOfBottomBound, onUnitDestroyed)
        {
            m_ChanceToMoveDiagonal = chanceToMoveDiagonal;
        }

        protected override iMoveStrategy CreateMovement(Transform controlledTransform, Vector2 viewSpriteBounds, float speed)
        {
            if (Random.Range(0, 100) > m_ChanceToMoveDiagonal)
                return new DiagonalMoveStrategy(controlledTransform, speed, m_ScreenBounds, viewSpriteBounds);

            return base.CreateMovement(controlledTransform, viewSpriteBounds, speed);
        }
    }

    public class FriendlyUnitFactory : DiagonalEnemyUnitFactory
    {
        public FriendlyUnitFactory(UnitView unitSource, Vector2 screenBounds, Vector2 speedRange, int hp, int chanceToMoveDiagonal, System.Action<UnitView> onUnitOutOfBottomBound, System.Action<UnitView> onUnitDestroyed) :
            base(unitSource, screenBounds, speedRange, hp, 0, chanceToMoveDiagonal, onUnitOutOfBottomBound, onUnitDestroyed)
        {
        }

        protected override iUnitEntity CreateEntity() => new FriendlyUnitEntity(m_HP, Color.green);
    }
}
