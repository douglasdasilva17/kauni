using UnityEngine;
using UnityEngine.Events;

namespace Supertactic
{
    public class AnimationEventListener : MonoBehaviour
    {
        public UnityEvent attackStartEvent;
        public UnityEvent attackEndEvent;
        public UnityEvent hitReactionEvent;

        private void HitReactionEvent()
        {
            hitReactionEvent.Invoke();
        }

        public void AttackStartEvent()
        {
            attackStartEvent.Invoke();
        }

        public void AttackEndEvent()
        {
            attackEndEvent.Invoke();
        }
    }
}
