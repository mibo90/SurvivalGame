using System.Collections;
using System.Collections.Generic;
using Svelto.Tasks.Enumerators;
using System.IO;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemySpawnerEngine : IEngine, IStep<EnemyWaveData>, IStep<DamageInfo>
    {
        public EnemySpawnerEngine(ISequencer enemyWaveSequence, ISequencer enemySpawnSequence, 
            Factories.IGameObjectFactory gameobjectFactory, IEntityFactory entityFactory)
        {
            _gameobjectFactory = gameobjectFactory;
            _entityFactory = entityFactory;
            this.enemyWaveSequence = enemyWaveSequence;
            this.enemySpawnSequence = enemySpawnSequence;

            IntervaledTick().Run();
        }

        IEnumerator IntervaledTick()
        {
//OK this is of fundamental importance: Never create implementors as Monobehaviour just to hold 
//data. Data should always been retrieved through a service layer regardless the data source.
//The benefit are numerous, including the fact that changing data source would require
//only changing the service code. In this simple example I am not using a Service Layer
//but you can see the point.          
//Also note that I am loading the data only once per application run, outside the 
//main loop. You can always exploit this trick when you now that the data you need
//to use will never change            
            var enemiestoSpawn = ReadEnemySpawningDataServiceRequest();

            while (!levelFinnished)
            {
//Svelto.Tasks allows to yield UnityYield instructions but this comes with a performance hit
//so the fastest solution is always to use custom enumerators. To be honest the hit is minimal
//but it's better to not abuse it.                
                yield return _waitForSecondsEnumerator;

                if (waveNumberOfEnemies == _numberOfEnemyKilled)
                {
                    wave++;
                    if (lastwave) { levelFinnished = true; }
                    enemyWaveSequence.Next(this, ref wave);
                    yield return _wavewaitForSecondsEnumerator;

                    foreach (var spawnData in enemiestoSpawn)
                    {
                        // The spawn time becomes normale spawntime + wavemodiefier times wave-1(wave-1 so the first wave is not
                        //affected by the modiefier).
                        float newSpawnTime = spawnData.spawnTime + (spawnData.waveSpawnModiefier * (wave - 1));
                        // make sure the spawntime is always higher then 0
                        if (newSpawnTime > 0)
                            spawnData.spawnTime = spawnData.timeLeft = newSpawnTime;
                        else spawnData.spawnTime = spawnData.timeLeft = 1;
                    }
                }
                   
                if (enemiestoSpawn != null)
                {
                    
                    for (int i = enemiestoSpawn.Length - 1; i >= 0 && _numberOfEnemyToSpawn > 0; --i)
                    {
                        var spawnData = enemiestoSpawn[i];
                        

                        if (spawnData.timeLeft <= 0.0f)
                        {
                            // Find a random index between zero and one less than the number of spawn points.
                            int spawnPointIndex = Random.Range(0, spawnData.spawnPoints.Length);

                            // Create an instance of the enemy prefab at the randomly selected spawn point position and rotation.
                            var go = _gameobjectFactory.Build(spawnData.enemyPrefab);
                            
                            //I have been lazy here and retrieving the data directly from the Monobehaviour
                            //but still I am not creating an implementor for the purpose!
                            var data = go.GetComponent<EnemyAttackDataHolder>();
                            
                            //we are going to use a mix of implementors as Monobehaviours and
                            //normal implementors here:                           
                            List<IImplementor> implementors = new List<IImplementor>();
                            go.GetComponentsInChildren(implementors);
                            implementors.Add(new EnemyAttackImplementor(data.timeBetweenAttacks, data.attackDamage));
                            
                            //In this example every kind of enemy generates the same list of EntityViews
                            //therefore I always use the same EntityDescriptor. However if the 
                            //different enemies had to create different EntityViews for different
                            //engines, this would have been a good example where EntityDescriptorHolder
                            //could have been used to exploit the the kind of polymorphism explained
                            //in my articles.
                            _entityFactory.BuildEntity<EnemyEntityDescriptor>(
                                go.GetInstanceID(), implementors.ToArray());

                            var transform = go.transform;
                            var spawnInfo = spawnData.spawnPoints[spawnPointIndex];

                            transform.position = spawnInfo.position;
                            transform.rotation = spawnInfo.rotation;
                            spawnData.timeLeft = spawnData.spawnTime;
                            EnemyMovementInfo movementInfo = new EnemyMovementInfo(enemySpeedPercentageGain, go.GetInstanceID());
                            enemySpawnSequence.Next(this, ref movementInfo);

                            _numberOfEnemyToSpawn--;
                        }

                        spawnData.timeLeft -= 1.0f;
                    }
                }
            }
        }

        static JSonEnemySpawnData[] ReadEnemySpawningDataServiceRequest()
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/EnemySpawningData.json");
            
            JSonEnemySpawnData[] enemiestoSpawn = JsonHelper.getJsonArray<JSonEnemySpawnData>(json);
            
            return enemiestoSpawn;
        }

        public void Step(ref EnemyWaveData token, int condition)
        {
            waveNumberOfEnemies =_numberOfEnemyToSpawn = token.numberOfEnemiesToSpawn;
            enemySpeedPercentageGain = token.enemySpeedPercentageGain;
            _numberOfEnemyKilled = 0;
            //wave++;
            if (condition == WaveCondition.Last)
            {
                lastwave = true;
            }
        }

        public void Step(ref DamageInfo token, int condition)
        {
            _numberOfEnemyKilled++;
        }

        readonly Factories.IGameObjectFactory   _gameobjectFactory;
        readonly IEntityFactory                 _entityFactory;
        readonly WaitForSecondsEnumerator       _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);
        readonly WaitForSecondsEnumerator       _wavewaitForSecondsEnumerator = new WaitForSecondsEnumerator(3);

        ISequencer enemyWaveSequence;
        ISequencer enemySpawnSequence;
        int     _numberOfEnemyToSpawn;
        int     waveNumberOfEnemies;
        int     _numberOfEnemyKilled;
        float enemySpeedPercentageGain;
        bool levelFinnished = false;
        bool lastwave = false;
        int wave = 0;
    }
}
