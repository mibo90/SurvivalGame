namespace Svelto.ECS.Example.Survive.HUD
{
    public class HUDEntityView : EntityView
    {
        public IAnimationComponent          HUDAnimator;
        public IDamageHUDComponent          damageImageComponent;
        public IHealthSliderComponent       healthSliderComponent;
        public IScoreComponent              scoreComponent;
        public IEnemyCountComponent         enemyCountComponent;
        public IPowerFilledImageComponent   powerFilledImageComponent;
        public IBulletCountComponent        bulletCountComponent;
    }
}
