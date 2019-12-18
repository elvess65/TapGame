using UnityEngine;

namespace WhaleAppTapGame.Logic.Entities
{
    public interface iUnitEntity
    {
        event System.Action<iUnitEntity> OnEntityDestroyed;
        event System.Action<int> OnHPChanged;

        int CurrentHP { get; }
        int MaxHP { get; }
        int Damage { get; }
        Color HightlightColor { get; }

        void TakeDamage(int damage);
    }
}
