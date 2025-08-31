using UnityEngine;

namespace Supertactic
{
    public class EnemyChaseState : EnemyBaseState
    {
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
        private readonly int SpeedHash = Animator.StringToHash("Speed");
        private const float CrossfadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        private const float AnimatorSpeed = 1f;

        // Vari�veis necess�rias
        private float _targetSpeed;
        private float _lerpDuration = 1.0f; // Dura��o do lerp
        private float _interpolatedSpeed;

        public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _targetSpeed = UnityEngine.Random.Range(stateMachine.MinimumSpeed, stateMachine.MovementSpeed);

            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossfadeDuration);
        }

        public override void Exit()
        {
            if (stateMachine == null)
            {
                return;
            }
            
            if (stateMachine.Agent != null)
            {
                stateMachine.Agent.ResetPath();
                stateMachine.Agent.velocity = Vector3.zero;
            }
        }

        public override void Tick(float deltaTime)
        {
            if (!IsInChaseRange())
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }
            else if (IsInAttackRange())
            {
                stateMachine.SwitchState(new EnemyAttackState(stateMachine));
                return;
            }

            MoveToPlayer(deltaTime);

            FacePlayer();

            InterpolateAnimationSpeed(deltaTime);
        }

        private void InterpolateAnimationSpeed(float deltaTime)
        {
            // Interpolar o movementSpeed
            _interpolatedSpeed = Mathf.Lerp(stateMachine.Animator.GetFloat(SpeedHash), _targetSpeed, deltaTime / _lerpDuration);

            // Verificar se a interpola��o est� completa
            if (Mathf.Abs(_interpolatedSpeed - _targetSpeed) < 0.01f)
            {
                // Escolher um novo valor aleat�rio para targetSpeed
                _targetSpeed = Random.Range(stateMachine.MinimumSpeed, stateMachine.MovementSpeed);
            }

            stateMachine.Animator.SetFloat(SpeedHash, _interpolatedSpeed, AnimatorDampTime, deltaTime);
        }

        private void MoveToPlayer(float deltaTime)
        {
            if (stateMachine.Agent != null)
            {
                // Atualizar o destino do agente para a posi��o do jogador
                stateMachine.Agent.destination = stateMachine.Player.transform.position;

                // Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

                //stateMachine.Agent.velocity = stateMachine.Controller.velocity;
                stateMachine.Agent.velocity = stateMachine.Animator.velocity;
            }
        }

        private bool IsInAttackRange()
        {
            float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
            return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
        }
    }
}
