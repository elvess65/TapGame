namespace WhaleAppTapGame.Logic.Entities
{
    public class DestroyableEntity : iDestroyableEntity
    {
        public event System.Action<iDestroyableEntity> OnEntityDestroyed;

        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }


        public DestroyableEntity(int hp)
        {
            CurrentHP = MaxHP = hp;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;

            if (CurrentHP <= 0)
                OnEntityDestroyed?.Invoke(this);
        }
    }
}
