using System.Collections.Generic;
using UnityEngine;

namespace Compixel.Settings
{
    public class ShadowsManager : BaseSettingsManager
    {
        public SelectorDotsManager Selector;

        private readonly List<string> shadowOptions = new List<string>
        {
            "Desligado", // 0
            "Baixa",    // 1 (funciona como Desligado)
            "Alta",    // 2 (HardOnly)
            "Muito Alta" // 3 (All)
        };

        private readonly List<ShadowQuality> shadowQualities = new List<ShadowQuality>
        {
            ShadowQuality.Disable,   // Desligado
            ShadowQuality.Disable,   // Baixa (mesmo que desligado)
            ShadowQuality.HardOnly,  // Alta
            ShadowQuality.All        // Muito Alta
        };

        private int currentShadowID;

        public void Start()
        {
            Selector.UpdateOptionTexts(shadowOptions);

            ShadowQuality currentShadowQuality = QualitySettings.shadows;
            currentShadowID = shadowQualities.IndexOf(currentShadowQuality);
            if (currentShadowID == -1) currentShadowID = 0;

            SetShadowsByQuality(Selector.defaultValue);
            Selector.SetDefaultValue(Selector.defaultValue);
        }

        public override void SaveSettings()
        {
            SetShadowsByQuality(currentShadowID);
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
            currentShadowID = value;
        }

        public void SetShadowsByQuality(int qualityID)
        {
            if (qualityID < 0 || qualityID >= shadowQualities.Count)
            {
                Debug.LogWarning("Invalid Shadow quality ID.");
                return;
            }

            ShadowQuality selectedQuality = shadowQualities[qualityID];
            QualitySettings.shadows = selectedQuality;

            //Debug.Log($"Sombras definidas como: {selectedQuality} ({shadowOptions[qualityID]})");
        }
    }
}
