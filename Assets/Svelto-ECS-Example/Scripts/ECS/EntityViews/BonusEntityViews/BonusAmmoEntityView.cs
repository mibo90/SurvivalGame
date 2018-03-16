namespace Svelto.ECS.Example.Survive.Bonus
{
    public class BonusAmmoEntityView : EntityView
    {
        public IBonusTriggerComponent targetTriggerComponent;
        public IPositionComponent positionComponent;
        public IDestroyComponent destroyComponent;
        public IBonusAmountComponent bonusAmountComponent;
        public IAnimationComponent animationComponent;
    }
}