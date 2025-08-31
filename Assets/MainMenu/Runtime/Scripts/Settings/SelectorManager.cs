using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

namespace Compixel.Settings
{
    public class SelectorManager : MonoBehaviour
    {
        [Header("SETTINGS")]
        [Tooltip("Minimum value")]
        public float minValue = 0f;

        [Tooltip("Maximum value")]
        public float maxValue = 1f;

        [Tooltip("Default value if not loading")]
        public float defaultValue = 0.5f;

        [Header("DATA")]
        [Tooltip("Tag to identify brightness setting")]
        public string saveTag = "Brightness";

        [Tooltip("Automatically save value when changed")]
        public bool saveValue = false;

        [Tooltip("Load saved value on start")]
        public bool loadValue = true;
        public bool invokeAtStart = false;

        [Header("REFERENCES")]
        public Image editImage; // UI image that indicates unsaved changes
        public TextMeshProUGUI outputText;
        public Image outputImage;
        public bool displayFullValue;

        [Header("KEY ACTIONS")]
        public UnityEvent<bool> OnUnsavedChangesEvent;
        public UnityEvent<float> OnChangedEvent;
        [Tooltip("Event caleld when brightness value was increased")]
        public UnityEvent<float> OnIncreaseEvent;
        [Tooltip("Event caleld when brightness value was decreased")]
        public UnityEvent<float> OnDecreaseEvent;

        private float lastSavedValue; // Stores the last saved brightness value
        private float currentValue = 0f;

        public float GetCurrentValue => currentValue;

        public float GetLastSavedValue => PlayerPrefs.GetFloat(
                saveTag,
                defaultValue
            );

        void Start()
        {
            Initialize();
        }

        //Call this method when the menu is open
        private void Initialize()
        {
            if (loadValue && PlayerPrefs.HasKey(saveTag))
            {
                currentValue = PlayerPrefs.GetFloat(saveTag);
            }
            else
            {
                currentValue = defaultValue;
            }

            if (saveValue)
            {
                PlayerPrefs.SetFloat(saveTag, currentValue);
            }

            DisplayOutput(currentValue);

            lastSavedValue = currentValue;

            UpdateEditIndicator();

            if (invokeAtStart)
            {
                OnChangedEvent.Invoke(currentValue);
            }
        }

        public void SaveData()
        {
            lastSavedValue = currentValue;
            PlayerPrefs.SetFloat(saveTag, currentValue);

            UpdateEditIndicator();
        }

        public void ResetData()
        {
            currentValue = defaultValue;
            DisplayOutput(currentValue);
            OnChangedEvent.Invoke(currentValue);
        }

        public void SetDefaultValue()
        {
            if (loadValue && PlayerPrefs.HasKey(saveTag))
            {
                currentValue = PlayerPrefs.GetFloat(saveTag);
            }
            else
            {
                currentValue = defaultValue;
            }

            DisplayOutput(currentValue);
        }

        public void IncreaseValue(float amount)
        {
            currentValue = Mathf.Clamp(currentValue + amount, minValue, maxValue);

            if (saveValue)
            {
                PlayerPrefs.SetFloat(saveTag, currentValue);
            }

            OnIncreaseEvent.Invoke(currentValue);
            OnChangedEvent.Invoke(currentValue);
            DisplayOutput(currentValue);
        }

        public void DecreaseValue(float amount)
        {
            currentValue = Mathf.Clamp(currentValue - amount, minValue, maxValue);

            if (saveValue)
            {
                PlayerPrefs.SetFloat(saveTag, currentValue);
            }

            OnDecreaseEvent.Invoke(currentValue);
            OnChangedEvent.Invoke(currentValue);
            DisplayOutput(currentValue);
        }

        public void AdjustValueSlider(float value)
        {
            currentValue = Mathf.Clamp(value, minValue, maxValue);

            if (saveValue)
            {
                PlayerPrefs.SetFloat(saveTag, currentValue);
            }

            OnChangedEvent.Invoke(currentValue);
            DisplayOutput(currentValue);
        }

        private void DisplayOutput(float value)
        {
            float fullValue = value * 100f;
            outputText.text = displayFullValue ? minValue + "/" + Mathf.RoundToInt(fullValue) : Mathf.RoundToInt(fullValue) + "%";
            outputImage.fillAmount = value;

            UpdateEditIndicator();
        }

        private void UpdateEditIndicator()
        {
            // Set the change flag based on whether the current value differs from the saved one
            bool hasUnsavedChanges = !Mathf.Approximately(currentValue, lastSavedValue);

            OnUnsavedChangesEvent.Invoke(hasUnsavedChanges);

            // Show or hide the edit icon accordingly
            if (editImage != null)
                editImage.enabled = hasUnsavedChanges;
        }
    }
}