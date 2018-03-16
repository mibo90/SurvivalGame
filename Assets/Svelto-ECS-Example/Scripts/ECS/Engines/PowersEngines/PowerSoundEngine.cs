using Svelto.ECS.Example.Survive.Player.Power;
using Svelto.Tasks.Enumerators;
using System.Collections;
namespace Svelto.ECS.Example.Survive.Sound
{
    public class PowerSoundEngine : IQueryingEntityViewEngine,IStep<PowerInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        { }

        void TriggerPowerAudio(PowerInfo info)
        {
            var audioEntityView = entityViewsDB.QueryEntityView<PowerSoundEntityView>(info.entityID);

            var audioComponent = audioEntityView.entitySoundComponent;

            audioComponent.playOneShot = info.audioType;
        }
        public void Step(ref PowerInfo token, int condition)
        {
            TriggerPowerAudio(token);
        }
    }
}