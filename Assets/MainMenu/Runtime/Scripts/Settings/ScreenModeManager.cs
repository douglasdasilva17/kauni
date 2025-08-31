using UnityEngine;

namespace Compixel.Settings
{
    public class ScreenModeManager : MonoBehaviour
    {
        public static ScreenModeManager current;

        public SelectorDotsManager selector;

        int currentModeID;
        static FullScreenMode selectedMode = FullScreenMode.FullScreenWindow;

        public void SaveScreenMode()
        {
            SetScreenModeByID(currentModeID);
            selector.SaveData();
        }

        public void ResetScreenMode()
        {
            selector.ResetData();
        }

        public void SetCurrentValue(int value)
        {
            Debug.Log("CurrentScreenMode " + value);
            currentModeID = value;
        }

        /// <summary>
        /// Sets the screen mode based on the given ID.
        /// 0 = Windowed
        /// 1 = Borderless Fullscreen (FullscreenWindow)
        /// 2 = Exclusive Fullscreen
        /// </summary>
        private void SetScreenModeByID(int modeID)
        {
            switch (modeID)
            {
                case 0:
                    selectedMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    selectedMode = FullScreenMode.Windowed;
                    break;
                case 2:
                    selectedMode = FullScreenMode.FullScreenWindow;
                    break;
                default:
                    Debug.LogWarning("Invalid screen mode ID. Valid IDs: 0 (Windowed), 1 (Borderless), 2 (Fullscreen).");
                    return;
            }

            // Optionally, save screen resolution before changing modes
            int width = Screen.currentResolution.width;
            int height = Screen.currentResolution.height;

            Screen.SetResolution(width, height, selectedMode);

            Debug.Log("Screen mode set to: " + selectedMode);
        }

        public static FullScreenMode GetFullScreenMode()
        {
            return selectedMode;
        }
    }
}