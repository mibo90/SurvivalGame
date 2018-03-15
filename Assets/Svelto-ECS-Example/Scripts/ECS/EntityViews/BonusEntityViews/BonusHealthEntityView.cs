namespace Svelto.ECS.Example.Survive.Bonus
{
    public class BonusHealthEntityView : EntityView
    {
        public IBonusTriggerComponent targetTriggerComponent;
        public IPositionComponent positionComponent;
        public IDestroyComponent destroyComponent;
        public IBonusAmountComponent bonusAmountComponent;
        public IAnimationComponent animationComponent;
    }
}