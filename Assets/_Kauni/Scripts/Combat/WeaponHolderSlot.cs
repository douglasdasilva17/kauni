using UnityEngine;
using Supertactic.Player;

namespace Supertactic.Combat
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        [SerializeField] Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public bool Occupied => currentWeaponModel != null;

        [SerializeField] GameObject currentWeaponModel;
        [SerializeField] WeaponItem currentWeaponItem;
        [SerializeField] PlayerManager player;

        public GameObject CurrentWeaponModel => currentWeaponModel;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        /// <summary>
        /// Instantiate a weapon prefab based on scriptable object
        /// </summary>
        /// <param name="weaponItem"></param>
        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            //   Player player = transform.root.GetChild(1).GetComponent<Player>();

            SwapWeapons(weaponItem, player);

            if (weaponItem.weaponId == 0)
            {
                player.PlayerArchery.bowWeapon = model.GetComponent<Bow>();
            }
            else if (weaponItem.weaponId == 1)
            {
                player.PlayerArchery.spearWeapon = model.GetComponent<Spear>();
            }

            if (model != null)
            {
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    // Set the parent of the model as the transform of this script
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }
            currentWeaponItem = weaponItem;
            currentWeaponModel = model;
        }

        void SwapWeapons(WeaponItem weaponItem, PlayerManager player)
        {
            // Play the animation from the last item
            //if (currentWeaponItem != null)
            //    player.AnimatorHandler.PlayTargetAnimation(currentWeaponItem.unequipAnimationName, true, false, false, false);

            // Play the weapon animation of the new item
            player.PlayerAnimator.PlayTargetAnimation(weaponItem.equipAnimationName, true, false, false, false);

            player.PlayerAnimator.Animator.SetInteger("WeaponId", weaponItem.weaponId);
        }
    }
}
