
namespace Svelto.ECS.Example.Survive
{
        public interface IBonusTriggerComponent : IComponent
        {
            BonusCollisionData entityInRange { get; }
        }

        public struct BonusCollisionData
        {
            public int otherEntityID;
            public bool collides;

            public BonusCollisionData(int otherEntityID, bool collides)
            {
                this.otherEntityID = otherEntityID;
                this.collides = collides;
            }
        }

        public interface IDestroyComponent
        {
            DispatchOnChange<bool> destroyed { get; }
        }
        
        public interface IBonusAmountComponent
        {
            int amount { get; }
        }

    public struct BonusInfo
    {
        public int targetEntityID;
        public int bonusEntityID;
        public int amount;
        public BonusType bonusType;

        public BonusInfo(int targetEntityID, int bonusEntityID,  int amount, BonusType bonusType)
        {
            this.targetEntityID = targetEntityID;
            this.bonusEntityID = bonusEntityID;
            this.amount = amount;
            this.bonusType = bonusType;
        }
    }
}
