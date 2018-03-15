using Svelto.ECS.Example.Survive.Bonus;
using Svelto.Tasks.Enumerators;
using System.Collections;
namespace Svelto.ECS.Example.Survive.Sound
{
    public class BonusSoundEngine : IQueryingEntityViewEngine, IStep<healthBonusInfo>, IStep<int>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        { }

        void TriggerCollectAudio(int sender)
        {
            var audioEntityView = entityViewsDB.QueryEntityView<BonusSoundEntityView>(sender);
           
            var audioComponent = audioEntityView.audioComponent;

            audioComponent.playOneShot = AudioType.collect;
            DestroyEntity(sender).Run();
        }
        IEnumerator TriggerSpawnAudio(int sender)
        {
            BonusSoundEntityView audioEntityView =null;
            while (audioEntityView == null)
            {
                yield return null;
                entityViewsDB.TryQueryEntityView(sender,out audioEntityView);
            }

            var audioComponent = audioEntityView.audioComponent;

            audioComponent.playOneShot = AudioType.spawn;
        }
        // destrorys the bonus entity after the sound has finnished playing on it.
        IEnumerator DestroyEntity(int sender)
        {
            var bonusHEntityView = entityViewsDB.QueryEntityView<BonusHealthEntityView>(sender);
            var audioEntityView = entityViewsDB.QueryEntityView<BonusSoundEntityView>(sender);
            while (audioEntityView.audioComponent.isPlaying)
            {
                yield return null;
            }
            bonusHEntityView.destroyComponent.destroyed.value = true;
        } 

        public void Step(ref healthBonusInfo token, int condition)
        {
            TriggerCollectAudio(token.bonusEntityID);
        }

        public void Step(ref int token, int condition)
        {
            TriggerSpawnAudio(token).Run();
        }

        readonly WaitForSecondsEnumerator _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);
    }
}