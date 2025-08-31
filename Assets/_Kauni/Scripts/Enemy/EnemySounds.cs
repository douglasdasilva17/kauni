using System.Collections;
using UnityEngine;

namespace Supertactic
{
    public class EnemySounds : MonoBehaviour
    {
        [SerializeField] AudioClip[] gruntSounds;
        public AudioClip[] zombieMoans; // Array de clipes de som de gemidos de zumbi
        public AudioClip[] attackSounds; // Array de clipes de som de ataques de zumbi
        private AudioSource audioSource;
        public bool moanActive = true; // Flag para ativar/desativar gemidos
        public float fadeDuration = 1.0f; // Dura��o do fade out
        public float moanDelayAfterAttack = 5.0f; // Tempo de espera ap�s o som de ataque

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(PlayRandomMoans());
        }

        IEnumerator PlayRandomMoans()
        {
            while (true)
            {
                if (moanActive)
                {
                    PlayRandomMoan();

                    // Esperar at� que o som atual termine
                    yield return new WaitForSeconds(audioSource.clip.length);
                }
                else
                {
                    yield return null; // Esperar um frame antes de verificar novamente
                }
            }
        }

        void PlayRandomMoan()
        {
            if (zombieMoans.Length > 0)
            {
                int index = Random.Range(0, zombieMoans.Length);
                audioSource.clip = zombieMoans[index];
                audioSource.Play();
            }
        }

        public void SetMoanActive(bool isActive)
        {
            if (isActive)
            {
                moanActive = true;
            }
            else
            {
                StartCoroutine(FadeOutAndStop());
            }
        }

        IEnumerator FadeOutAndStop()
        {
            float startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0;
            audioSource.Stop();
            moanActive = false;
            audioSource.volume = startVolume;
        }

        public void PlayAttackSound()
        {
            if (attackSounds.Length > 0)
            {
                int index = Random.Range(0, attackSounds.Length);
                AudioClip attackClip = attackSounds[index];

                StartCoroutine(PlayAttackSoundCoroutine(attackClip));
            }
        }

        IEnumerator PlayAttackSoundCoroutine(AudioClip attackClip)
        {
            // Stop the moan if it's playing
            if (audioSource.isPlaying && moanActive)
            {
                moanActive = false;
                StopCoroutine(PlayRandomMoans());
            }

            // Play the attack sound
            audioSource.clip = attackClip;
            audioSource.volume = 1.0f; // Reset the volume for the attack sound
            audioSource.Play();

            // Wait for the attack sound to finish
            yield return new WaitForSeconds(attackClip.length);

            // Wait additional time before allowing moans to play again
            yield return new WaitForSeconds(moanDelayAfterAttack);

            // Re-enable moaning
            moanActive = true;
            StartCoroutine(PlayRandomMoans());
        }


        public void PlayGruntSounds()
        {
            AudioManager.Instance.PlaySoundFX(gruntSounds, transform, 0.5f, 1f);
        }
    }
}
