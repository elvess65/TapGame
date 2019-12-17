namespace WhaleAppTapGame.Logic.Entities
{
    public interface iDestroyableEntity
    {
        event System.Action<iDestroyableEntity> OnEntityDestroyed;

        int CurrentHP { get; }
        int MaxHP { get; }

        void TakeDamage(int damage);
    }
}
