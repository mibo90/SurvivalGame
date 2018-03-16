using System.Collections;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Bonus
{
    public class BonusAmmoEngine : SingleEntityViewEngine<TargetEntityView>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        { }

        public BonusAmmoEngine(ISequencer bonusAmmoSequence)
        {
            _bonusAmmoSequence = bonusAmmoSequence;
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(CheckIfBonusAmmoHittingTarget()).SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(TargetEntityView entity)
        {
            _taskRoutine.Start();
        }
        protected override void Remove(TargetEntityView obj)
        {
            _taskRoutine.Stop();
        }

        IEnumerator CheckIfBonusAmmoHittingTarget()
        {
            while (true)
            {

                var targetEntitiesView = entityViewsDB.QueryEntityViews<TargetEntityView>();

                var bonusAmmoList = entityViewsDB.QueryEntityViews<BonusAmmoEntityView>();

                for (int bonusHindex = bonusAmmoList.Count - 1; bonusHindex >= 0; --bonusHindex)
                {
                    var bonusAmmoEntityView = bonusAmmoList[bonusHindex];
                    var bonusHCollisionData = bonusAmmoEntityView.targetTriggerComponent.entityInRange;

                    for (int bonusTargetIndex = 0; bonusTargetIndex < targetEntitiesView.Count; bonusTargetIndex++)
                    {
                        var targetEntityView = targetEntitiesView[bonusTargetIndex];

                        if (bonusHCollisionData.collides == true &&
                            bonusHCollisionData.otherEntityID == targetEntityView.ID)
                        {


                            var bonusAmmoInfo = new BonusInfo(targetEntityView.ID, bonusAmmoEntityView.ID,
                                bonusAmmoEntityView.bonusAmountComponent.amount,BonusType.ammo);
                            _bonusAmmoSequence.Next(this, ref bonusAmmoInfo);
                        }
                    }
                }

                yield return null;
            }
        }

        ISequencer _bonusAmmoSequence;
        ITaskRoutine _taskRoutine;
    }
}