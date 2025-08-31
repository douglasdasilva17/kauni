using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PressButtonEvent : MonoBehaviour
{
    [Header("INPUT ACTION")]
    [SerializeField] private InputActionReference inputActionReference;

    [Header("EVENT")]
    [SerializeField] private UnityEvent pressAction;

    private void OnEnable()
    {
        if (inputActionReference != null)
        {
            inputActionReference.action.performed += OnPerformed;
            inputActionReference.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputActionReference != null)
        {
            inputActionReference.action.performed -= OnPerformed;
            inputActionReference.action.Disable();
        }
    }

    private void OnPerformed(InputAction.CallbackContext context)
    {
        pressAction?.Invoke();
    }
}
