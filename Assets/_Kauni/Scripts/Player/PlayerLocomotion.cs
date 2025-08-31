using UnityEngine;
using Supertactic.Input;
using Supertactic.Cameras;

namespace Supertactic.Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] float lookDistance = 5;
        [SerializeField] float lookSpeed = 5; // How much fast the player looks at camera's direction

        [Header("Move Settings")]
        public InputReader input;
        [SerializeField] float walkVelocity;
        [SerializeField] float runVelocity;
        [SerializeField] float rotationSpeed = 300; // How much fast the player rotates itself

        Vector3 _rollDirection;
        Vector3 _moveDirection;
        Vector3 _normalVector;
        bool _isRunning;

        [Header("Gravity Settings")]
        public float surfaceCheckRadius = 0.3f;
        public Vector3 surfaceCheckOffset;
        public LayerMask surfaceLayerMask;
        public bool onSurface;

        float fallingSpeed;
        Vector3 fallingDirection;

        Transform myTransform;
        CharacterController cc;
        PlayerAnimator animatorManager;
        PlayerManager playerManager;

        public bool canRotate = true;
        public bool canMove = true;
        public float RotationSpeed => rotationSpeed;

        // Start is called before the first frame update
        void Awake()
        {
            myTransform = transform;
            cc = GetComponent<CharacterController>();
            animatorManager = GetComponent<PlayerAnimator>();
            playerManager = GetComponent<PlayerManager>();


            //SetControl(true, true);
        }

        void FixedUpdate()
        {
            if (onSurface)
            {
                fallingSpeed = 0f;
            }
            else
            {
                fallingSpeed += Physics.gravity.y * Time.deltaTime;
            }

            var velocity = fallingDirection;
            velocity.y = fallingSpeed;

            HandleLocomotion();
            SurfaceCheck();
        }

        /// <summary>
        /// Handles the player movement and rotation
        /// </summary>
        public void HandleLocomotion()
        {
            // Mapping all the inputs
            float horizontalInput = input.MoveAxis.x;
            float verticalInput = input.MoveAxis.y;
            bool runInput = input.RunInput;

            if (runInput && !_isRunning)
            {
                _isRunning = true;
            }

            if (verticalInput == 0 && horizontalInput == 0 && _isRunning)
            {
                _isRunning = false;
            }

            bool runState = _isRunning;

            // Move the player object based on input axis
            HandleMovement(horizontalInput, verticalInput, runState);

            // Rotate the player object based on input axis
            HandleRotation(horizontalInput, verticalInput);
        }

        void HandleMovement(float horizontal, float vertical, bool runState)
        {
            //if (canMove == false)
            //    return;

            // Get the camera's transform
            Transform cameraTransform = CameraManager.instance.cameraObject.transform;

            // Calculate and set the camera's forward and camera's right for the move direction
            _moveDirection = cameraTransform.forward * vertical;
            _moveDirection += cameraTransform.right * horizontal;
            _moveDirection.y = 0;
            _moveDirection.Normalize();

            // CameraManager.instance.HandleSprintZoom(player.InputReader.RunInput);

            // Calculate movement speed based on run input 
            float moveVelocity = runState ? runVelocity : walkVelocity;

            // Clamp the movement amount to check movement value
            float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            // Only moves if the move amount is greater than 0.5f
            if (movementAmount > 0.5f || movementAmount < -0.5f)
            {
                Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
                cc.Move(projectedVelocity * Time.deltaTime);
            }

            _moveDirection = fallingDirection;

            // Set the axis movement to animator blend tree 
            animatorManager.UpdateAnimatorValues(horizontal, vertical, runState);
        }

        void HandleRotation(float horizontal, float vertical)
        {
            //if (canRotate == false)
            //    return;

            if (horizontal != 0 || vertical != 0)
            {
                RotatePlayer();
            }
        }

        public void RotatePlayer()
        {
            Transform cameraPivot = CameraManager.instance.cameraPivotTransform.transform;
            Vector3 cameraPivotPos = cameraPivot.position;
            Vector3 lookPoint = cameraPivotPos + (cameraPivot.forward * lookDistance);
            Vector3 direction = lookPoint - myTransform.position;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation.x = 0;
            lookRotation.z = 0;

            Quaternion finalRotation = Quaternion.Slerp(myTransform.rotation, lookRotation, Time.deltaTime * lookSpeed);
            myTransform.rotation = finalRotation;
        }

        public void HandleDodgeInput(float horizontal, float vertical, bool dodge)
        {
            if (dodge)
            {
                AttemptToPerformDodge(horizontal, vertical);
            }
        }

        void AttemptToPerformDodge(float horizontal, float vertical)
        {
            // If player is performing a action, any other actions can be performed
            if (playerManager.isPerformingAction)
                return;

            float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            if (movementAmount > 0)
            {
                _rollDirection = CameraManager.instance.cameraObject.transform.forward * vertical;
                _rollDirection += CameraManager.instance.cameraObject.transform.right * horizontal;
                _rollDirection.y = 0;
                _rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(_rollDirection);
                myTransform.rotation = playerRotation;

                animatorManager.PlayTargetAnimation("Rolling", true, true, false, true);
            }
        }

        void SurfaceCheck()
        {
            onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, surfaceLayerMask);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius);
        }

        public void SetControl(bool canMove, bool canRotate)
        {
            this.canMove = canMove;
            this.canRotate = canRotate;
        }
    }
}