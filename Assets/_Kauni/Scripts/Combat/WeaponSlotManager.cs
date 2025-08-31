using Supertactic.Player;
using UnityEngine;

namespace Supertactic.Combat
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private WeaponHolderSlot leftHandSlot;
        private WeaponHolderSlot rightHandSlot;
        private PlayerManager playerManager;

        void Awake()
        {
            WeaponHolderSlot[] slots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in slots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }

            playerManager = GetComponent<PlayerManager>();
        }

        public void UnloadWeaponOnSlot(bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.UnloadWeapon();
            }
            else
            {
                rightHandSlot.UnloadWeapon();
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
            }
        }

        //public void SpearAttackEvent()
        //{
        //    rightHandSlot.CurrentWeaponModel.GetComponent<Spear>().EnableTriggerBox();
        //}

        //public void SpearReleaseEvent()
        //{
        //    rightHandSlot.CurrentWeaponModel.GetComponent<Spear>().DisableTriggerBox();
        //}
    }
}
