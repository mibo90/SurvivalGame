using System;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Bonus
{
    public class BonusAnimationEngine : IQueryingEntityViewEngine, IStep<healthBonusInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        { }

        void TriggerHealthCollectAnimation(int entityID)
        {
            var entity = entityViewsDB.QueryEntityView<BonusHealthEntityView>(entityID);
            entity.animationComponent.trigger = "Collected";
        }

        public void Step(ref healthBonusInfo token, int condition)
        {
            TriggerHealthCollectAnimation(token.bonusEntityID);
        }
    }
}