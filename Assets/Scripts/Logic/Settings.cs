using UnityEngine;

namespace WhaleAppTapGame.Logic
{
    [System.Serializable]
    public class Settings 
    {
        [System.Serializable]
        public class UnitEntitySettings
        {
            public int HP;
            public int Damage;
            public Color HightlightColor;
        }

        [System.Serializable]
        public class SpawnSettings
        {
            public float FirstSpawnDelay;
            public float SpawnPeriod;
        }

        [System.Serializable]
        public class ScoreSettings
        {
            public int ScorePerEnemy;
        }
    }
}
