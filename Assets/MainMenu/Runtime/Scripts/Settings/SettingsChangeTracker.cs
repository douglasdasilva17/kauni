using UnityEngine;

namespace Compixel.Settings
{
    public class SettingsChangeTracker : MonoBehaviour
    {        
        public bool hasUnsavedChanges { get; private set; } = false; // Flag to indicate if there are unsaved changes

        public void SetUnsavedChanges(bool value)
        {
            hasUnsavedChanges = value;

            SettingsManager.instance.CheckUnsavedChanges();
        }
    }
}
