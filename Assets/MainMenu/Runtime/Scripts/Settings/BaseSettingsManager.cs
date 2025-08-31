using System.Collections;
using UnityEngine;

namespace Compixel.Settings
{
    public abstract class BaseSettingsManager : MonoBehaviour
    {
        // Defina aqui métodos e eventos comuns:
        public virtual void InitializeSettings() { }
        public virtual void SaveSettings() { }
        public virtual void ResetSettings() { }
        public virtual bool HasUnsavedChanges() { return false; }
        public virtual void SetCurrentValue(int value) { }
    }
}