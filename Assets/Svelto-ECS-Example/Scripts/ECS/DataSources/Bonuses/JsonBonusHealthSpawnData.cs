using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    [Serializable]
    public class JSonBonusSpawnData
    {
        public GameObject bonusPrefab;
        public SpawningStruct[] spawnPoints;

        public JSonBonusSpawnData(BonusSpawnData spawnData)
        {
            bonusPrefab = spawnData.bonusPrefab;
            spawnPoints = new SpawningStruct[spawnData.spawnPoints.Length];

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                spawnPoints[i].position = spawnData.spawnPoints[i].position;
                spawnPoints[i].rotation = spawnData.spawnPoints[i].rotation;
            }
        }
    }

    [Serializable]
    public class BonusSpawnData
    {
        public GameObject bonusPrefab;
        public Transform[] spawnPoints;
    }

}