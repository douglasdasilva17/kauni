using UnityEngine;

namespace Supertactic
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State _currentState;

        public void SwitchState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public void Update()
        {
            _currentState?.Tick(Time.deltaTime);
        }
    }
}
