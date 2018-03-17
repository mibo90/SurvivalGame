using System.Collections;
using Svelto.Tasks;
using Svelto.ECS.Example.Survive.Player;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player.Power
{
    public class PushPowerEngine : MultiEntityViewsEngine<PowerEntityView,PlayerEntityView>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        { }

        public PushPowerEngine(ISequencer pushPowerSequence, ITime time)
        {
            _pushPowerSequence = pushPowerSequence;
            _time = time;
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(Tick()).SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(PlayerEntityView entityView)
        {
            _playerEntityView = entityView;
        }

        protected override void Remove(PlayerEntityView entityView)
        {
            _playerEntityView = null;
        }

        protected override void Add(PowerEntityView entity)
        {
            _powerEntityView = entity;
            _taskRoutine.Start();
        }
        protected override void Remove(PowerEntityView obj)
        {
            _powerEntityView = null;
            _taskRoutine.Stop();
        }

        IEnumerator Tick()
        {
            while (_powerEntityView == null) yield return null;
            var powerComponent = _powerEntityView.powerComponent;
            powerComponent.timer = powerComponent.cooldown;
            while (true)
            {
                powerComponent.timer += _time.deltaTime;
                // wait for the button to be pressed and for the cooldown to be over before the power can be used
                if (_playerEntityView.inputComponent.push &&
                    powerComponent.timer >= powerComponent.cooldown )
                    Push(_powerEntityView).Run();

                yield return null;
            }
        }

        IEnumerator Push(PowerEntityView powerEntityView)
        {
            var powerComponent = powerEntityView.powerComponent;
            var scaleComponent = powerEntityView.scaleComponent;
            powerComponent.timer = 0;
            var powerInfo = new PowerInfo(_powerEntityView.ID, AudioType.activate, powerComponent.cooldown);
            _pushPowerSequence.Next(this, ref powerInfo,PowerCondition.Start);
            float powerTimer = 0;
            while (powerTimer<powerComponent.duration)
            {
                powerTimer += _time.deltaTime;
                float currentScaleSpeed = powerComponent.speed * _time.deltaTime;
                Vector3 newScale = scaleComponent.scale + new Vector3(currentScaleSpeed, currentScaleSpeed, currentScaleSpeed);
                scaleComponent.scale = newScale;
                yield return null;
            }
            scaleComponent.scale = new Vector3(0, 0, 0);
            powerInfo.audioType = AudioType.deactivate;
            _pushPowerSequence.Next(this, ref powerInfo, PowerCondition.Stop);
        }

        ISequencer _pushPowerSequence;
        ITaskRoutine _taskRoutine;
        PowerEntityView _powerEntityView;
        PlayerEntityView _playerEntityView;

        readonly ITime _time;
    }
}