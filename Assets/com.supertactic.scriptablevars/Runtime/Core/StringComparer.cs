using Supertactic.ScriptableVars;
using UnityEngine;
using UnityEngine.Events;

public class StringComparer : MonoBehaviour
{
    public string StringToCompare;
    public StringVar Value;
    public UnityEvent OnTrue;
    public UnityEvent OnFalse;

    private void OnEnable()
    {
       Value.OnValueChanged += HandleOnValueChanged;
    }

    private void OnDisable()
    {
       Value.OnValueChanged -= HandleOnValueChanged;
    }

    void HandleOnValueChanged()
    {
        if (StringToCompare == Value.Value)
            OnTrue?.Invoke();
        else
            OnFalse?.Invoke(); 
    }
}
