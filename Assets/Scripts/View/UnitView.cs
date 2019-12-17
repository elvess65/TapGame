using UnityEngine;
using WhaleAppTapGame.Logic.Entities;

namespace WhaleAppTapGame.Logic.View
{
    public class UnitView : MonoBehaviour
    {
        public event System.Action<DestroyableEntity> OnEntityDestroyed;

        public SpriteRenderer MainSpriteRenderer;
        public SpriteRenderer HightlightSpriteRenderer;

        private DestroyableEntity m_Entity;


        public void Init(DestroyableEntity entity)
        {
            m_Entity = entity;
            OnEntityDestroyed += EnemyView_EntityDestroyedHandler;
        }

        public void TakeDamage() => m_Entity.TakeDamage();


        void EnemyView_EntityDestroyedHandler(DestroyableEntity sender)
        {
            OnEntityDestroyed?.Invoke(sender);
        }
    }
}
