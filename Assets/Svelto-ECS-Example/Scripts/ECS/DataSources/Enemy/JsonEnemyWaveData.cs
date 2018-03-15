using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    [Serializable]
    public class JSonEnemyWaveData
    {
        public int numberOfEnemiesToSpawn;
        public float enemySpeedPercentageGain;

        public JSonEnemyWaveData(EnemyWaveData waveData)
        {
            numberOfEnemiesToSpawn = waveData.numberOfEnemiesToSpawn;
            enemySpeedPercentageGain = waveData.enemySpeedPercentageGain;
        }
    }

    [Serializable]
    public class EnemyWaveData
    {
        [Tooltip("Number of enemies to spawn this wave")]
        public int numberOfEnemiesToSpawn;
        [Tooltip("The percentage speed gain of the enemies of this wave")]
        public float enemySpeedPercentageGain;

        public EnemyWaveData(JSonEnemyWaveData jsonWaveData)
        {
            numberOfEnemiesToSpawn = jsonWaveData.numberOfEnemiesToSpawn;
            enemySpeedPercentageGain = jsonWaveData.enemySpeedPercentageGain;
        }
    }

}