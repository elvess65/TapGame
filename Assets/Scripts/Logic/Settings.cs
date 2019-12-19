using UnityEngine;

namespace WhaleAppTapGame.Logic
{
    [System.Serializable]
    public class Settings 
    {
        [System.Serializable]
        public class UnitEntitySettings
        {
            public Vector2Int HP_Damage;
            public Color HightlightColor;
        }

        [System.Serializable]
        public class EnemyUnitEntitySettings : UnitEntitySettings
        {
            public Vector2 SpeedRange;
        }

        [System.Serializable]
        public class DiagonalEnemyUnitEntitySettings : EnemyUnitEntitySettings
        {
            [Range(0, 100)]
            public int ChanceToMoveDiagonal;
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
