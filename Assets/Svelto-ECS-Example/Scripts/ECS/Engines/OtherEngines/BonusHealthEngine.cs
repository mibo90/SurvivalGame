using System.Collections;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Bonus
{
    public class BonusHealthEngine :SingleEntityViewEngine<TargetEntityView>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        { }

        public BonusHealthEngine(ISequencer bonusHealthSequence)
        {
            _targetDamageSequence = bonusHealthSequence;
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(CheckIfBonusHittingTarget()).SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(TargetEntityView entity)
        {
            _taskRoutine.Start();
        }

        protected override void Remove(TargetEntityView obj)
        {
            _taskRoutine.Stop();
        }

        IEnumerator CheckIfBonusHittingTarget()
        {
            while (true)
            {
               
                var targetEntitiesView = entityViewsDB.QueryEntityViews<TargetEntityView>();
               
                var bonusHealthList = entityViewsDB.QueryEntityViews<BonusHealthEntityView>();

                for (int bonusHindex = bonusHealthList.Count - 1; bonusHindex >= 0; --bonusHindex)
                {
                    var bonusHealthEntityView = bonusHealthList[bonusHindex];
                    var bonusHCollisionData = bonusHealthEntityView.targetTriggerComponent.entityInRange;

                    for (int bonusTargetIndex = 0; bonusTargetIndex < targetEntitiesView.Count; bonusTargetIndex++)
                    {
                        var targetEntityView = targetEntitiesView[bonusTargetIndex];

                        if (bonusHCollisionData.collides == true &&
                            bonusHCollisionData.otherEntityID == targetEntityView.ID)
                        {
                            bonusHealthEntityView.destroyComponent.destroyed.value = true;
                            
                            var healthBonusInfo = new healthBonusInfo(targetEntityView.ID, bonusHealthEntityView.ID,
                                bonusHealthEntityView.bonusAmountComponent.amount);
                                _targetDamageSequence.Next(this, ref healthBonusInfo);
                        }
                    }
                }

                yield return null;
            }
        }

        ISequencer _targetDamageSequence;
        ITime _time;
        ITaskRoutine _taskRoutine;
    }
}