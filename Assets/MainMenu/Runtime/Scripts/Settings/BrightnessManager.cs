using Compixel.Settings;
using UnityEngine;

namespace Compixel.Settings
{
    public class BrightnessManager : MonoBehaviour
    {
        public SelectorManager selector;

        public void SaveBrightness()
        {
            selector.SaveData();
        }

        public void ResetBrightness()
        {
            selector.ResetData();
        }
    }
}
