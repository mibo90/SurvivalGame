using UnityEngine;

namespace Svelto.ECS.Example.Survive.Bonus
{

    public class BonusAmmoImplementor : MonoBehaviour, IImplementor, IDestroyComponent, IPositionComponent, IBonusAmountComponent
    {
        public DispatchOnChange<bool> destroyed { get; private set; }

        public Vector3 position { get; private set; }

        public int amount { get; private set; }

        public int ammoBonus = 0;

        void Awake()
        {
            position = transform.position;
            amount = ammoBonus;
            destroyed = new DispatchOnChange<bool>(GetInstanceID());
            destroyed.NotifyOnValueSet(OnDestroyed);
        }

        void OnDestroyed(int sender, bool isDestroyed)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
