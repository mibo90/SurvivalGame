namespace Svelto.ECS.Example.Survive.Sound
{
    public class BonusSoundEngine : IQueryingEntityViewEngine, IStep<healthBonusInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        { }

        void TriggerCollectAudio(int sender)
        {
            var audioEntityView = entityViewsDB.QueryEntityView<BonusSoundEntityView>(sender);
            var audioComponent = audioEntityView.audioComponent;

            audioComponent.playOneShot = AudioType.collect;
        }

        public void Step(ref healthBonusInfo token, int condition)
        {
            TriggerCollectAudio(token.bonusEntityID);
        }
    }
}