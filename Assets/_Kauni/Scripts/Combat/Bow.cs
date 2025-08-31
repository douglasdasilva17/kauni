using UnityEngine;

namespace Supertactic.Combat
{
    public class Bow : MonoBehaviour
    {
        [Header("Arrow Properties")]
        public int arrowCount = 12;
        public float shootForce = 40f;
        public float range = 1000f;

        [Header("Bow Properties")]
        public GameObject arrowPrefab;
        public Transform bowSpawnPoint;

        [Header("Bow Equip & Unequip")]
        public Transform bowEquipTransform;
        public Transform bowUnequipTransform;
        public Transform bowEquipParent;
        public Transform bowUnequipParent;

        Transform playerTransform;

        [Header("Bow Sounds")]
        [SerializeField]
        AudioClip[] pullStringClip;
        [SerializeField]
        AudioClip[] fireArrowClip;

        [Header("Flags")]
        public bool canUseBow = false;

        Transform myTransform;

        void Start()
        {
            myTransform = transform;
            playerTransform = transform.root.GetChild(1);
        }

        void Update()
        {
            canUseBow = arrowCount > 0;
        }

        public void ShootArrow()
        {
            HitData hitData = GetHitData();
            bowSpawnPoint.LookAt(hitData.point);

            GameObject currentArrow = Instantiate(arrowPrefab, bowSpawnPoint.position, bowSpawnPoint.rotation);
            currentArrow.GetComponent<Arrow>().SetTargetPosition(hitData);

            AudioManager.Instance.PlaySoundFX(fireArrowClip, transform, UnityEngine.Random.Range(0.3f, 0.5f), 1f);

            arrowCount--;
        }

        HitData GetHitData()
        {
            RaycastHit hit;
            HitData hitData = new HitData();
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

            Ray ray = Camera.main.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.collider)
                {
                    hitData.point = hit.point;
                    hitData.normal = hit.normal;
                    hitData.hitTag = hit.collider.tag;
                }
            }

            return hitData;
        }

        public void PrepareArrow()
        {
            AudioManager.Instance.PlaySoundFX(pullStringClip, transform, UnityEngine.Random.Range(0.3f, 0.5f), 1f);
        }

        public void EquipBow()
        {
            myTransform.position = bowEquipTransform.position;
            myTransform.rotation = bowEquipTransform.rotation;
            myTransform.parent = bowEquipParent;
        }

        public void UnequipBow()
        {
            myTransform.position = bowUnequipTransform.position;
            myTransform.rotation = bowUnequipTransform.rotation;
            myTransform.parent = bowUnequipParent;
        }
    }
}