namespace Svelto.ECS.Example.Survive.Bonus
{
    public class BonusCollectedEngine : IStep<healthBonusInfo>, IEngine
    {
        public BonusCollectedEngine(IEntityFunctions entityFunctions)
        {
            _entityFunctions = entityFunctions;
        }

        public void Step(ref healthBonusInfo token, int condition)
        {
            _entityFunctions.RemoveEntity(token.bonusEntityID);
        }

        readonly IEntityFunctions _entityFunctions;
    }
}