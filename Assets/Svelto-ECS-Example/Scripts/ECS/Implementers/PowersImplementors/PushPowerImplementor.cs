using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player.Power
{
    class PushPowerImplementor : MonoBehaviour, IImplementor, IScaleComponent, IPowerComponent
    {
        public DispatchOnChange<bool> destroyed { get; private set; }

        public Vector3 scale { get { return transform.localScale; } set { transform.localScale = value; } }

        public int duration { get { return _duration; }}

        public int cooldown { get { return _cooldown; } }

        public int speed { get { return _speed; } }

        public float timer { get; set; }

        //void Awake()
        //{
        //    destroyed = new DispatchOnChange<bool>(GetInstanceID());
        //    destroyed.NotifyOnValueSet(OnDestroyed);
        //}

        //void OnDestroyed(int sender, bool isDestroyed)
        //{
        //    Destroy(transform.parent.gameObject);
        //}

        [Tooltip("Duration of the power effect")]
        [SerializeField]
        int _duration;
        [Tooltip("Speed in scalePoints per second")]
        [SerializeField]
        int _speed;
        [Tooltip("Duration of the cooldown")]
        [SerializeField]
        int _cooldown;
        
    }
}
