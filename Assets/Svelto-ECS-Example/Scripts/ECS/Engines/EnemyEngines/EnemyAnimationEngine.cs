using System;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyAnimationEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {}

        void EntityDamaged(DamageInfo damageInfo)
        {
            var entity = entityViewsDB.QueryEntityView<EnemyEntityView>(damageInfo.entityDamagedID);

            entity.vfxComponent.position = damageInfo.damagePoint;
            entity.vfxComponent.play.value = true;
        }

        void TriggerTargetDeathAnimation()
        {
            var entity = entityViewsDB.QueryEntityViews<EnemyEntityView>();

            for (int i = 0; i < entity.Count; i++)
                entity[i].animationComponent.playAnimation = "PlayerDead";
        }

        void TriggerDeathAnimation(int targetID)
        {
            var entity = entityViewsDB.QueryEntityView<EnemyEntityView>(targetID);
            
            entity.animationComponent.playAnimation = "Dead";
        }

        public void Step(ref DamageInfo token, int condition)
        {
            if (token.entityType == EntityDamagedType.Enemy)
            {
                //if enemy dies
                if (condition == DamageCondition.Dead)
                    TriggerDeathAnimation(token.entityDamagedID);
                else
                //if enemy is damaged
                    EntityDamaged(token);
            }
            else
            {
                //is player is dead, the enemy cheers
                if (condition == DamageCondition.Dead)
                    TriggerTargetDeathAnimation();    
            }
        }
    }
}
