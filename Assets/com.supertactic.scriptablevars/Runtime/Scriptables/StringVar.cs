using UnityEngine;
using UnityEngine.Events;

namespace Supertactic.ScriptableVars
{
    [CreateAssetMenu(fileName = "String Var", menuName = "Supertactic/Variables/String Var", order = 1000)]
    public class StringVar : ScriptableObject
    {
        public string Value;

        [TextArea(3, 10)]
        public string Description;

        public UnityAction OnValueChanged;

        public void Pin(string value)
        {
            this.Value = value;
            //string activeValue = Value == string.Empty ? value : Value;

            OnValueChanged?.Invoke();
        }
    }
}
