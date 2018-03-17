using System.Collections;
using System.Collections.Generic;
using Svelto.Tasks.Enumerators;
using System.IO;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Bonus
{
    public class BonusAmmoSpawnerEngine : IEngine
    {
        public BonusAmmoSpawnerEngine(ISequencer bonusSpawnerSequence, Factories.IGameObjectFactory gameobjectFactory,
            IEntityFactory entityFactory)
        {
            _bonusSpawnerSequence = bonusSpawnerSequence;
            _gameobjectFactory = gameobjectFactory;
            _entityFactory = entityFactory;
            IntervaledTick().Run();
        }

        IEnumerator IntervaledTick()
        {

            var bonusAmmoToSpawn = ReadEnemySpawningDataServiceRequest();

            while (true)
            {
                _waitForSecondsEnumerator.Reset(Random.Range(10, 30));
                yield return _waitForSecondsEnumerator;

                if (bonusAmmoToSpawn != null)
                {

                    var spawnData = bonusAmmoToSpawn[Random.Range(0, bonusAmmoToSpawn.Length)];

                    // Find a random index between zero and one less than the number of spawn points.
                    int spawnPointIndex = Random.Range(0, spawnData.spawnPoints.Length);

                    // Create an instance of the enemy prefab at the randomly selected spawn point position and rotation.
                    var go = _gameobjectFactory.Build(spawnData.bonusPrefab);

                    List<IImplementor> implementors = new List<IImplementor>();
                    go.GetComponentsInChildren(implementors);

                    _entityFactory.BuildEntity<BonusAmmoEntityDescriptor>(
                        go.GetInstanceID(), implementors.ToArray());

                    var transform = go.transform;
                    var spawnInfo = spawnData.spawnPoints[spawnPointIndex];

                    transform.position = spawnInfo.position;
                    transform.rotation = spawnInfo.rotation;
                    int ID = go.GetInstanceID();
                    _bonusSpawnerSequence.Next(this, ref ID);
                }
            }
        }

        static JSonBonusSpawnData[] ReadEnemySpawningDataServiceRequest()
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/BonusAmmoSpawningData.json");

            JSonBonusSpawnData[] bonusHealthtoSpawn = JsonHelper.getJsonArray<JSonBonusSpawnData>(json);

            return bonusHealthtoSpawn;
        }


        readonly Factories.IGameObjectFactory _gameobjectFactory;
        readonly IEntityFactory _entityFactory;
        readonly WaitForSecondsEnumerator _waitForSecondsEnumerator = new WaitForSecondsEnumerator(10);

        readonly ISequencer _bonusSpawnerSequence;

    }
}
