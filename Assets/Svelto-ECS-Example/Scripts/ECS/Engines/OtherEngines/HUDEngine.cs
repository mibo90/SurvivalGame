using Svelto.Tasks.Enumerators;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Svelto.ECS.Example.Survive.HUD
{
    public class HUDEngine : SingleEntityViewEngine<HUDEntityView>, IQueryingEntityViewEngine, IStep<DamageInfo>, IStep<EnemyWaveData>,
                            IStep<healthBonusInfo>
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

        void OnDamageEvent(DamageInfo damaged)
        {
            UpdateSlider(damaged);
        }

        void OnHealthBonusEvent(healthBonusInfo healthBonus)
        {
            UpdateSlider(healthBonus);
        }

        void OnDeadEvent()
        {
            _guiEntityView.healthSliderComponent.value = 0;

            RestartLevelAfterFewSeconds().Run();
        }
        void OnLevelCompleteEvent()
        {
            NextLevel().Run();
        }
        void OnWaveCompleteEvent()
        {
            NextWave().Run();
        }

        void UpdateSlider(DamageInfo damaged)
        {
            var damageComponent = _guiEntityView.damageImageComponent;

            damageComponent.imageColor = damageComponent.flashColor;

            var hudHealthEntityView =
                entityViewsDB.QueryEntityView<HUDHealthEntityView>(damaged.entityDamagedID);
         
            _guiEntityView.healthSliderComponent.value = hudHealthEntityView.healthComponent.currentHealth;
        }
        void UpdateSlider(healthBonusInfo bonus)
        {
            var hudHealthEntityView =
                entityViewsDB.QueryEntityView<HUDHealthEntityView>(bonus.targetEntityID);
            _guiEntityView.healthSliderComponent.value = hudHealthEntityView.healthComponent.currentHealth;
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

        public void Step(ref DamageInfo token, int condition)
        {
            if (condition == DamageCondition.Damage)
                OnDamageEvent(token);
            else
            if (condition == DamageCondition.Dead)
                OnDeadEvent();
        }
        public void Step(ref EnemyWaveData token, int condition)
        {
            if (condition == WaveCondition.Next || condition == WaveCondition.Last)
            {
                OnWaveCompleteEvent();
            }
            else if (condition == WaveCondition.Stop)
                OnLevelCompleteEvent();
        }

        public void Step(ref healthBonusInfo healthBonus, int condition)
        {
            OnHealthBonusEvent(healthBonus);
        }

        HUDEntityView                      _guiEntityView;
        readonly WaitForSecondsEnumerator  _waitForSeconds = new WaitForSecondsEnumerator(5);
        readonly ITime                     _time;
    }
}

