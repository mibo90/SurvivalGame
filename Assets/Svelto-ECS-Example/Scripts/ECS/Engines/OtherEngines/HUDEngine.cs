using Svelto.Tasks.Enumerators;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Svelto.ECS.Example.Survive.Player.Gun;

namespace Svelto.ECS.Example.Survive.HUD
{
    public class HUDEngine : SingleEntityViewEngine<HUDEntityView>, IQueryingEntityViewEngine, IStep<DamageInfo>, IStep<EnemyWaveData>,
                            IStep<BonusInfo>, IStep<GunInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public HUDEngine(ITime time)
        {
            _time = time;
        }

        public void Ready()
        {}

        protected override void Add(HUDEntityView EntityView)
        {
            _guiEntityView = EntityView;
            Tick().Run();
        }

        protected override void Remove(HUDEntityView EntityView)
        {
            _guiEntityView = null;
        }

        IEnumerator Tick()
        {
            while (true)
            {
                var damageComponent = _guiEntityView.damageImageComponent;

                damageComponent.imageColor = Color.Lerp(damageComponent.imageColor, Color.clear, damageComponent.speed * _time.deltaTime);

                yield return null;
                
                if (_guiEntityView == null) yield break;
            }
        }
        #region "Ammo Update"
        public void Step(ref GunInfo token, int condition)
        {
            OnGunUsed(token); 
        }
        void OnGunUsed(GunInfo gunInfo)
        {
            UpdateAmmo(gunInfo);
        }
        void UpdateAmmo(GunInfo gunInfo)
        {
            _guiEntityView.bulletCountComponent.magazineCount = gunInfo.magazineCapacity;
            _guiEntityView.bulletCountComponent.currentCount = gunInfo.currentBulletCount;
        }
        #endregion

        #region "Damage update"
        public void Step(ref DamageInfo token, int condition)
        {
            if (condition == DamageCondition.Damage)
                OnDamageEvent(token);
            else
            if (condition == DamageCondition.Dead)
                OnDeadEvent();
        }
        void UpdateSlider(DamageInfo damaged)
        {
            var damageComponent = _guiEntityView.damageImageComponent;

            damageComponent.imageColor = damageComponent.flashColor;

            var hudHealthEntityView =
                entityViewsDB.QueryEntityView<HUDHealthEntityView>(damaged.entityDamagedID);
         
            _guiEntityView.healthSliderComponent.value = hudHealthEntityView.healthComponent.currentHealth;
        }

        void OnDamageEvent(DamageInfo damaged)
        {
            UpdateSlider(damaged);
        }

        void OnDeadEvent()
        {
            _guiEntityView.healthSliderComponent.value = 0;

            RestartLevelAfterFewSeconds().Run();
        }
        IEnumerator RestartLevelAfterFewSeconds()
        {
            _waitForSeconds.Reset(5);
            yield return _waitForSeconds;

            _guiEntityView.HUDAnimator.trigger = "GameOver";

            _waitForSeconds.Reset(2);
            yield return _waitForSeconds;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        #endregion

        #region "Wave update"
        public void Step(ref EnemyWaveData token, int condition)
        {
            if (condition == WaveCondition.Next || condition == WaveCondition.Last)
            {
                OnWaveCompleteEvent();
            }
            else if (condition == WaveCondition.Stop)
                OnLevelCompleteEvent();
        }

        IEnumerator NextLevel()
        {
            _waitForSeconds.Reset(2);
            yield return _waitForSeconds;

            _guiEntityView.HUDAnimator.trigger = "LevelComplete";

            _waitForSeconds.Reset(4);
            yield return _waitForSeconds;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        IEnumerator NextWave()
        {
            
            _guiEntityView.HUDAnimator.setBool("NextLevel", true);

            _waitForSeconds.Reset(2);
            yield return _waitForSeconds;

            _guiEntityView.HUDAnimator.setBool("NextLevel", false);
        }

        void OnLevelCompleteEvent()
        {
            NextLevel().Run();
        }
        void OnWaveCompleteEvent()
        {
            NextWave().Run();
        }
        #endregion

        #region "Bonus update"
        public void Step(ref BonusInfo token, int condition)
        {
            if (token.bonusType == BonusType.health)
                OnHealthBonusEvent(token);
            else
                OnAmmoBonusEvent(token);
        }
        void OnAmmoBonusEvent(BonusInfo ammoBonus)
        {
            UpdateAmmo(ammoBonus);
        }
        void UpdateAmmo(BonusInfo ammoBonus)
        {
            var bulletComponent = _guiEntityView.bulletCountComponent;
            if (bulletComponent.magazineCount > bulletComponent.currentCount + ammoBonus.amount)
                bulletComponent.currentCount = ammoBonus.amount;
            else
                bulletComponent.currentCount = bulletComponent.magazineCount;
        }
        void OnHealthBonusEvent(BonusInfo healthBonus)
        {
            UpdateSlider(healthBonus);
        }
        void UpdateSlider(BonusInfo bonus)
        {
            var hudHealthEntityView =
                entityViewsDB.QueryEntityView<HUDHealthEntityView>(bonus.targetEntityID);
            _guiEntityView.healthSliderComponent.value = hudHealthEntityView.healthComponent.currentHealth;
        }
        #endregion

        HUDEntityView _guiEntityView;
        readonly WaitForSecondsEnumerator  _waitForSeconds = new WaitForSecondsEnumerator(5);
        readonly ITime                     _time;
    }
}

