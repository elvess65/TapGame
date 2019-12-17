using UnityEngine;

namespace WhaleAppTapGame.Logic.Entities
{
    //TODO renamge iUnitEntity
    public interface iDestroyableEntity
    {
        event System.Action<iDestroyableEntity> OnEntityDestroyed;

        int CurrentHP { get; }
        int MaxHP { get; }
        int Damage { get; }
        Color HightlightColor { get; }

        void TakeDamage(int damage);
    }
}
