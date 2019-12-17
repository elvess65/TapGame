using UnityEngine;
using WhaleAppTapGame.Logic.Entities;
using WhaleAppTapGame.Logic.Movement;

namespace WhaleAppTapGame.Logic.View
{
    public class UnitView : MonoBehaviour
    {
        public event System.Action<UnitView> OnUnitDestroyed;
        public event System.Action<UnitView> OnUnitOutOfBottomBound;

        public SpriteRenderer MainSpriteRenderer;
        public SpriteRenderer HightlightSpriteRenderer;

        private iDestroyableEntity m_Entity;
        private iMoveStrategy m_MoveStrategy;

        public float SpriteHalfWidth => MainSpriteRenderer.bounds.size.x / 2;
        public float SpriteHalfHeight => MainSpriteRenderer.bounds.size.y / 2;
        public int Damage => m_Entity.Damage;


        public void Init(iDestroyableEntity entity, iMoveStrategy moveStrategy)
        {
            //Entity
            m_Entity = entity;
            m_Entity.OnEntityDestroyed += (iDestroyableEntity sender) => OnUnitDestroyed?.Invoke(this);

            //Move strategy
            m_MoveStrategy = moveStrategy;
            m_MoveStrategy.OnOutOfBottomBound += () => OnUnitOutOfBottomBound?.Invoke(this);
        }

        
        public void TakeDamage(int damage) => m_Entity.TakeDamage(damage);

        public void Move() => m_MoveStrategy?.Move();
    }
}
