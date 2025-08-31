using System.Collections.Generic;
using UnityEngine;

namespace Compixel.Settings
{
    public class ResolutionManager : MonoBehaviour
    {
        public SelectorDotsManager Selector;

        Resolution[] allResolutions;
        int nativeRes;
        int currentRes;
        List<Resolution> selectedResolutions = new List<Resolution>();

        void Start()
        {
            allResolutions = Screen.resolutions;

            // Define some commonly used resolutions
            List<Vector2Int> importantResolutions = new List<Vector2Int>
            {
                new Vector2Int(1280, 720),   // HD
                new Vector2Int(1600, 900),   // HD+
                new Vector2Int(1920, 1080),  // Full HD
                new Vector2Int(2560, 1440),  // QHD
               // new Vector2Int(3840, 2160),  // 4K
            };

            // Add native resolution
            Resolution native = Screen.currentResolution;
            Vector2Int nativeSize = new Vector2Int(native.width, native.height);

            if (!importantResolutions.Contains(nativeSize))
            {
                importantResolutions.Insert(0, nativeSize); // ensure native comes first
            }

            List<string> resolutionStringList = new List<string>();

            foreach (Vector2Int size in importantResolutions)
            {
                foreach (Resolution res in allResolutions)
                {
                    if (res.width == size.x && res.height == size.y)
                    {
                        selectedResolutions.Add(res);
                        resolutionStringList.Add($"{res.width} x {res.height}");
                        break;
                    }
                }
            }

            Selector.UpdateOptionTexts(resolutionStringList);

            // Set native resolution as default
            for (int i = 0; i < selectedResolutions.Count; i++)
            {
                if (selectedResolutions[i].width == native.width &&
                    selectedResolutions[i].height == native.height)
                {
                    nativeRes = i;
                    break;
                }
            }

            currentRes = nativeRes;

            Selector.SetDefaultValue(currentRes);

            SetResolutionByID(currentRes);

            //Selector.Initialize();
        }

        public void InitializeResolution()
        {
            //Not emplementation
        }

        public void SaveResolution()
        {
            SetResolutionByID(currentRes);

            Selector.SaveData();
        }

        public void ResetResolution()
        {
            Selector.ResetData();
        }

        public void SetCurrentValue(int value)
        {
            currentRes = value;
        }

        private void SetResolutionByID(int resolutionID)
        {
            if (resolutionID < 0 || resolutionID >= selectedResolutions.Count)
            {
                Debug.LogWarning("Invalid resolution ID: " + resolutionID);
                return;
            }

            Resolution resolution = selectedResolutions[resolutionID];
            Screen.SetResolution(resolution.width, resolution.height, ScreenModeManager.GetFullScreenMode());

            Debug.Log($"Resolution set to: {resolution.width}x{resolution.height}");
        }
    }
}
