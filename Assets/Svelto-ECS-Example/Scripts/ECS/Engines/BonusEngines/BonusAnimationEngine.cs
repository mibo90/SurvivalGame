using System;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Bonus
{
    public class BonusAnimationEngine : IQueryingEntityViewEngine, IStep<BonusInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        { }

        void TriggerHealthCollectAnimation(int entityID)
        {
            var entity = entityViewsDB.QueryEntityView<BonusHealthEntityView>(entityID);
            entity.animationComponent.trigger = "Collected";
        }
        void TriggerAmmoCollectAnimation(int entityID)
        {
            var entity = entityViewsDB.QueryEntityView<BonusAmmoEntityView>(entityID);
            entity.animationComponent.trigger = "Collected";
        }

        public void Step(ref BonusInfo token, int condition)
        {
            if (token.bonusType == BonusType.health)
                TriggerHealthCollectAnimation(token.bonusEntityID);
            else
                TriggerAmmoCollectAnimation(token.bonusEntityID);
        }
    }
}