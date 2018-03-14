namespace Svelto.ECS.Example.Survive.HUD
{
    public class EnemyCountEngine : SingleEntityViewEngine<HUDEntityView>, IStep<EnemyWaveData>, IStep<DamageInfo>
    {
        public EnemyCountEngine()
        {
        }

        public void Step(ref EnemyWaveData token, int condition)
        {
            _guiEntityView.enemyCountComponent.enemyCount += token.numberOfEnemiesToSpawn;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            _guiEntityView.enemyCountComponent.enemyCount--;
        }

        protected override void Add(HUDEntityView EntityView)
        {
            _guiEntityView = EntityView;
        }

        protected override void Remove(HUDEntityView EntityView)
        {
            _guiEntityView = null;
        }

      

        HUDEntityView _guiEntityView;
    }
}
