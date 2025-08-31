using UnityEngine;
using UnityEngine.Events;

namespace Compixel.Settings
{
    public class SettingsButton : MonoBehaviour
    {
        public UnityEvent OnSaveEvent;
        public UnityEvent OnResetEvent;

        // Called to save the current brightness value if it has changed
        public void SaveSettings()
        {
            OnSaveEvent.Invoke();
        }

        public void ResetSettings()
        {
            OnResetEvent.Invoke();
        }
    }
}
