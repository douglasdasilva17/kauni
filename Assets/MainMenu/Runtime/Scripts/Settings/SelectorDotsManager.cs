using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Compixel.Settings
{
    public class SelectorDotsManager : MonoBehaviour
    {
        [Header("References")]
        public Image editImage; // UI image that indicates unsaved changes
        public List<string> optionTexts = new List<string>();
        public TextMeshProUGUI outputText;

        [Tooltip("Prefab used to represent each option as a dot")]
        public GameObject dotPrefab;

        [Tooltip("Transform where the instantiated dots will be placed")]
        public Transform dotContainer;

        [Header("Dot Colors")]
        public Color selectedColor = Color.white;
        public Color unselectedColor = Color.gray;

        [Header("Settings")]
        public string saveTag = "SelectionDot";
        public int defaultValue = 0;
        public bool loadValue = true;
        public bool saveValue = true;
        public bool initializeAtStart = true;

        [Header("Events")]
        public UnityEvent<bool> OnUnsavedChangesEvent;
        public UnityEvent<int> OnChangedEvent;

        private int lastSavedValue = -1;
        private int currentValue = 0;

        private Image[] optionDots;

        void Start()
        {            
            SpawnDots();

            if (initializeAtStart)
            {
                Initialize();
            }
            else
            {
                UpdateUI();
                UpdateEditIndicator();
            }
        }

        // Initialize the selection index based on PlayerPrefs or default
        public void Initialize()
        {
            if (loadValue && PlayerPrefs.HasKey(saveTag))
            {
                currentValue = PlayerPrefs.GetInt(saveTag);
            }
            else
            {
                currentValue = defaultValue;
            }

            if (saveValue)
            {
                PlayerPrefs.SetFloat(saveTag, currentValue);
            }

            lastSavedValue = currentValue;

            UpdateUI();

            UpdateEditIndicator();
        }

        public void SaveData()
        {
            lastSavedValue = currentValue;

            UpdateEditIndicator();
        }

        public void ResetData()
        {
            currentValue = defaultValue;

            PlayerPrefs.SetInt(saveTag, currentValue);

            UpdateUI();

            UpdateEditIndicator(true);

            OnChangedEvent.Invoke(currentValue);
        }

        // Instantiate a dot for each option text and store their references
        private void SpawnDots()
        {
            // Destroy existing children to avoid duplicates
            foreach (Transform child in dotContainer)
            {
                Destroy(child.gameObject);
            }

            optionDots = new Image[optionTexts.Count];

            for (int i = 0; i < optionTexts.Count; i++)
            {
                GameObject dot = Instantiate(dotPrefab, dotContainer);
                Image dotImage = dot.GetComponent<Image>();

                if (dotImage == null)
                {
                    Debug.LogError("The dot prefab must contain an Image component.");
                    continue;
                }

                optionDots[i] = dotImage;
            }
        }

        public void SetDefaultValue(int value)
        {
            lastSavedValue = value;
            defaultValue = value;
            currentValue = value;

            PlayerPrefs.SetInt(saveTag, currentValue);

            UpdateUI();

            UpdateEditIndicator();

            OnChangedEvent.Invoke(currentValue);
        }

        // Change selection to a specific index
        public void SelectIndex(int index)
        {
            currentValue = index;

            if (saveValue)
            {
                PlayerPrefs.SetInt(saveTag, currentValue);
            }

            UpdateUI();

            UpdateEditIndicator();

            OnChangedEvent.Invoke(currentValue);
        }

        // Move to the next option in the array
        public void NextOption()
        {
            int next = currentValue + 1;
            if (next >= optionTexts.Count) next = 0;
            SelectIndex(next);

            //UpdateEditIndicator();

            OnChangedEvent.Invoke(currentValue);
        }

        // Move to the previous option in the array
        public void PreviousOption()
        {
            int prev = currentValue - 1;
            if (prev < 0) prev = optionTexts.Count - 1;
            SelectIndex(prev);

            //UpdateEditIndicator();

            OnChangedEvent.Invoke(currentValue);
        }

        // Update the output text and dot colors
        private void UpdateUI()
        {
            if (optionTexts.Count == 0)
            { return; }

            if (optionDots.Length == 0)
            { return; }

            outputText.text = optionTexts[currentValue];

            for (int i = 0; i < optionDots.Length; i++)
            {
                if (optionDots[i] != null)
                {
                    optionDots[i].color = (i == currentValue) ? selectedColor : unselectedColor;
                }
            }
        }

        private void UpdateEditIndicator(bool isResetToDefault = false)
        {
            if (isResetToDefault)
            {
                // Show or hide the edit icon accordingly
                if (editImage != null)
                    editImage.enabled = false;

                OnUnsavedChangesEvent.Invoke(false);
            }
            else
            {
                // Set the change flag based on whether the current value differs from the saved one
                bool hasUnsavedChanges = !Mathf.Approximately(currentValue, lastSavedValue);

                OnUnsavedChangesEvent.Invoke(hasUnsavedChanges);

                // Show or hide the edit icon accordingly
                if (editImage != null)
                    editImage.enabled = hasUnsavedChanges;
            }
        }

        //Update the optionTexts and initialize it
        public void UpdateOptionTexts(List<string> optionTexts)
        {
            this.optionTexts.Clear();
            this.optionTexts = optionTexts;

            SpawnDots();
        }

        public int GetCurrentOrDefaultValue()
        {
            int value;

            if (PlayerPrefs.HasKey(saveTag))
            {
                defaultValue = PlayerPrefs.GetInt(saveTag);
                value = defaultValue;
            }
            else
            {
                value = defaultValue;
            }

            return value;
        }
    }
}
