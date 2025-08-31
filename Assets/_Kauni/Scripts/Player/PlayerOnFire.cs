using Supertactic.Mukani;
using System.Security.Cryptography;
using UnityEngine;

namespace Supertactic
{
    public class PlayerOnFire : MonoBehaviour
    {
        private readonly int OnFireStateHash = Animator.StringToHash("On Fire");

        [Header("Sounds")]
        public AudioClip[] screamClips;

        [Header("VFX")]
        [SerializeField] private ParticleSystem bodyFireEffect;

        private Animator _animator; // Refer�ncia ao componente Animator
        public float fireDuration = 1.0f; // Dura��o do fogo em segundos

        private bool isOnFire = false;
        private float fireTimer = 0.0f;

        private Damageable _damageable;
        private DamageVisualHandler _damageVisualHandler;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _damageVisualHandler = FindObjectOfType<DamageVisualHandler>();
        }

        private void OnEnable()
        {
            _damageable = GetComponent<Damageable>();
            _damageable.OnDamageComplete += HandleDamageComplete;
        }

        private void OnDisable()
        {
            _damageable.OnDamageComplete -= HandleDamageComplete;
        }

        private void HandleDamageComplete(EDamageType damageType)
        {
            if (damageType == EDamageType.Fire)
            {
                StartFire();
            }
        }

        void Update()
        {
            if (isOnFire)
            {
                fireTimer += Time.deltaTime;
                if (fireTimer >= fireDuration)
                {
                    StopFire();
                }
            }
        }

        public void StartFire()
        {
            if (!isOnFire)
            {
                isOnFire = true;
                fireTimer = 0.0f;
                bodyFireEffect.Play();
                _damageVisualHandler.Execute();
                _animator.CrossFadeInFixedTime(OnFireStateHash, 0.1f);
                //animator.SetBool("IsOnFire", true);
                AudioClip selectedClip = screamClips[UnityEngine.Random.Range(0, screamClips.Length)];
                AudioManager.Instance.PlaySoundFX(selectedClip, transform, UnityEngine.Random.Range(0.2f, 0.4f), 1f);
            }
        }

        public void StopFire()
        {
            if (isOnFire)
            {
                isOnFire = false;
                //bodyFireEffect.SetActive(false);
                //animator.SetBool("IsOnFire", false);
            }
        }
    }
}
