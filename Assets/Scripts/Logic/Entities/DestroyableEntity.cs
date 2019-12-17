namespace WhaleAppTapGame.Logic.Entities
{
    public class DestroyableEntity : iDestroyableEntity
    {
        public event System.Action<iDestroyableEntity> OnEntityDestroyed;

        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }
        public int Damage { get; private set; }


        public DestroyableEntity(int hp, int damage)
        {
            CurrentHP = MaxHP = hp;
            Damage = damage;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;

            if (CurrentHP <= 0)
                OnEntityDestroyed?.Invoke(this);
        }
    }
}
