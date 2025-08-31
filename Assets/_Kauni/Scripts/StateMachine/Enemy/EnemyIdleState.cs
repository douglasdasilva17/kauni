using UnityEngine;

namespace Supertactic
{
    public class EnemyIdleState : EnemyBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedHash = Animator.StringToHash("Speed");
        private const float CrossfadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossfadeDuration);
        }

        public override void Exit() { }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if (IsInChaseRange())
            {
                stateMachine.SwitchState(new EnemyChaseState(stateMachine));
                return;
            }

            FacePlayer();

            stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
        }
    }
}
