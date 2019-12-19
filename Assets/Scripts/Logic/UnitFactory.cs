using UnityEngine;
using WhaleAppTapGame.Logic.Entities;
using WhaleAppTapGame.Logic.Movement;
using WhaleAppTapGame.Logic.View;

namespace WhaleAppTapGame.Logic
{
    public abstract class UnitFactory
    {
        public System.Action<UnitView> OnUnitDestroyed;
        public System.Action<UnitView> OnUnitOutOfBottomBound;

        protected Vector2Int m_HP_Damage;
        protected Color m_HightlightColor;
        protected Vector2 m_ScreenBounds;

        private UnitView m_UnitSource;
        private Vector2 m_SpeedRange;


        public UnitFactory(UnitView unitSource, Vector2 screenBounds, Vector2 speedRange, Color hightlightColor, Vector2Int hp_damage)
        {
            m_HP_Damage = hp_damage;
            m_UnitSource = unitSource;
            m_SpeedRange = speedRange;
            m_ScreenBounds = screenBounds;
            m_HightlightColor = hightlightColor;
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

            unitView.OnUnitOutOfBottomBound += OnUnitOutOfBottomBound;
            unitView.OnUnitDestroyed += OnUnitDestroyed;

            return unitView;
        }

        protected abstract iUnitEntity CreateEntity();
        protected abstract iMoveStrategy CreateMovement(Transform controlledTransform, Vector2 viewSpriteBounds, float speed);
    }

    public class SimpleEnemyUnitFactory : UnitFactory
    {
        public SimpleEnemyUnitFactory(UnitView unitSource, Vector2 screenBounds, Vector2 speedRange, Color hightlightColor, Vector2Int hp_damage) : 
            base(unitSource, screenBounds, speedRange, hightlightColor, hp_damage)
        { }

        protected override iUnitEntity CreateEntity() => new UnitEntity(m_HP_Damage.x, m_HP_Damage.y, m_HightlightColor);

        protected override iMoveStrategy CreateMovement(Transform controlledTransform, Vector2 viewSpriteBounds, float speed) =>
            new SimpleMoveStrategy(controlledTransform, speed, m_ScreenBounds, viewSpriteBounds);
    }

    public class DiagonalEnemyUnitFactory : SimpleEnemyUnitFactory
    {
        private int m_ChanceToMoveDiagonal;

        public DiagonalEnemyUnitFactory(UnitView unitSource, Vector2 screenBounds, Vector2 speedRange, Color hightlightColor, Vector2Int hp_damage, int chanceToMoveDiagonal) : 
            base(unitSource, screenBounds, speedRange, hightlightColor, hp_damage)
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
        public FriendlyUnitFactory(UnitView unitSource, Vector2 screenBounds, Vector2 speedRange, Color hightlightColor, Vector2Int hp_damage, int chanceToMoveDiagonal) :
            base(unitSource, screenBounds, speedRange, hightlightColor, hp_damage, chanceToMoveDiagonal)
        {
        }

        protected override iUnitEntity CreateEntity() => new FriendlyUnitEntity(m_HP_Damage.x, m_HightlightColor);
    }
}
