using System.Collections;
using Michsky.UI.Dark;
using UnityEngine;

namespace Compixel.Settings
{
    public class BloodFXManager : BaseSettingsManager
    {
        public static BloodFXManager instance;

        public SwitchManager Switch;
        int currentvSyncID;

        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1);

            Switch.SetDefaultValue();
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

        // M�todo para ativar o VSync
        public static void EnableVSync()
        {
            QualitySettings.vSyncCount = 1; // Ativa o VSync (1 significa que ele ser� ativado)
            Debug.Log("VSync enabled");
        }

        // M�todo para desativar o VSync
        public static void DisableVSync()
        {
            QualitySettings.vSyncCount = 0; // Desativa o VSync
            Debug.Log("VSync disabled");
        }

        // M�todo para verificar se o VSync est� ativado
        public static bool IsVSyncEnabled()
        {
            return QualitySettings.vSyncCount > 0; // Retorna verdadeiro se o VSync estiver ativado
        }
    }
}
