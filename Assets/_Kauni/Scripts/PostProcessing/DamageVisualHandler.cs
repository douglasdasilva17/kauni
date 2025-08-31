using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Supertactic
{
    public class DamageVisualHandler : MonoBehaviour
    {
        private const float recoverSpeed = 0.005f;

        private float currentIntensity = 0;
        private Volume volume;
        private Vignette vignette;

        private void Start()
        {
            volume = GetComponent<Volume>();
            volume.profile.TryGet<Vignette>(out vignette);

            if (!vignette)
            {
                Debug.LogWarning("Error vignette not found");
            }
            else
            {
                vignette.SetAllOverridesTo(false);
            }
        }


        public void Execute(float intensity = 0.4f)
        {
            StartCoroutine(TakeDamageRoutine(intensity));
        }

        private IEnumerator TakeDamageRoutine(float intensity)
        {
            currentIntensity = intensity;

            vignette.SetAllOverridesTo(true);
            vignette.intensity.Override(currentIntensity);

            yield return new WaitForSeconds(currentIntensity);

            while (currentIntensity > 0)
            {
                currentIntensity -= recoverSpeed;

                if (currentIntensity < 0) currentIntensity = 0;

                vignette.intensity.Override(currentIntensity);

                yield return new WaitForSeconds(0.1f);
            }

            vignette.SetAllOverridesTo(false);
            yield break;
        }
    }
}
