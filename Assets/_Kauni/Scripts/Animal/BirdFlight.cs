using UnityEngine;
using static FEasing;

namespace Supertactic.Animal
{
    public class BirdFlightController : MonoBehaviour
    {
        [SerializeField] float baseSpeed = 4f;

        [SerializeField] float flySpeed = 10f;
        [SerializeField] float glideSpeed = 5f;
        [SerializeField] float minGlideSpeed = 2f;
        [SerializeField] float tiltAmount = 30f;
        [SerializeField] float pitchAmount = 30f;
        [SerializeField] float maxTiltAngle = 45f;
        [SerializeField] float idleTurnSpeed = 2f;
        [SerializeField] float slowDownFactor = 0.5f;
        [SerializeField] float maxIdlePitchAngle = 15f;

        [SerializeField] private float tiltSpeed = 5f;
        [SerializeField] private float pitchSpeed = 5f;
        [SerializeField] private float brakeSpeed;

        private Animator _animator;
        private Rigidbody _body;
        private bool isFlying = false;
        private bool isGliding = false;
        private float targetTilt = 0f;
        private float targetPitch = 0f;
        private float currentTilt;
        private float currentPitch;

        [Header("Wing Flap")]
        [SerializeField] private float minFlapTime = 1f;
        [SerializeField] private float maxFlapTime = 5f;

        [Header("Audio")]
        [SerializeField] private AudioSource source;

        private float _flapTimer = 0;
        private float _randomFlapTime;

        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _body = GetComponent<Rigidbody>();
            _randomFlapTime = UnityEngine.Random.Range(minFlapTime, maxFlapTime);
        }

        void FixedUpdate()
        {
            HandleInput();
            UpdateAnimation();
        }

        void HandleInput()
        {
            bool spacePressed = UnityEngine.Input.GetKey(KeyCode.Space);
            if (spacePressed)
            {
                if (!isFlying)
                {
                    StartFlapWings();
                }
            }
            else if (isFlying)
            {
                StartGlide();
            }
            else
            {
                StopFlying();
            }

            if (UnityEngine.Input.GetKey(KeyCode.P))
            {
                GetComponent<FlockChildSound>().PlayRandomSound();
            }

            if (isFlying) FlyLogic();
            else if (isGliding) GlideLogic();
            else StationaryLogic();
        }

        void UpdateAnimation()
        {
            if (isFlying)
            {
                if (_body.velocity.magnitude >= minGlideSpeed)
                {
                    if (!isGliding) StartGlide();
                }
                else if (isGliding)
                {
                    StartFlapWings();
                }
            }
        }

        void StationaryLogic()
        {
            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            float vertical = UnityEngine.Input.GetAxis("Vertical");

            transform.Rotate(0, horizontal * idleTurnSpeed * Time.deltaTime, 0);

            float targetIdlePitch = Mathf.Clamp(vertical * pitchAmount, -maxIdlePitchAngle, maxIdlePitchAngle);
            currentPitch = Mathf.LerpAngle(transform.eulerAngles.x, targetIdlePitch, Time.deltaTime * pitchSpeed);
            transform.rotation = Quaternion.Euler(currentPitch, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        void FlyLogic()
        {
            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            float vertical = UnityEngine.Input.GetAxis("Vertical");

            targetTilt = horizontal * tiltAmount;
            targetPitch = vertical * pitchAmount;

            currentTilt = Mathf.LerpAngle(transform.eulerAngles.z, targetTilt, Time.deltaTime * tiltSpeed);
            currentPitch = Mathf.LerpAngle(transform.eulerAngles.x, targetPitch, Time.deltaTime * pitchSpeed);

            transform.rotation = Quaternion.Euler(currentPitch, transform.eulerAngles.y, currentTilt);

            float adjustedFlySpeed = flySpeed * (1 - Mathf.Abs(vertical) * slowDownFactor);
            Vector3 forward = transform.forward * adjustedFlySpeed * Time.deltaTime;
            _body.MovePosition(_body.position + forward);
            _body.velocity = transform.forward * adjustedFlySpeed;

            if (Mathf.Abs(currentTilt) >= maxTiltAngle || Mathf.Abs(targetPitch) >= maxTiltAngle)
            {
                StartGlide();
            }
        }

        void GlideLogic()
        {
            Vector3 forward = transform.forward * glideSpeed * Time.deltaTime;
            _body.MovePosition(_body.position + forward);

            Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        }

        void StartGlide()
        {
            isGliding = true;
            isFlying = false;
            _animator.SetBool("isFlying", isFlying);
            _animator.SetBool("isGliding", isGliding);
        }

        void StartFlapWings()
        {
            isFlying = true;
            isGliding = false;
            _animator.SetBool("isFlying", isFlying);
            _animator.SetBool("isGliding", isGliding);
            RandomFlapWing();
        }

        void RandomFlapWing()
        {
            _flapTimer += Time.deltaTime;
            if (_flapTimer > _randomFlapTime)
            {
                _flapTimer = 0;
                _randomFlapTime = UnityEngine.Random.Range(minFlapTime, maxFlapTime);
                _animator.CrossFade("Flap", 0.1f);
            }
        }

        void StopFlying()
        {
            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            float vertical = UnityEngine.Input.GetAxis("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                transform.Rotate(0, horizontal * idleTurnSpeed * Time.deltaTime, 0);

                float adjustedFlySpeed = baseSpeed * (1 - Mathf.Abs(vertical) + (1 - Mathf.Abs(vertical)));
                Vector3 forward = transform.forward * baseSpeed * Time.deltaTime;
                _body.MovePosition(_body.position + forward);
                _body.velocity = transform.forward * baseSpeed;
            }
            else
            {
                isFlying = false;
                isGliding = false;
                _animator.SetBool("isFlying", isFlying);
                _animator.SetBool("isGliding", isGliding);
                _body.velocity = Vector3.Lerp(_body.velocity, Vector3.zero, brakeSpeed * Time.deltaTime);
                _body.angularVelocity = Vector3.Lerp(_body.angularVelocity, Vector3.zero, brakeSpeed * Time.deltaTime);
            }
        }

        public void Play(AudioClip clip)
        {
            source.pitch = Random.Range(0.5f, 1.5f);
            source.PlayOneShot(clip, Random.Range(0.6f, 0.9f));
        }
    }
}
