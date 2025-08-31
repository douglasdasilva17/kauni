using Supertactic.Player;
using UnityEngine;

namespace Supertactic.UI
{
    public class InteractObjectActivatorUI : MonoBehaviour
    {
        [SerializeField]
        float interactDistance = 5f;
        [SerializeField]
        float inputDistance = 2f;
        [SerializeField]
        GameObject interactObject;
        [SerializeField]
        GameObject inputObject;

        private PlayerManager _playerManager;

        private void Start()
        {
            _playerManager = FindObjectOfType<PlayerManager>();
        }

        void Update()
        {
            if (_playerManager == null)
            {
                return;
            }

            float distance = Vector3.Distance(transform.position, _playerManager.transform.position);
            if (distance <= inputDistance)
            {
                interactObject.SetActive(false);
                inputObject.SetActive(true);
            }
            else if (distance <= interactDistance)
            {
                interactObject.SetActive(true);
                inputObject.SetActive(false);
            }
            else
            {
                interactObject.SetActive(false);
                inputObject.SetActive(false);
            }
        }
    }
}
