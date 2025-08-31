using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Compixel.Settings
{
    public class AudioMixerManager : BaseSettingsManager
    {
        public SelectorManager selector;
        public AudioMixer audioMixer; // Reference to the Unity AudioMixer
        public string exposedParameter = "MasterVolume"; // Exposed parameter name in the mixer
        //public SliderManager slider; // UI slider component
        public static AudioMixerManager instance;

        private float currentVolume = 1.0f; // Default to full volume

        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1);

            // Get saved value or default
            currentVolume = selector.GetCurrentValue;
            SetVolume(currentVolume);
        }

        public override void SaveSettings()
        {
            SetVolume(currentVolume);
            selector.SaveData();
        }

        public override void ResetSettings()
        {
            selector.ResetData();
        }

        public void IncreaseVolume(float step = 0.05f)
        {
            currentVolume = Mathf.Clamp01(currentVolume + step);
            SetVolume(currentVolume);
            //slider.mainSlider.value = currentVolume;
        }

        public void DecreaseVolume(float step = 0.05f)
        {
            currentVolume = Mathf.Clamp01(currentVolume - step);
            SetVolume(currentVolume);
            //slider.mainSlider.value = currentVolume;
        }

        // Called by UnityEvent
        public void OnValueChanged(float newValue)
        {
            currentVolume = newValue;
            SetVolume(currentVolume);
        }

        private void SetVolume(float value)
        {
            // Converts from linear slider (0-1) to decibels (-80dB to 0dB)
            float volumeInDb = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
            audioMixer.SetFloat(exposedParameter, volumeInDb);
            Debug.Log($"Set {exposedParameter} to {volumeInDb} dB");
        }
    }
}
