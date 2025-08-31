using UnityEngine;
using UnityEngine.AI;

namespace Supertactic
{
    public class EnemyStateMachine : StateMachine
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        [field: SerializeField] public WeaponDamage weaponDamage { get; private set; }

        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float MinimumSpeed { get; private set; }
        [field: SerializeField] public float ChaseRange { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public int AttackDamage { get; private set; }
        [field: SerializeField] public int AttackKnockback { get; private set; }

        public GameObject Player { get; private set; }

        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player");            
        }

        private void Start()
        {
            if (Agent != null)
            {
                // we dont want modify the position and rotation by the agent
                Agent.updatePosition = false;
                Agent.updateRotation = false;
            }

            SwitchState(new EnemyIdleState(this));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ChaseRange);
        }

        public void SetDeathState()
        {
            SwitchState(new EnemyDeathState(this));
        }
    }
}
