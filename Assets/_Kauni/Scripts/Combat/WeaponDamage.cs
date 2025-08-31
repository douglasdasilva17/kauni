using System.Collections.Generic;
using UnityEngine;

namespace Supertactic
{
    public class WeaponDamage : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        [SerializeField] private EDamageType damageType;
        [SerializeField] private int damage;

        private float knockback;
        private List<Collider> alreadyCollidedWith = new List<Collider>();

        private void OnEnable()
        {
            alreadyCollidedWith.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            // Prevent the script from detecting the actor's own collider
            if (other == myCollider) { return; }

            if (alreadyCollidedWith.Contains(other)) { return; }

            alreadyCollidedWith.Add(other);

            // Check if the damageable script exist in this object
            if (other.TryGetComponent<Damageable>(out Damageable damageable))
            {
                damageable.Hit(damage, damageType);
            }

            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
                forceReceiver.AddForce(direction * knockback);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            alreadyCollidedWith.Clear();
        }

        public void SetAttack(int damage, float knockback)
        {
            this.damage = damage;
            this.knockback = knockback;
        }
    }
}
