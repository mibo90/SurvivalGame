using Svelto.ECS.Example.Survive.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
    public class EnemyCountImplementor : MonoBehaviour, IImplementor, IEnemyCountComponent
    {
        public int enemyCount { get { return _enemyCount; } set { _enemyCount = value; _text.text = "Enemies: " + _enemyCount; } }

        void Awake()
        {
            // Set up the reference.
            _text = GetComponent<Text>();

        }

        int _enemyCount;
        Text _text;
    }
}
