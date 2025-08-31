using Supertactic.Player;
using Supertactic.Input;
using UnityEngine;

namespace Supertactic.Combat
{
    public class PlayerCombat : MonoBehaviour
    {
        public PlayerLocomotion PlayerLocomotion { get; private set; }
        public PlayerAnimator AnimatorHandler { get; private set; }

        [Header("Weapons")]
        public GameObject arrowVisualization; //Gameobject used to complement bow mechanic

        [HideInInspector] public Bow bowWeapon;
        [HideInInspector] public Spear spearWeapon;

        [SerializeField] InputReader input;

        void Start()
        {
            input.EnablePlayerActions();

            PlayerLocomotion = GetComponent<PlayerLocomotion>();
            AnimatorHandler = GetComponent<PlayerAnimator>();
            arrowVisualization.SetActive(false);
        }

        void Update()
        {
            HandleBowAimShootInput();
            HandleSpearInput();
        }

        private void HandleSpearInput()
        {
            if (input.AimInput && !input.RunInput)
            {
                // Performing the bow aim animation
                AnimatorHandler.PlayAimAnimation(true);

                // Adjust player rotation to the camera rotation
                PlayerLocomotion.RotatePlayer();

                if (spearWeapon != null)
                {
                    if (input.FireInput)
                    {
                        AnimatorHandler.Animator.SetBool("Fire2", true);
                    }
                    else
                    {
                        AnimatorHandler.Animator.SetBool("Fire2", false);
                    }
                }
            }
        }

        // TODO: CREATE A METHOD TO SWITCH BETWEEN WEAPONS AND SETUP THE PLAYER BOOLEAN
        // ACCORDING WITH THE WEAPON ACTIVE.
        #region Bow
        public void HandleBowAimShootInput()
        {
            // TODO: CREATE A VARIABLE THAT REPRESENTS THE CURRENT AND ACTIVE WEAPON TO CALL THIS
            if (input.AimInput && !input.RunInput)
            {
                // Performing the bow aim animation
                AnimatorHandler.PlayAimAnimation(true);

                // Adjust player rotation to the camera rotation
                PlayerLocomotion.RotatePlayer();

                if (input.FireInput && bowWeapon != null)
                {
                    AnimatorHandler.Animator.SetTrigger("Fire");
                }
            }
            else
            {
                AnimatorHandler.PlayAimAnimation(false);
                AnimatorHandler.ResetPullString();
                arrowVisualization.SetActive(false);
            }
        }

        #endregion
    }
}
