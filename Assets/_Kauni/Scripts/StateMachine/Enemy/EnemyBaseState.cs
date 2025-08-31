using UnityEngine;

namespace Supertactic
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine stateMachine;

        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected void Move(float deltaTime)
        {
            Move(Vector2.zero, deltaTime);
        }

        protected void FacePlayer()
        {
            if (stateMachine.Player == null) { return; }

            Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
            lookPos.y = 0f;

            stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
        }

        protected bool IsInChaseRange()
        {
            float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
            return playerDistanceSqr <= stateMachine.ChaseRange * stateMachine.ChaseRange;
        }
    }
}
