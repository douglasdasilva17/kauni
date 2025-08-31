using UnityEngine;
using UnityEngine.AI;

namespace Supertactic
{
    public class ForceReceiver : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float drag = 0.3f;

        private Vector3 dampVelocity;
        private Vector3 impact;
        private float verticalVelocity;

        public Vector3 Movement => impact + Vector3.up * verticalVelocity;

        private void Update()
        {
            if (verticalVelocity < 0f && controller.isGrounded)
            {
                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampVelocity, drag);

            if (agent != null)
            {
                if (impact == Vector3.zero)
                {
                    agent.enabled = true;
                }
            }
        }

        public void AddForce(Vector3 force)
        {
            impact += force;

            if (agent != null)
            {
                agent.enabled = false;
            }
        }
    }
}
