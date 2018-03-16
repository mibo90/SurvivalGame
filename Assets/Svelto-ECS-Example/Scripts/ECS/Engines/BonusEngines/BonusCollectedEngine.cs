namespace Svelto.ECS.Example.Survive.Bonus
{
    public class BonusCollectedEngine : IStep<BonusInfo>, IEngine
    {
        public BonusCollectedEngine(IEntityFunctions entityFunctions)
        {
            _entityFunctions = entityFunctions;
        }

        public void Step(ref BonusInfo token, int condition)
        {
            _entityFunctions.RemoveEntity(token.bonusEntityID);
        }

        readonly IEntityFunctions _entityFunctions;
    }
}