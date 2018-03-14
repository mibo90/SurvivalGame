using UnityEngine;

namespace Svelto.ECS.Example.Survive.Bonus
{

    public class BonusHealthTriggerImplementor : MonoBehaviour, IImplementor, IBonusHealthTriggerComponent
    {
        public HealthBonusCollisionData entityInRange { get; private set; }

        void OnTriggerEnter(Collider other)
        {
            entityInRange = new HealthBonusCollisionData(other.gameObject.GetInstanceID(), true);
        }

        void OnTriggerExit(Collider other)
        {
            entityInRange = new HealthBonusCollisionData(other.gameObject.GetInstanceID(), false);
        }

        bool _targetInRange;
    }
}
