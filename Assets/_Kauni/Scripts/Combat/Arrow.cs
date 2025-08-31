using UnityEngine;

namespace Supertactic.Combat
{
    public class Arrow : MonoBehaviour
    {
        public float Speed = 20.0f;

        Transform myTransform;
        HitData hitData;
        bool arrowSticked = false;

        private void Awake()
        {
            myTransform = transform;
        }

        private void Update()
        {
            HandleArrowMovement();
        }

        private void HandleArrowMovement()
        {
            float step = Speed * Time.deltaTime;

            if (myTransform.position != hitData.point)
                myTransform.position = Vector3.MoveTowards(myTransform.position, hitData.point, step);
            else
            {
                // Target reached
                gameObject.SetActive(false);

                if (arrowSticked)
                {
                    return;
                }

                arrowSticked = true;

                GetComponent<HitEffects>().PerformHitEffect(hitData);
            }
        }

        public void SetTargetPosition(HitData hitData)
        {
            this.hitData = hitData;
            transform.LookAt(hitData.point);
        }
    }
}