using System.Collections;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyMovementEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
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
                var enemyTargetEntityViews = entityViewsDB.QueryEntityViews<EnemyTargetEntityView>();

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

        public void Step(ref DamageInfo token, int condition)
        {
            StopEnemyOnDeath(token.entityDamagedID);
        }
    }
}
