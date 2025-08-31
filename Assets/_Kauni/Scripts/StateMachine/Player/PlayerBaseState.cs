using UnityEngine;

namespace Supertactic
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine stateMachine;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected void Move(float deltaTime)
        {
            Move(Vector2.zero, deltaTime);
        }

        //protected void FacePlayer()
        //{
        //    if (stateMachine.Player == null) { return; }

        //    Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        //    lookPos.y = 0f;

        //    stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
        //}

        protected void Move(Vector3 motion, float deltaTime)
        {
            stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
        }

        protected void ReturnLocomotion()
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
    }
}
