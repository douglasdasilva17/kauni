using System.Collections.Generic;
using UnityEngine;

namespace Compixel.Settings
{
    public class AntialiasingManager : BaseSettingsManager
    {
        public SelectorDotsManager Selector;

        private readonly List<string> aaOptions = new List<string>
        {
            "Desligado", // 0x
            "Baixo",     // 2x
            "Médio",     // 4x
            "Alto"       // 8x
        };

        private readonly List<int> aaLevels = new List<int>
        {
            0,  // Desligado
            2,  // Baixo
            4,  // Médio
            8   // Alto
        };

        private int currentQualityID;

        public void Start()
        {
            // Atualiza visualmente as op��es no seletor
            Selector.UpdateOptionTexts(aaOptions);

            // Define valor atual a partir do QualitySettings
            int currentAA = QualitySettings.antiAliasing;
            currentQualityID = aaLevels.IndexOf(currentAA);
            if (currentQualityID == -1) currentQualityID = 0;

            // Aplica o valor inicial
            SetAntialiasingByQuality(Selector.defaultValue);

            // Set forced the value
            Selector.SetDefaultValue(Selector.defaultValue);
        }

        public override void SaveSettings()
        {
            SetAntialiasingByQuality(currentQualityID);
            Selector.SaveData();
        }

        public override void ResetSettings()
        {
            Selector.ResetData();
        }

        public void UpdateSelectorIndex(int value)
        {
            Selector.SelectIndex(value);
        }

        public override void SetCurrentValue(int value)
        {
            currentQualityID = value;
        }

        public void SetAntialiasingByQuality(int qualityID)
        {
            if (qualityID < 0 || qualityID >= aaLevels.Count)
            {
                Debug.LogWarning("Invalid AA quality ID.");
                return;
            }

            int aaLevel = aaLevels[qualityID];
            QualitySettings.antiAliasing = aaLevel;
            Debug.Log($"Antialiasing set to: {aaLevel}x ({aaOptions[qualityID]})");
        }
    }
}
