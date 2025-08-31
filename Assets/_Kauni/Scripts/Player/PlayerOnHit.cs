using Supertactic.Mukani;
using UnityEngine;

namespace Supertactic
{
    [RequireComponent(typeof(Damageable))]
    public class PlayerOnHit : MonoBehaviour
    {
        [Header("Sounds")]
        [SerializeField] private AudioClip[] gruntClips;

        private Animator _animator;

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
            if (damageType == EDamageType.Normal)
            {
                StartHit();
            }
        }

        public void StartHit()
        {
            // Perform visual effects of damage if the target collider is from the player
            _damageVisualHandler.Execute();
            //TODO: Implement hit reaction animation for the player and the enemy
            AudioManager.Instance.PlaySoundFX(gruntClips, transform, UnityEngine.Random.Range(0.2f, 0.4f), 1f);
        }
    }
}
