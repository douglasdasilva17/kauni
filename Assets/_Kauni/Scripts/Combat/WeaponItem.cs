using UnityEngine;

namespace Supertactic
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]    
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;
        //public AnimatorController controller;
        public int weaponId;
    }
}
