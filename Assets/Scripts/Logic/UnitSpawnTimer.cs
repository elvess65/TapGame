using UnityEngine;

namespace WhaleAppTapGame.Logic
{
    public class UnitSpawnTimer
    {
        public event System.Action OnUnitShouldBeSpawned;

        private float m_SpawnPeriod;
        private float m_ToSpawnTime = 0;

        public UnitSpawnTimer(float firstSpawnDelay, float spawnPeriod)
        {
            m_SpawnPeriod = spawnPeriod;
            m_ToSpawnTime = firstSpawnDelay;
        }

        public void UpdateComponent(float deltaTime)
        {
            m_ToSpawnTime -= deltaTime;
            if (m_ToSpawnTime <= 0)
            {
                m_ToSpawnTime = m_SpawnPeriod;

                OnUnitShouldBeSpawned?.Invoke();
            }
        }
    }
}
