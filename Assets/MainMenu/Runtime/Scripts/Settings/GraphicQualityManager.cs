using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Compixel.Settings
{
    public class GraphicsQualityManager : BaseSettingsManager
    {
        public SelectorDotsManager Selector;

        private List<string> qualityLevels = new List<string> { "Baixa", "Média", "Alta", "Muito Alta" };
        private int currentQualityIndex = 0;

        [Tooltip("Event called when the player goes to the next quality level (not yet saved)")]
        [SerializeField] private UnityEvent<int> OnQualityLevelChanged;

        public void Start()
        {
            // Atualiza o seletor com os nomes fixos das qualidades
            Selector.UpdateOptionTexts(qualityLevels);

            // Pega o nível de qualidade atual do Unity e limita para o intervalo de 0–3
            int realIndex = Mathf.Clamp(QualitySettings.GetQualityLevel(), 0, 3);
            currentQualityIndex = realIndex;

            SetQualityByID(Selector.defaultValue);

            Selector.SelectIndex(Selector.defaultValue);
        }

        public override void SaveSettings()
        {
            SetQualityByID(currentQualityIndex);
            Selector.SaveData();
        }

        public override void ResetSettings()
        {
            Selector.ResetData();
        }

        public override void SetCurrentValue(int value)
        {
            currentQualityIndex = value;
            OnQualityLevelChanged.Invoke(value);
        }

        public void SetQualityByID(int id)
        {
            if (id < 0 || id >= qualityLevels.Count)
            {
                Debug.LogWarning("Invalid Quality ID: " + id);
                return;
            }

            QualitySettings.SetQualityLevel(id, true);
            Debug.Log($"Qualidade gráfica definida para: {qualityLevels[id]}");
        }
    }
}
