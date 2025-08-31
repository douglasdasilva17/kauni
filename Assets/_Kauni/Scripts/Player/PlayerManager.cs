using UnityEngine;
using Supertactic.Combat;
using Supertactic.Input;
using Supertactic.Inventory;

namespace Supertactic.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] InputReader input;
        public PlayerLocomotion PlayerLocomotion { get; private set; }
        public PlayerAnimator PlayerAnimator { get; private set; }
        public PlayerCombat PlayerArchery { get; private set; }
        public PlayerInventory PlayerInventory { get; private set; }

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool isRunning = false;

        [Header("RATE")]
        [Tooltip("How fast the character holst his weapons")]
        [SerializeField] float holsterRate = 0.5f;
        float holsterTimer;
        bool canSwitchWeapon;
        bool isWeaponAlreadyEquipped;

        void Awake()
        {
            PlayerLocomotion = GetComponent<PlayerLocomotion>();
            PlayerAnimator = GetComponent<PlayerAnimator>();
            PlayerArchery = GetComponent<PlayerCombat>();
            PlayerInventory = GetComponent<PlayerInventory>();
        }

        private void OnEnable()
        {

            input.EnablePlayerActions();
            input.SwitchWeapon += OnSwitchWeapon;
        }

        private void OnDisable()
        {
            input.SwitchWeapon -= OnSwitchWeapon;
        }

        void OnSwitchWeapon(bool isSwitched)
        {
            if (isSwitched && canSwitchWeapon)
            {
                if (PlayerInventory.GetLeftWeapon)
                {
                    PlayerInventory.SwitchWeapon(isWeaponAlreadyEquipped ? -1 : 0);
                    canSwitchWeapon = false;
                    isWeaponAlreadyEquipped = !isWeaponAlreadyEquipped;
                }
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (!canSwitchWeapon)
            {
                holsterTimer += Time.deltaTime;
                if (holsterTimer > holsterRate)
                {
                    holsterTimer = 0;
                    canSwitchWeapon = true;
                }
            }

            PlayerLocomotion.HandleDodgeInput(input.MoveAxis.x, input.MoveAxis.y, input.DodgeInput);
            PlayerArchery.HandleBowAimShootInput();
        }
    }
}