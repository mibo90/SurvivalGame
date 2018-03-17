using Svelto.ECS.Example.Survive.Bonus;
using Svelto.Tasks.Enumerators;
using System.Collections;
namespace Svelto.ECS.Example.Survive.Sound
{
    public class BonusSoundEngine : IQueryingEntityViewEngine, IStep<BonusInfo>, IStep<int>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        { }

        void TriggerCollectAudio(BonusInfo info)
        {
            var audioEntityView = entityViewsDB.QueryEntityView<BonusSoundEntityView>(info.bonusEntityID);
           
            var audioComponent = audioEntityView.audioComponent;

            audioComponent.playOneShot = AudioType.collect;
            if (info.bonusType ==BonusType.health)
            DestroyHEntity(info.bonusEntityID,audioComponent).Run();
            else
                DestroyAEntity(info.bonusEntityID, audioComponent).Run();
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
        IEnumerator DestroyHEntity(int sender,IEntitySoundComponent audioComponent)
        {
            var bonusHEntityView = entityViewsDB.QueryEntityView<BonusHealthEntityView>(sender);
            while (audioComponent.isPlaying)
            {
                yield return null;
            }
            bonusHEntityView.destroyComponent.destroyed.value = true;
        }
        IEnumerator DestroyAEntity(int sender, IEntitySoundComponent audioComponent)
        {
            var bonusAEntityView = entityViewsDB.QueryEntityView<BonusAmmoEntityView>(sender);
            while (audioComponent.isPlaying)
            {
                yield return null;
            }
            bonusAEntityView.destroyComponent.destroyed.value = true;
        }

        public void Step(ref BonusInfo token, int condition)
        {
            TriggerCollectAudio(token);
        }

        public void Step(ref int token, int condition)
        {
            TriggerSpawnAudio(token).Run();
        }

        readonly WaitForSecondsEnumerator _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);
    }
}