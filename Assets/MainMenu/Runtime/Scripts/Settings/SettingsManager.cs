using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Compixel.Settings
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager instance;

        [Header("REFERENCES")]
        [SerializeField] private SettingsTab[] tabs;
        [SerializeField] private Button applyButton;

        [Header("Bundle")]
        [SerializeField] private TextMeshProUGUI versionText;
        
        private List<SettingsChangeTracker> settingsChangeTrackers = new();

        void Awake()
        {
            instance = this;

            // Coleta todos os SettingsChangeTracker nos botões filhos
            foreach (var tab in tabs)
            {
                foreach (var button in tab.GetComponentsInChildren<Button>(true))
                {
                    var tracker = button.GetComponent<SettingsChangeTracker>();
                    if (tracker != null)
                    {
                        settingsChangeTrackers.Add(tracker);
                    }
                }
            }

            //Display the version of the Build
            versionText.text = $"v{Application.version}";
        }

        public void CheckUnsavedChanges()
        {
            foreach (var tracker in settingsChangeTrackers)
            {
                if (tracker.hasUnsavedChanges)
                {
                    applyButton.interactable = true;
                    applyButton.GetComponent<CanvasGroup>().DOFade(1f, 1f);
                    return;
                }
            }

            applyButton.interactable = false;
            applyButton.GetComponent<CanvasGroup>().DOFade(0.25f, 1f);
        }

        public void SaveSettings()
        {
            foreach (var tab in tabs)
            {
                if (tab.HasUnsavedChanges()) // Só salva se houver alterações
                {
                    tab.SaveSettings();
                }
            }

            CheckUnsavedChanges(); // Atualiza estado do botão após salvar
        }

        public void ResetSettings()
        {
            foreach (var tab in tabs)
            {
                tab.ResetSettings();
            }

            CheckUnsavedChanges();
        }
    }
}
