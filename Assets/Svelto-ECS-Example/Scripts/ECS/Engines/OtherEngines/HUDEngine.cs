using Svelto.Tasks.Enumerators;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Svelto.ECS.Example.Survive.Player.Gun;

namespace Svelto.ECS.Example.Survive.HUD
{
    public class HUDEngine : IQueryingEntityViewEngine, IStep<DamageInfo>, IStep<EnemyWaveData>,
                            IStep<BonusInfo>, IStep<GunInfo>, IStep<PowerInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public HUDEngine(ITime time)
        {
            _time = time;
        }

        public void Ready()
        {
            Tick().Run();
        }

        IEnumerator Tick()
        {
            while (true)
            {
                var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
                for (int i = 0; i < hudEntityViews.Count; i++)
                {
                    var damageComponent = hudEntityViews[i].damageImageComponent;

                    damageComponent.imageColor = Color.Lerp(damageComponent.imageColor, Color.clear,
                        damageComponent.speed * _time.deltaTime);
                }

                yield return null;
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
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
            {
                hudEntityViews[i].bulletCountComponent.magazineCount = gunInfo.magazineCapacity;
                hudEntityViews[i].bulletCountComponent.currentCount = gunInfo.currentBulletCount;
            }
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
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            var hudHealthEntityView = entityViewsDB.QueryEntityView<HUDHealthEntityView>(damaged.entityDamagedID);
            for (int i = 0; i < hudEntityViews.Count; i++)
            {
                var guiEntityView = hudEntityViews[i];
                var damageComponent = guiEntityView.damageImageComponent;

                damageComponent.imageColor = damageComponent.flashColor;
                hudEntityViews[i].healthSliderComponent.value = hudHealthEntityView.healthComponent.currentHealth;
            }
        }

        void OnDamageEvent(DamageInfo damaged)
        {
            UpdateSlider(damaged);
        }

        void OnDeadEvent()
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
            {
                hudEntityViews[i].healthSliderComponent.value = 0;
            }
            RestartLevelAfterFewSeconds().Run();
        }
        IEnumerator RestartLevelAfterFewSeconds()
        {
            _waitForSeconds.Reset(5);
            yield return _waitForSeconds;

            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
                hudEntityViews[i].HUDAnimator.playAnimation = "GameOver";

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

            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
                hudEntityViews[i].HUDAnimator.playAnimation = "LevelComplete";

            _waitForSeconds.Reset(4);
            yield return _waitForSeconds;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        IEnumerator NextWave()
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
                hudEntityViews[i].HUDAnimator.setBool("NextLevel", true);

            _waitForSeconds.Reset(2);
            yield return _waitForSeconds;

            for (int i = 0; i < hudEntityViews.Count; i++)
                hudEntityViews[i].HUDAnimator.setBool("NextLevel", false);
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
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
            {
                var bulletComponent = hudEntityViews[i].bulletCountComponent;
                if (bulletComponent.magazineCount > bulletComponent.currentCount + ammoBonus.amount)
                    bulletComponent.currentCount = ammoBonus.amount;
                else
                    bulletComponent.currentCount = bulletComponent.magazineCount;
            }
        }
        void OnHealthBonusEvent(BonusInfo healthBonus)
        {
            UpdateSlider(healthBonus);
        }
        void UpdateSlider(BonusInfo bonus)
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            var hudHealthEntityView =
                entityViewsDB.QueryEntityView<HUDHealthEntityView>(bonus.targetEntityID);
            for (int i = 0; i < hudEntityViews.Count; i++)
                hudEntityViews[i].healthSliderComponent.value = hudHealthEntityView.healthComponent.currentHealth;
        }

        #endregion

        #region "Power image update"
        public void Step(ref PowerInfo token, int condition)
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
                UpdatePowerUI(token, hudEntityViews[i]).Run();
        }

        IEnumerator UpdatePowerUI(PowerInfo info, HUDEntityView entityview)
        {
            entityview.powerFilledImageComponent.fillAmount = 0f;
            while (entityview.powerFilledImageComponent.fillAmount < 1f)
            {
                entityview.powerFilledImageComponent.fillAmount += _time.deltaTime / info.cooldown;
                yield return null;
            }

        }
        #endregion

        //DataStructures.FasterReadOnlyList<HUDEntityView> hudEntityViews = new DataStructures.FasterReadOnlyList<HUDEntityView>();

        readonly WaitForSecondsEnumerator _waitForSeconds = new WaitForSecondsEnumerator(5);
        readonly ITime _time;
    }
}

