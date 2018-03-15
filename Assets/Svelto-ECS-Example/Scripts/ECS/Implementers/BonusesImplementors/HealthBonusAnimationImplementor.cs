using UnityEngine;

namespace Svelto.ECS.Example.Survive.Bonus
{
    public class HealthBonusAnimationImplementor : MonoBehaviour, IImplementor,
        IAnimationComponent
    {
        public void setBool(string name, bool value)
        {
            _anim.SetBool(name, value);
        }

        public string trigger { set { _anim.SetTrigger(value); } }

        void Awake()
        {
            // Setting up the references.
            _anim = GetComponent<Animator>();
        }

        Animator _anim;                 // Reference to the animator.
    }
}
