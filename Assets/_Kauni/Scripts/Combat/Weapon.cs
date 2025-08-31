using UnityEngine;

namespace Supertactic
{
    public class Weapon : MonoBehaviour
    {
        public GameObject weaponDamage;

        public void EnableCollider()
        {          
            weaponDamage.SetActive(true);
        }

        public void DisableCollider()
        {
            weaponDamage.SetActive(false); 
        }
    }
}
