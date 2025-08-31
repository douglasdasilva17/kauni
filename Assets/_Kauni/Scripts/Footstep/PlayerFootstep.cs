using UnityEngine;

namespace Supertactic.Player
{
    public class PlayerFootstep : MonoBehaviour
    {
        [Header("Footstep Properties")]

        [SerializeField]
        AudioClip[] grass;
        [SerializeField]
        AudioClip[] concrete;
        [SerializeField]
        AudioClip[] dirt;
        [SerializeField]
        AudioClip[] gravel;
        [SerializeField]
        AudioClip[] wood;
        [SerializeField]
        AudioClip[] gore;
        [SerializeField]
        AudioClip[] water;

        [Header("Detection Properties")]
        [SerializeField] Transform rayStart;
        [SerializeField] float groundDistance = 0.3f;
        [SerializeField] float footRadius = 0.1f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] Transform leftFootPoint;
        [SerializeField] Transform rightFootPoint;

        PlayerManager player;
        CharacterController cc;
        Rigidbody rb;

        string terrainTag = "Default";
        bool leftFeetGrounded = false;
        bool rightFeetGrounded = false;

        public float footstepInterval = 0.5f; // Intervalo entre os passos em segundos
        private float nextFootstepTime; // Tempo do pr�ximo passo
        private bool canPlayFootstep = true; // Flag para permitir ou n�o o som dos passos


        void Awake()
        {
            player = GetComponent<PlayerManager>();
            cc = GetComponent<CharacterController>();
         //   rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            //Check if character is grunded
            if (!IsGrounded())
                return;

            if (player.isPerformingAction)
                return;

            //if (rb.velocity.magnitude < 0.1f)
            //    return;

            // Verifica se o som dos passos pode ser reproduzido
            if (Time.time >= nextFootstepTime)
            {
                canPlayFootstep = true;
            }

            if (IsFeetGrounded(leftFootPoint.position, footRadius) && !leftFeetGrounded)
            {
                HandleFootstepSound();
                leftFeetGrounded = true;
                rightFeetGrounded = false;
            }


            if (IsFeetGrounded(rightFootPoint.position, footRadius) && !rightFeetGrounded)
            {
                HandleFootstepSound();
                leftFeetGrounded = false;
                rightFeetGrounded = true;
            }
        }

        void CheckGroundTag()
        {
            RaycastHit hit;

            if (Physics.Raycast(rayStart.position, -rayStart.up, out hit, groundDistance, groundLayer))
            {
                terrainTag = hit.collider.tag;
            }
        }

        bool IsFeetGrounded(Vector3 position, float radius)
        {
            // Perform a raycast downwards from the given position
            return Physics.CheckSphere(position, radius, groundLayer);
        }

        bool IsGrounded()
        {
            // Perform a raycast downwards from the given position
            RaycastHit hit;
            if (Physics.Raycast(rayStart.position, -rayStart.up, out hit, groundDistance, groundLayer))
            {
                // Check if the raycast hit something tagged as ground
                if (hit.collider)
                {
                    return true;
                }
            }
            return false;
        }

        void HandleFootstepSound()
        {
            if (canPlayFootstep)
            {
                ResetFootstepCounter();

                CheckGroundTag();

                float pitchRandomized = UnityEngine.Random.Range(0.8f, 1f);
                float volumeRandomized = UnityEngine.Random.Range(0.1f, 0.3f);

                switch (terrainTag)
                {
                    case "Grass":
                        AudioManager.Instance.PlaySoundFX(grass, transform, volumeRandomized, pitchRandomized);
                        break;
                    case "Concrete":
                        AudioManager.Instance.PlaySoundFX(grass, transform, volumeRandomized, pitchRandomized);
                        break;
                    case "Dirt":
                        AudioManager.Instance.PlaySoundFX(dirt, transform, volumeRandomized, pitchRandomized);
                        break;
                    case "Gravel":
                        AudioManager.Instance.PlaySoundFX(gravel, transform, volumeRandomized, pitchRandomized);
                        break;
                    case "Wood":
                        AudioManager.Instance.PlaySoundFX(wood, transform, volumeRandomized, pitchRandomized);
                        break;
                    case "Gore":
                        AudioManager.Instance.PlaySoundFX(gore, transform, volumeRandomized, pitchRandomized);
                        break;
                    case "Water":
                        AudioManager.Instance.PlaySoundFX(water, transform, volumeRandomized, pitchRandomized);
                        break;
                    default:
                        AudioManager.Instance.PlaySoundFX(grass, transform, volumeRandomized, pitchRandomized);
                        break;
                }
            }
        }

        private void ResetFootstepCounter()
        {
            // Define o pr�ximo tempo permitido para tocar o som dos passos
            nextFootstepTime = Time.time + footstepInterval;

            // Impede que o som dos passos seja reproduzido at� que o intervalo termine
            canPlayFootstep = false;
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawRay(rayStart.position, -rayStart.up * groundDistance);
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(leftFootPoint.position, footRadius);
            Gizmos.DrawWireSphere(rightFootPoint.position, footRadius);
            Gizmos.color = Color.red;
        }
    }
}