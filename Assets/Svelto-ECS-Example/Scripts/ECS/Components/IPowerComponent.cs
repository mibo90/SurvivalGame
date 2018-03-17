
namespace Svelto.ECS.Example.Survive
{
    public interface IPowerComponent: IComponent
    {
         int duration { get;}
         int cooldown { get;}
         int speed { get; }
         float timer { get; set; }  
    }

    public struct PowerInfo
    {
        public int entityID;
        public AudioType audioType;
        public int cooldown;

        public PowerInfo(int entityID, AudioType audioType, int cooldown)
        {
            this.entityID = entityID;
            this.audioType = audioType;
            this.cooldown = cooldown;
        }
    }
}
