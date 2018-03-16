using Svelto.ECS.Example.Survive.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
    public class PushPowerTimerImplementor : MonoBehaviour, IPowerFilledImageComponent, IImplementor
    {
        public Image image { get; private set; }

        void Awake()
        {
            image = GetComponent<Image>();
        }

        public float fillAmount
        {
            get { return image.fillAmount; }
            set { image.fillAmount = value; }
        }
    }
}
