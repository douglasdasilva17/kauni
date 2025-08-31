using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Michsky.UI.Dark
{
    public class SwitchManager : MonoBehaviour
    {
        [Header("SETTINGS")]
        [Tooltip("IMPORTANT! EVERY SWITCH MUST HAVE A DIFFERENT TAG")]
        public string switchTag = "Switch";
        public bool isOn = true;
        public bool saveValue = true;
        public bool defaultValue = false;
        public bool invokeAtStart = true;
        public float switchDelayClick = 1f;

        [Header("References")]
        public Image editImage; // Indicador de edição

        [Header("Events")]
        public UnityEvent OnEvents;
        public UnityEvent OffEvents;

        public UnityEvent<bool> OnUnsavedChangesEvent;

        private Animator switchAnimator;
        private Button switchButton;
        private bool lastSavedValue;
        [SerializeField] bool debugLog;

        void Start()
        {
            switchAnimator = GetComponent<Animator>();
            switchButton = GetComponent<Button>();
            switchButton.onClick.AddListener(AnimateSwitch);

            // Loads saved data
            if (PlayerPrefs.HasKey(switchTag))
            {
                string saved = PlayerPrefs.GetString(switchTag);
                isOn = (saved == "true");
            }

            // Apply the value to UI
            if (isOn)
            {
                switchAnimator.Play("Switch On");
            }
            else
            {
                switchAnimator.Play("Switch Off");
            }

            //Set default value as the current
            lastSavedValue = isOn;

            /*
              Invoke the OnEvents or OffEvents to guarantee the
              first state of other systems that listen these events
            */
            if (invokeAtStart)
            {
                if (isOn)
                    OnEvents.Invoke();
                else
                    OffEvents.Invoke();
if(debugLog)
                Debug.Log($"Switch changed to:{isOn}");
            }

            UpdateEditIndicator();
        }

        public void SetDefaultValue()
        {
            PlayerPrefs.SetString(switchTag, defaultValue ? "true" : "false");

            if (editImage != null)
                editImage.enabled = false;

            isOn = defaultValue;

            // Apply the value to UI
            if (defaultValue)
            {
                switchAnimator.Play("Switch On");
            }
            else
            {
                switchAnimator.Play("Switch Off");
            }
        }

        public void AnimateSwitch()
        {
            if (!switchButton.interactable)
                return;

            // Block interactions
            switchButton.interactable = false;

            isOn = !isOn;

            if (isOn)
            {
                switchAnimator.Play("Switch On");
                OnEvents.Invoke();
            }
            else
            {
                switchAnimator.Play("Switch Off");
                OffEvents.Invoke();
            }

            if (saveValue)
            {
                PlayerPrefs.SetString(switchTag, isOn ? "true" : "false");
            }

            UpdateEditIndicator();

            // Reativa o botão após um tempo (ajuste o tempo conforme a animação)
            Invoke(nameof(EnableInteraction), switchDelayClick); // tempo de delay entre cliques
        }

        private void EnableInteraction()
        {
            switchButton.interactable = true;
        }

        public void SaveData()
        {
            PlayerPrefs.SetString(switchTag, isOn ? "true" : "false");

            lastSavedValue = isOn;

            UpdateEditIndicator();
        }

        public void ResetData()
        {
            lastSavedValue = defaultValue;
            isOn = defaultValue;

            PlayerPrefs.SetString(switchTag, isOn ? "true" : "false");

            if (isOn)
            {
                switchAnimator.Play("Switch On");
                OnEvents.Invoke();
            }
            else
            {
                switchAnimator.Play("Switch Off");
                OffEvents.Invoke();
            }

            UpdateEditIndicator(true);
        }

        private void UpdateEditIndicator(bool isResetToDefault = false)
        {
            if (isResetToDefault)
            {
                if (editImage != null)
                    editImage.enabled = false;

                OnUnsavedChangesEvent.Invoke(false);
            }
            else
            {
                bool changed = isOn != lastSavedValue;

                if (editImage != null)
                    editImage.enabled = changed;

                OnUnsavedChangesEvent.Invoke(changed);
            }
        }
    }
}
