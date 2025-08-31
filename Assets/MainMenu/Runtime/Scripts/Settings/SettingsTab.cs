using UnityEngine;

namespace Compixel.Settings
{
    public class SettingsTab : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private SettingsButton[] buttons;

        //public void InitializeSettings()
        //{
        //    foreach (var button in buttons)
        //    {
        //        button.InitializeSettings();
        //    }
        //}

        public void SaveSettings()
        {
            if (buttons.Length > 0)
            {
                foreach (var button in buttons)
                {
                    button.SaveSettings();
                }
            }
        }

        public void ResetSettings()
        {
            if (buttons.Length > 0)
            {
                foreach (var button in buttons)
                {
                    button.ResetSettings();
                }
            }
        }

        //Verifica se há alterações não salvas nesta aba
        public bool HasUnsavedChanges()
        {
            foreach (var button in buttons)
            {
                var tracker = button.GetComponent<SettingsChangeTracker>();
                if (tracker != null && tracker.hasUnsavedChanges)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
