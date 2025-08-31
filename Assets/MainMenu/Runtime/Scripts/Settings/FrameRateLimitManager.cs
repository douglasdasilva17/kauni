using System.Collections.Generic;
using UnityEngine;

namespace Compixel.Settings
{
    public class FrameRateLimitManager : BaseSettingsManager
    {
        public SelectorDotsManager Selector;

        // All available FPS options
        private readonly List<int> allFpsOptions = new List<int> { 0, 30, 60, 120, 144, 165, 240 };
        private List<int> validFpsOptions = new List<int>();

        private int currentFPSIndex = 0;

        public void Start()
        {
            // Detect max refresh rate of the current monitor
            RefreshRate refreshRatio = Screen.currentResolution.refreshRateRatio;
            int maxRefreshRate = (int)refreshRatio.value;

            // Filter valid FPS options
            validFpsOptions.Clear();
            foreach (int fps in allFpsOptions)
            {
                if (fps == 0 || fps <= maxRefreshRate)
                {
                    validFpsOptions.Add(fps);
                }
            }

            // Generate label list
            List<string> fpsLabels = new List<string>();
            foreach (int fps in validFpsOptions)
            {
                fpsLabels.Add(fps == 0 ? "Ilimitado" : $"{fps} Hz");
            }

            // Initialize SelectorDotsManager with filtered list
            Selector.UpdateOptionTexts(fpsLabels);

            currentFPSIndex = Selector.GetCurrentOrDefaultValue();

            // Set initial FPS value and apply settings
            SetFrameRateLimitByID(currentFPSIndex);

            // Set default selector value
            Selector.SetDefaultValue(currentFPSIndex);
        }

        public override void SaveSettings()
        {
            SetFrameRateLimitByID(currentFPSIndex);
            Selector.SaveData();
        }

        public override void ResetSettings()
        {
            Selector.ResetData();
        }

        public override void SetCurrentValue(int value)
        {
            currentFPSIndex = value;
        }

        public void SetFrameRateLimitByID(int frameRateLimitID)
        {
            if (frameRateLimitID < 0 || frameRateLimitID >= validFpsOptions.Count)
            {
                Debug.LogWarning("Invalid FPS ID: " + frameRateLimitID);
                return;
            }

            int fps = validFpsOptions[frameRateLimitID];

            // Only apply FPS if VSync is disabled
            if (!VSyncManager.IsVSyncEnabled())
            {
                Application.targetFrameRate = fps;
                Debug.Log($"Frame rate set to: {(fps == 0 ? "Unlimited" : fps.ToString())}");
            }
        }
    }
}
