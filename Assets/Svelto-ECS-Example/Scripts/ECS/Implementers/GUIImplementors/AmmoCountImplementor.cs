using Svelto.ECS.Example.Survive.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
    public class AmmoCountImplementor : MonoBehaviour, IImplementor, IBulletCountComponent
    {
        public int magazineCount { get { return _magazinCount; } set { _magazinCount = value; } }
        public int currentCount
        {
            get { return _currentCount; }
            set
            {
                _currentCount = value;
                _text.text = "Ammo \n" + currentCount + "/" + magazineCount;
            }
        }

        void Awake()
        {
            // Set up the reference.
            _text = GetComponent<Text>();

        }

        int _magazinCount;
        int _currentCount;
        Text _text;
    }
}