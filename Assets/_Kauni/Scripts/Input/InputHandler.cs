using Supertactic.GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Supertactic.Input
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        InputManager inputManager;
        PlayerControls inputActions;
        Vector2 moveInput;
        Vector2 lookInput;

        void OnEnable()
        {
            inputManager = GetComponent<InputManager>();

            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.Player.Move.performed += inputActions => moveInput = inputActions.ReadValue<Vector2>();
                inputActions.Player.Look.performed += i => lookInput = i.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = moveInput.x;
            vertical = moveInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = lookInput.x;
            mouseY = lookInput.y;
        }
    }
}
