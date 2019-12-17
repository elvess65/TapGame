namespace WhaleAppTapGame.Logic.Entities
{
    public class DestroyableEntity
    {
        public event System.Action<DestroyableEntity> OnEntityDestroyed;

        private int m_CurrentHP;
        private int m_MaxHP;

        public DestroyableEntity(int hp)
        {
            m_CurrentHP = m_MaxHP = hp;
        }

        public void TakeDamage()
        {
            m_CurrentHP--;

            if (m_CurrentHP <= 0)
                OnEntityDestroyed?.Invoke(this);
        }
    }
}
