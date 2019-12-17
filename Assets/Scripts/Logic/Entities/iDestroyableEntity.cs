namespace WhaleAppTapGame.Logic.Entities
{
    //TODO renamge iUnitEntity
    public interface iDestroyableEntity
    {
        event System.Action<iDestroyableEntity> OnEntityDestroyed;

        int CurrentHP { get; }
        int MaxHP { get; }
        int Damage { get; }

        void TakeDamage(int damage);
    }
}
