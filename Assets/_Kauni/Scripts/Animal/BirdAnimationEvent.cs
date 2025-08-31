using UnityEngine;

namespace Supertactic.Animal
{
    public class BirdAnimationEvent : MonoBehaviour
    {
        [SerializeField] BirdFlightController controller;
        [SerializeField] AudioClip wingFlapClip;

        void Flap()
        {
            controller.Play(wingFlapClip);
        }
    }
}
