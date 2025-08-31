using Supertactic.Combat;
using Supertactic.GameEvents;
using UnityEngine;

namespace Supertactic.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] WeaponItem leftWeapon; // Bow weapon
        [SerializeField] WeaponItem rightWeapon; // Spear weapon
        [SerializeField] WeaponItem handWeapon; // Hands

        [Header("Config")]
        [SerializeField] WeaponItem bowWeapon;


        WeaponSlotManager weaponSlotManager;
        public int totalWeapons;
        int currentWeapon = -1;
        int lastWeapon = -1;
        public WeaponItem GetLeftWeapon => leftWeapon;

        void Awake()
        {
            weaponSlotManager = GetComponent<WeaponSlotManager>();
        }

        private void OnEnable()
        {
            GameEventsManager.instance.weaponEvents.onBowGained += BowGained;
        }

        private void OnDisable()
        {
            GameEventsManager.instance.weaponEvents.onBowGained -= BowGained;
        }

        private void BowGained(int bow)
        {
            leftWeapon = bowWeapon;
        }

        void Start()
        {
            SwitchWeapon(currentWeapon);
        }

        public void NextWeapon()
        {
            currentWeapon++;

            if (currentWeapon > totalWeapons - 1)
                currentWeapon = -1;

            SwitchWeapon(currentWeapon);
        }

        public void SwitchWeapon(int weaponId)
        {
            switch (weaponId)
            {
                case -1:

                    weaponSlotManager.UnloadWeaponOnSlot(false);
                    weaponSlotManager.LoadWeaponOnSlot(handWeapon, true);
                    break;
                case 0:

                    weaponSlotManager.UnloadWeaponOnSlot(false);
                    weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);

                    break;
                case 1:

                    weaponSlotManager.UnloadWeaponOnSlot(true);
                    weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
                    break;
            }

            lastWeapon = weaponId;
        }
    }
}