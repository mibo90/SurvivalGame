using System.Collections;
using UnityEngine;
using Svelto.ECS.Example.Survive.Enemies;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.Player.Gun
{
    public class PlayerGunShootingEngine : MultiEntityViewsEngine<GunEntityView, PlayerEntityView>, 
        IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
            _taskRoutine.Start();
        }
        
        public PlayerGunShootingEngine(EnemyKilledObservable enemyKilledObservable, ISequencer damageSequence, IRayCaster rayCaster, ITime time)
        {
            _enemyKilledObservable = enemyKilledObservable;
            _enemyDamageSequence   = damageSequence;
            _rayCaster             = rayCaster;
            _time                  = time;
            _taskRoutine           = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(Tick())
                                               .SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(GunEntityView entityView)
        {
            _playerGunEntityView = entityView;
        }

        protected override void Remove(GunEntityView entityView)
        {
            _taskRoutine.Stop();
            _playerGunEntityView = null;
        }

        protected override void Add(PlayerEntityView entityView)
        {
            _playerEntityView = entityView;
        }

        protected override void Remove(PlayerEntityView entityView)
        {
            _taskRoutine.Stop();
            _playerEntityView = null;
        }

        IEnumerator Tick()
        {
            while (_playerEntityView == null || _playerGunEntityView == null) yield return null;
            
            while (true)
            {
                var playerGunComponent = _playerGunEntityView.gunComponent;

                playerGunComponent.timer += _time.deltaTime;
                
                if (_playerEntityView.inputComponent.fire &&
                    playerGunComponent.timer >= _playerGunEntityView.gunComponent.timeBetweenBullets)
                    Shoot(_playerGunEntityView);

                yield return null;
            }
        }

        void Shoot(GunEntityView playerGunEntityView)
        {
            var playerGunComponent    = playerGunEntityView.gunComponent;
            var playerGunHitComponent = playerGunEntityView.gunHitTargetComponent;

            playerGunComponent.timer = 0;

            Vector3 point;
            var     entityHit = _rayCaster.CheckHit(playerGunComponent.shootRay, playerGunComponent.range, ENEMY_LAYER, SHOOTABLE_MASK | ENEMY_MASK, out point);
            
            if (entityHit != -1)
            {
                PlayerTargetEntityView targetComponent;
                //note how the GameObject GetInstanceID is used to identify the entity as well
                if (entityViewsDB.TryQueryEntityView(entityHit, out targetComponent))
                {
                    var damageInfo = new DamageInfo(playerGunComponent.damagePerShot, point, entityHit, EntityDamagedType.PlayerTarget);
                    _enemyDamageSequence.Next(this, ref damageInfo);

                    playerGunComponent.lastTargetPosition = point;
                    playerGunHitComponent.targetHit.value = true;

                    return;
                }
            }

            playerGunHitComponent.targetHit.value = false;
        }

        void OnTargetDead(int targetID)
        {
            ///
            /// Pay attention to this bit. The engine is querying a
            /// PaleryTargetEntityView and not a EnemyEntityView.
            /// this is more than a sophistication, it actually the implementation
            /// of the rule that every engine must use its own set of
            /// EntityViews to promote encapsulation and modularity
            ///
            var playerTarget = entityViewsDB.QueryEntityView<PlayerTargetEntityView>(targetID);
            var targetType   = playerTarget.playerTargetComponent.targetType;

            _enemyKilledObservable.Dispatch(ref targetType);
        }

        public void Step(ref DamageInfo token, int condition)
        {
            OnTargetDead(token.entityDamagedID);
        }

        readonly EnemyKilledObservable _enemyKilledObservable;
        readonly ISequencer            _enemyDamageSequence;
        readonly IRayCaster            _rayCaster;

        PlayerEntityView _playerEntityView;
        GunEntityView    _playerGunEntityView;
        
        readonly ITime _time;
        readonly ITaskRoutine     _taskRoutine;

        static readonly int SHOOTABLE_MASK = LayerMask.GetMask("Shootable");
        static readonly int ENEMY_MASK     = LayerMask.GetMask("Enemies");
        static readonly int ENEMY_LAYER    = LayerMask.NameToLayer("Enemies");
    }
}