using System;
using System.IO;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    class EnemyWaveEngine: IEngine, IStep<int>
    {
        readonly ISequencer enemyWaveSequence;
        readonly EnemyWaveData[] wavesData;

        public EnemyWaveEngine(Sequencer enemyWaveSequence)
        {
            this.enemyWaveSequence = enemyWaveSequence;
            JSonEnemyWaveData[] jSonEnemyWave = ReadEnemyWaveDataServiceRequest();
            wavesData = new EnemyWaveData[jSonEnemyWave.Length];
            for (int i = 0; i < jSonEnemyWave.Length; i++)
            {
                wavesData[i] = new EnemyWaveData(jSonEnemyWave[i]);
            }
        }

        public void Step(ref int token, int condition)
        {
            if (wavesData.Length > token)
                enemyWaveSequence.Next(this, ref wavesData[token - 1], WaveCondition.Next);
            else if(wavesData.Length == token)
                enemyWaveSequence.Next(this, ref wavesData[token - 1], WaveCondition.Last);
            else
            {
                enemyWaveSequence.Next(this, ref wavesData[token - 2], WaveCondition.Stop);
            }
        }

        static JSonEnemyWaveData[] ReadEnemyWaveDataServiceRequest()
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/EnemyWavesData.json");

            JSonEnemyWaveData[] enemyWaves = JsonHelper.getJsonArray<JSonEnemyWaveData>(json);

            return enemyWaves;
        }
    }
}
