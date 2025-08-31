using UnityEngine;

namespace Supertactic
{
    public class EnemyImpactState : EnemyBaseState
    {
        private readonly int ImpactHash = Animator.StringToHash("Impact");
        private const float CrossfadeDuration = 0.1f;

        private float _duration = 1f;

        public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossfadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            _duration -= deltaTime;

            if (_duration <= 0f)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            }
        }

        public override void Exit() { }
    }
}
