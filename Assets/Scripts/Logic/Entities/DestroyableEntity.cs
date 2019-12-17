using UnityEngine;

namespace WhaleAppTapGame.Logic.Entities
{
    public class DestroyableEntity : iDestroyableEntity
    {
        public event System.Action<iDestroyableEntity> OnEntityDestroyed;

        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int Damage { get; private set; }
        public Color HightlightColor { get; private set; }

        public DestroyableEntity(int hp, int damage, Color hightlightColor)
        {
            CurrentHP = MaxHP = hp;
            Damage = damage;
            HightlightColor = hightlightColor;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;

            if (CurrentHP <= 0)
                OnEntityDestroyed?.Invoke(this);
        }
    }
}
