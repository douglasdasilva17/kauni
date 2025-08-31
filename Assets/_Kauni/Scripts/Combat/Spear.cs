using UnityEngine;

namespace Supertactic.Combat
{
    public class Spear : MonoBehaviour
    {
        public int damage = 5;
        public Collider triggerBox;
        public GameObject hitParticle;

        private void Start()
        {
            triggerBox = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.gameObject.GetComponent<Damageable>();

            if (enemy != null)
            {
                enemy.Hit(damage, EDamageType.Normal);
                Instantiate(hitParticle, other.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            }
        }

        public void EnableTriggerBox()
        {
            triggerBox.enabled = true;
        }

        public void DisableTriggerBox()
        {
            triggerBox.enabled = false;
        }
    }
}
