namespace Svelto.ECS.Example.Survive.Bonus
{
    public class BonusHealthEntityView : EntityView
    {
        public IBonusHealthTriggerComponent targetTriggerComponent;
        public IPositionComponent positionComponent;
        public IDestroyComponent destroyComponent;
        public IBonusAmountComponent bonusAmountComponent;
    }

    //public class BonusTargetEntityView : EntityView
    //{
    //    public IPositionComponent targetPositionComponent;
    //}
}