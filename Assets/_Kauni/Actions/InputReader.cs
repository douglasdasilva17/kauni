using Supertactic.ScriptableVars;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerControls;

namespace Supertactic.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2, bool> Look = delegate { };
        public event UnityAction<bool> Run = delegate { };
        public event UnityAction<bool> Jump = delegate { };
        public event UnityAction<bool> Dodge = delegate { };
        public event UnityAction<bool> Aim = delegate { };
        public event UnityAction<bool> Fire = delegate { };
        public event UnityAction<bool> SwitchWeapon = delegate { };
        public event UnityAction<bool> Submit = delegate { };

        PlayerControls inputActions;

        public Vector2 MoveAxis => inputActions.Player.Move.ReadValue<Vector2>();

        public Vector2 LookAxis => inputActions.Player.Look.ReadValue<Vector2>();

        public bool RunInput => inputActions.Player.Run.ReadValue<float>() > 0.5f;

        public bool AimInput => inputActions.Player.Aim.ReadValue<float>() > 0.5f;

        public bool FireInput => inputActions.Player.Fire.ReadValue<float>() > 0.5f;

        public bool DodgeInput => inputActions.Player.Dodge.ReadValue<float>() > 0.5f;

        public bool SubmitInput => inputActions.Player.Submit.ReadValue<float>() > 0.5f;

        public bool SwitchWeaponInput => inputActions.Player.SwitchWeapon.ReadValue<float>() > 0.5f;
        public StringVar CurrentControlScheme;

        void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.Player.SetCallbacks(this);
            }
        }

        void OnDisable()
        {
            inputActions.Disable();
        }

        public void DisablePlayerActions()
        {
            inputActions.Disable();
        }

        public void EnablePlayerActions()
        {
            inputActions.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
        }

        private bool IsDeviceMouse(InputAction.CallbackContext context)
        {
            return context.control.device.name == CurrentControlScheme.Value;
            //return context.control.device.name == "Mouse";
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Fire.Invoke(true);
                    break;
                case InputActionPhase.Performed:
                    Fire.Invoke(false);
                    break;
                case InputActionPhase.Canceled:
                    Fire.Invoke(false);
                    break;
            }
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Dodge.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Dodge.Invoke(false);
                    break;
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Run.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Run.Invoke(false);
                    break;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Jump.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump.Invoke(false);
                    break;
            }
        }

        public void OnSwitchWeapon(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    SwitchWeapon.Invoke(true);
                    break;
                case InputActionPhase.Disabled:
                    SwitchWeapon.Invoke(false);
                    break;
            }
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Aim.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Aim.Invoke(false);
                    break;
            }
        }        

        public void OnSubmit(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Submit.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Submit.Invoke(false);
                    break;
            }
        }
    }
}