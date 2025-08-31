using UnityEngine;

namespace Supertactic
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public Targeter Targeter { get; private set; }
    }
}
