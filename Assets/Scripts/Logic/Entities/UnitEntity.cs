using System;
using UnityEngine;

namespace WhaleAppTapGame.Logic.Entities
{
    public class UnitEntity : iUnitEntity
    {
        public event Action<iUnitEntity> OnEntityDestroyed;
        public event Action<int> OnHPChanged;

        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int Damage { get; private set; }
        public Color HightlightColor { get; private set; }

        public UnitEntity(int hp, int damage, Color hightlightColor)
        {
            CurrentHP = MaxHP = hp;
            Damage = damage;
            HightlightColor = hightlightColor;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;

            OnHPChanged?.Invoke(CurrentHP);

            if (CurrentHP <= 0)
                OnEntityDestroyed?.Invoke(this);
        }
    }
}
