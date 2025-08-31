using UnityEngine;

namespace Supertactic
{
    public class PlayerSounds : MonoBehaviour
    {
        [Header("Action Sounds")]
        [SerializeField] AudioClip[] voiceRollClips;
        [SerializeField] AudioClip[] clothRollClips;

        [Header("Parkour Sounds")]
        [SerializeField] AudioClip[] stepUpClips;
        [SerializeField] AudioClip[] jumpUpClips;
        [SerializeField] AudioClip[] climbUpClips;

        [SerializeField] AudioClip[] spearClips;

        public void StepUpSFX()
        {
            AudioManager.Instance.PlaySoundFX(stepUpClips, transform, UnityEngine.Random.Range(0.1f, 0.25f), 1f);
        }

        public void JumpUpSFX()
        {
            AudioManager.Instance.PlaySoundFX(jumpUpClips, transform, UnityEngine.Random.Range(0.2f, 0.4f), 1f);
        }

        public void ClimbUpSFX()
        {
            AudioManager.Instance.PlaySoundFX(climbUpClips, transform, UnityEngine.Random.Range(0.2f, 0.4f), 1f);
        }

        void PlayRollSFX()
        {
            AudioManager.Instance.PlaySoundFX(voiceRollClips, transform, UnityEngine.Random.Range(0.2f, 0.5f), 1f);
            AudioManager.Instance.PlaySoundFX(clothRollClips, transform, UnityEngine.Random.Range(0.1f, 0.2f), 1f);
        }

        private void SpearAttackEvent()
        {
            AudioManager.Instance.PlaySoundFX(spearClips, transform, UnityEngine.Random.Range(0.2f, 0.5f), 1f);
        }
    }
}