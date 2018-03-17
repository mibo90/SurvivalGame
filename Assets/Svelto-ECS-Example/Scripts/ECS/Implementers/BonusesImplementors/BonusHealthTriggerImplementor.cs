using UnityEngine;

namespace Svelto.ECS.Example.Survive.Bonus
{

    public class BonusHealthTriggerImplementor : MonoBehaviour, IImplementor, IBonusTriggerComponent
    {
        public BonusCollisionData entityInRange { get; private set; }

        void OnTriggerEnter(Collider other)
        {
            entityInRange = new BonusCollisionData(other.gameObject.GetInstanceID(), true);
        }

        void OnTriggerExit(Collider other)
        {
            entityInRange = new BonusCollisionData(other.gameObject.GetInstanceID(), false);
        }

        bool _targetInRange;
    }
}
