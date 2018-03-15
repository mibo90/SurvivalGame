using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    [Serializable]
    public class JSonBonusHealthSpawnData
    {
        public GameObject healthBonusPrefab;
        public SpawningStruct[] spawnPoints;

        public JSonBonusHealthSpawnData(BonusHealthSpawnData spawnData)
        {
            healthBonusPrefab = spawnData.healthBonusPrefab;
            spawnPoints = new SpawningStruct[spawnData.spawnPoints.Length];

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                spawnPoints[i].position = spawnData.spawnPoints[i].position;
                spawnPoints[i].rotation = spawnData.spawnPoints[i].rotation;
            }
        }
    }

    [Serializable]
    public class BonusHealthSpawnData
    {
        public GameObject healthBonusPrefab;
        public Transform[] spawnPoints;
    }

}