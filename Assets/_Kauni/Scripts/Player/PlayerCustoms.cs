using UnityEngine;

namespace Supertactic.Player
{
    public class PlayerCustoms : MonoBehaviour
    {
        [SerializeField] Transform hairParent;
        [SerializeField] GameObject hairPrefab;

        private void Start()
        {
            Instantiate(hairPrefab, hairParent);
        }
    }
}
