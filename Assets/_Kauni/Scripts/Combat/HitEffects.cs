using UnityEngine;

namespace Supertactic.Combat
{
    public class HitEffects : MonoBehaviour
    {
        public GameObject fleshParticle;
        public AudioClip[] fleshSound;

        public GameObject metalParticle;
        public AudioClip[] metalSound;

        public GameObject stoneParticle;
        public AudioClip[] stoneSound;

        public GameObject woodParticle;
        public AudioClip[] woodSound;

        public GameObject waterParticle;
        public AudioClip[] waterSound;

        public float delayTime = 2;

        public void PerformHitEffect(HitData hitData)
        {
            switch (hitData.hitTag)
            {
                case "Water":
                    Water(hitData);
                    break;
            }
        }

        private void Water(HitData hitData)
        {
            GameObject hitWater = Instantiate(waterParticle, hitData.point, Quaternion.FromToRotation(Vector3.up, hitData.normal));
            AudioManager.Instance.PlaySoundFX(waterSound[Random.Range(0, waterSound.Length)], transform, 1f, 1f);
            Destroy(hitWater, delayTime);
        }

        void Flesh()
        {
            GameObject hitflesh = Instantiate(fleshParticle, transform.position, Quaternion.identity);
            hitflesh.GetComponent<ParticleSystem>().Play();
            AudioManager.Instance.PlaySoundFX(fleshSound[Random.Range(0, fleshSound.Length)], transform, 1f, 1f);
            Destroy(hitflesh, delayTime);
        }

        void Metal()
        {
            GameObject hitmetal = Instantiate(metalParticle, transform.position, Quaternion.identity);
            hitmetal.GetComponent<ParticleSystem>().Play();
            AudioManager.Instance.PlaySoundFX(metalSound[Random.Range(0, metalSound.Length)], transform, 1f, 1f);
            Destroy(hitmetal, delayTime);
        }

        void Stone()
        {
            GameObject hitstone = Instantiate(stoneParticle, transform.position, Quaternion.identity);
            hitstone.GetComponent<ParticleSystem>().Play();
            AudioManager.Instance.PlaySoundFX(stoneSound[Random.Range(0, stoneSound.Length)], transform, 1f, 1f);
            Destroy(hitstone, delayTime);
        }

        void Wood()
        {
            GameObject hitwood = Instantiate(woodParticle, transform.position, Quaternion.identity);
            hitwood.GetComponent<ParticleSystem>().Play();
            AudioManager.Instance.PlaySoundFX(stoneSound[Random.Range(0, stoneSound.Length)], transform, 1f, 1f);
            Destroy(hitwood, delayTime);
        }
    }
}