using Svelto.Tasks.Enumerators;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyMovementEngine : IQueryingEntityViewEngine, IStep<DamageInfo>, IStep<EnemyMovementInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
            Tick().Run();
        }

        IEnumerator Tick()
        {
            while (true)
            {
                var enemyTargetEntityViews = entityViewsDB.QueryEntityViews<TargetEntityView>();

                if (enemyTargetEntityViews.Count > 0)
                {
                    var targetEntityView = enemyTargetEntityViews[0];

                    var enemies = entityViewsDB.QueryEntityViews<EnemyEntityView>();

                    for (var i = 0; i < enemies.Count; i++)
                    {
                        var component = enemies[i].movementComponent;

                        component.navMeshDestination = targetEntityView.targetPositionComponent.position;
                    }
                }

                yield return null;
            }
        }

        void StopEnemyOnDeath(int targetID)
        {
            EnemyEntityView entityView = entityViewsDB.QueryEntityView<EnemyEntityView>(targetID);
            
            entityView.movementComponent.navMeshEnabled = false;
            entityView.movementComponent.setCapsuleAsTrigger = true;
            entityView.rigidBodyComponent.isKinematic = true;
        }

        IEnumerator ModifyMovementSpeed(EnemyMovementInfo info)
        {
            int targetId = info.entityID;
            EnemyEntityView entityView=null;

            // I dont feel like this is a good solution, but I'm not sure how else I can know when the entities have 
            // finnished building.
            while (entityView == null)
            {
                yield return null;
                entityViewsDB.TryQueryEntityView(targetId, out entityView);
            }

            entityView.movementComponent.moveSpeed += info.movementSpeed;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            StopEnemyOnDeath(token.entityDamagedID);
        }

        public void Step(ref EnemyMovementInfo token, int condition)
        {
            ModifyMovementSpeed(token).Run();
        }

    }
}
