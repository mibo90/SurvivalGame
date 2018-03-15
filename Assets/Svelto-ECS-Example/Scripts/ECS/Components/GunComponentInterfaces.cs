using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player.Gun
{
    public interface IGunAttributesComponent: IComponent
    {
        float   timeBetweenBullets { get; }
        Ray     shootRay           { get; }
        float   range              { get; }
        int     damagePerShot      { get; }
        int     magazineCapacity   { get; }
        int     currentBulletCount { get; set; }
        float   timer              { get; set; }
        Vector3 lastTargetPosition { get; set; }
    }

    public struct GunInfo
    {
        public int magazineCapacity { get; private set; }
        public int currentBulletCount { get; private set; }
        public GunInfo(int magazineCapacity, int currentBulletCount)
        {
            this.magazineCapacity = magazineCapacity;
            this.currentBulletCount = currentBulletCount;
        }
    } 

    public interface IGunHitTargetComponent : IComponent
    {
        DispatchOnSet<bool> targetHit { get; }
    }

    public interface IGunFXComponent: IComponent
    {
        float   effectsDisplayTime { get; }
        Vector3 lineEndPosition    { set; }
        Vector3 lineStartPosition  { set; }
        bool    lineEnabled        { set; }
        bool    play               { set; }
        bool    lightEnabled       { set; }
        bool    playAudio          { set; }
    }
}