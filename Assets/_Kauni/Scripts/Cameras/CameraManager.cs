using HairFX;
using Supertactic.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Supertactic.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance;

        [Header("Camera Speed")]
        public float moveSpeed = 5;
        public float zoomSpeed = 5;
        public float rotationSpeed = 3.5f;
        Vector3 cameraFollowVelocity = Vector3.zero;

        [Header("Camera FOV")]
        public float originalFOV = 70;
        public float zoomFOV = 20;

        [Header("Camera Gamepad Sensitivity")]
        public float cameraXGamepadSensitivity = 1.2f;
        public float cameraYGamepadSensitivity = 1.2f;

        [Header("Camera Mouse Sensitivity")]
        public float cameraXMouseSensitivity = 0.025f;
        public float cameraYMouseSensitivity = 0.025f;

        float currentXSensitivity;
        float currentYSensitivity;

        [Header("Camera Sensitivity")]
        public float aimXSensitivity = 1f;
        public float aimYSensitivity = 1f;

        [Header("Camera Clamp")]
        [SerializeField]
        float minLimitAngle = -30;
        [SerializeField]
        float maxLimitAngle = 30;

        [Header("Camera Collision")]
        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minCollisionOffset = 0.2f;

        public LayerMask ignoreLayers;

        float targetPositionZ;
        float defaultPositionZ;
        Vector3 cameraTransformPosition;

        [Header("Camera Transform")]
        public Camera cameraObject;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        public Transform cameraTarget;

        Transform myTransform;

        float cameraXRotation = 0;
        float cameraYRotation = 0;

        [SerializeField] InputReader inputReader;

        void Awake()
        {
            instance = this;

            currentXSensitivity = cameraXMouseSensitivity;
            currentYSensitivity = cameraYMouseSensitivity;
        }

        void Start()
        {
            defaultPositionZ = cameraTransform.position.z;
            myTransform = transform;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (cameraTarget == null)
                cameraTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void FixedUpdate()
        {
            HandleRotationCamera();
        }

        void LateUpdate()
        {
            HandleFollowTarget();
        }

        void HandleFollowTarget()
        {
            Vector3 moveVector = Vector3.SmoothDamp(myTransform.position, cameraTarget.position, ref cameraFollowVelocity, moveSpeed * Time.deltaTime);
            myTransform.position = moveVector;
            //  HandleCameraCollision();
        }

        public void SetSensitivity(string controlScheme)
        {
            switch (controlScheme)
            {
                case "Keyboard&Mouse":
                    currentXSensitivity = cameraXMouseSensitivity;
                    currentYSensitivity = cameraYMouseSensitivity;
                    break;

                case "Gamepad":
                    currentXSensitivity = cameraXGamepadSensitivity;
                    currentYSensitivity = cameraYGamepadSensitivity;
                    break;
            }
        }

        void HandleRotationCamera()
        {
            float xSensitivity = inputReader.AimInput ? aimXSensitivity : currentXSensitivity;
            float ySensitivity = inputReader.AimInput ? aimYSensitivity : currentYSensitivity;
            cameraXRotation -= inputReader.LookAxis.y * ySensitivity;
            cameraYRotation += inputReader.LookAxis.x * xSensitivity;

            cameraXRotation = Mathf.Clamp(cameraXRotation, minLimitAngle, maxLimitAngle);
            cameraYRotation = Mathf.Repeat(cameraYRotation, 360);

            Vector3 rotationAngle = new Vector3(cameraXRotation, cameraYRotation, 0);
            Quaternion targetRotation = Quaternion.Slerp(
                cameraPivotTransform.transform.localRotation,
                Quaternion.Euler(rotationAngle), rotationSpeed * Time.deltaTime);

            cameraPivotTransform.transform.localRotation = targetRotation;
        }

        public void HandleSprintZoom(bool isSprinting)
        {
            if (isSprinting)
                cameraObject.fieldOfView = Mathf.Lerp(cameraObject.fieldOfView, 100, zoomSpeed * Time.deltaTime);
            else
                cameraObject.fieldOfView = Mathf.Lerp(cameraObject.fieldOfView, originalFOV, zoomSpeed * Time.deltaTime);
        }

        public void HandleAimZoom(bool isAiming)
        {
            if (isAiming)
                ZoonIn();
            else
                ZoomOut();
        }

        public void ZoomOut()
        {
            cameraObject.fieldOfView = Mathf.Lerp(cameraObject.fieldOfView, originalFOV, zoomSpeed * Time.deltaTime);
        }

        public void ZoonIn()
        {
            cameraObject.fieldOfView = Mathf.Lerp(cameraObject.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
        }

        void HandleCameraCollision()
        {
            targetPositionZ = defaultPositionZ;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPositionZ), ignoreLayers))
            {
                float dist = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPositionZ = -(dist - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPositionZ) < minCollisionOffset)
            {
                targetPositionZ = -minCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPositionZ, Time.deltaTime * 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public Vector3 ScreenToWorldPoint(Vector3 point)
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            return cameraObject.WorldToScreenPoint(new Vector3(screenCenter.x, screenCenter.y, cameraObject.nearClipPlane));
        }
    }
}