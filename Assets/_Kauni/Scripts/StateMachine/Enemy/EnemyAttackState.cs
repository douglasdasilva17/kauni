using UnityEngine;

namespace Supertactic
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("Attack");
        private const float TransitionDuration = 0.1f;

        public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            // Apply damage and knockback. 
            // TODO: Implement hit react for player like Knocked Down, Kneel, Hit Reaction Right, Hit Reaction Left
            stateMachine.weaponDamage.SetAttack(stateMachine.AttackDamage, stateMachine.AttackKnockback);

            // Play attack animation (randomize the attacks)
            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
        }

        public override void Exit() { }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(stateMachine.Animator) >= 1)
            {
                stateMachine.SwitchState(new EnemyChaseState(stateMachine));
            }
        }
    }
}
