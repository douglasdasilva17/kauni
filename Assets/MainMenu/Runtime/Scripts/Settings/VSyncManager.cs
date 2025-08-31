using Michsky.UI.Dark;
using UnityEngine;

namespace Compixel.Settings
{    
    public class VSyncManager : BaseSettingsManager
    {
        public SwitchManager Switch;
        public static VSyncManager instance;
        int currentvSyncID;

        private void Awake()
        {
            instance = this;
        }

        public override void SaveSettings()
        {
            SetvSyncByID(currentvSyncID);
            Switch.SaveData();
        }

        public override void ResetSettings()
        {
            Switch.ResetData();
        }

        public override void SetCurrentValue(int value)
        {
            currentvSyncID = value;   
        }

        private void SetvSyncByID(int vSyncID)
        {
            if (currentvSyncID == 0)
            {
                DisableVSync();
            }
            else
            {
                EnableVSync();
            }
        }

        // Método para ativar o VSync
        public static void EnableVSync()
        {
            QualitySettings.vSyncCount = 1; // Ativa o VSync (1 significa que ele será ativado)
            Debug.Log("VSync enabled");
        }

        // Método para desativar o VSync
        public static void DisableVSync()
        {
            QualitySettings.vSyncCount = 0; // Desativa o VSync
            Debug.Log("VSync disabled");
        }

        // Método para verificar se o VSync está ativado
        public static bool IsVSyncEnabled()
        {
            return QualitySettings.vSyncCount > 0; // Retorna verdadeiro se o VSync estiver ativado
        }
    }
}
