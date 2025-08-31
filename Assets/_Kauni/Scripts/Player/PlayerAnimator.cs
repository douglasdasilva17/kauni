using Supertactic.Cameras;
using UnityEngine;

namespace Supertactic.Player
{
    [DisallowMultipleComponent]
    public class PlayerAnimator : MonoBehaviour
    {
        private Transform _bowStringPosition;
        private Transform _notPullStringPosition;
        private Transform _pullStringPosition;

        readonly int verticalHash = Animator.StringToHash("Vertical");
        readonly int horizontalHash = Animator.StringToHash("Horizontal");

        private PlayerManager _playerManager;
        private Animator _animator;

        public Animator Animator => _animator;

        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            _animator = GetComponent<Animator>();
            _bowStringPosition = transform.Find("WB.string");
            _notPullStringPosition = transform.Find("NotPullStringTargetPosition");
            _pullStringPosition = transform.Find("PullStringTargetPosition");
        }

        /// <summary>
        /// Set the bool Aim to true and change camera zoom if the player is holding a weapon 
        /// </summary>
        /// <param name="state"></param>
        public void PlayAimAnimation(bool state)
        {
            _animator.SetBool("Aim", state);

            //TODO: Check if the player is holding a weapon
            CameraManager.instance.HandleAimZoom(state);
        }

        public void HandleRootMotion(bool applyRootMotion, bool isPerformingAction)
        {
            _animator.applyRootMotion = applyRootMotion;
            _playerManager.isPerformingAction = isPerformingAction;
        }

        public void UpdateAnimatorValues(float horizontal, float vertical, bool run)
        {
            #region Vertical
            float v = 0;
            // Clamping the vertical movement for better result
            if (vertical > 0 && vertical < 0.55f)
                v = 0.5f;
            else if (vertical > 0.1f)
                v = 1;
            else if (vertical < 0 && vertical > -0.55f)
                v = 0;
            else if (vertical < -0.1f)
                v = -1f;
            else
                v = 0;
            #endregion

            #region Horizontal
            float h = 0;
            if (horizontal > 0 && horizontal < 0.55f)
                h = 0.5f;
            else if (horizontal > 0.1f)
                h = 1f;
            else if (horizontal < 0 && horizontal > -0.55f)
                h = 0f;
            else if (horizontal < -0.1f)
                h = -1f;
            else
                h = 0;
            #endregion

            _animator.SetFloat(horizontalHash, h, 0.2f, Time.deltaTime);
            _animator.SetFloat(verticalHash, v, 0.2f, Time.deltaTime);
            _animator.SetBool("Run", run);
        }

        // Play a target animation using rootmotion or not.
        public void PlayTargetAnimation(string targetAnimation, bool isPerformingAction, bool canRotate = false, bool canMove = false)
        {
            _animator.CrossFade(targetAnimation, 0.2f);
            //animator.SetBool("isPerformingAction", isPerformingAction);
            _playerManager.isPerformingAction = isPerformingAction;
            _playerManager.PlayerLocomotion.SetControl(canMove, canRotate);
        }

        // Play a target animation using rootmotion or not.
        public void PlayTargetAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion, bool canRotate = false, bool canMove = false)
        {
            // HandleRootMotion(applyRootMotion, isPerformingAction);

            _animator.CrossFade(targetAnimation, 0.2f);

            _playerManager.isPerformingAction = isPerformingAction;

            _playerManager.PlayerLocomotion.SetControl(canMove, canRotate);
        }

        //private void OnAnimatorMove()
        //{
        //    if(Player.applyRootMotion)
        //    {
        //        Vector3 velocity = Player.animatorHandler.Animator.deltaPosition;
        //        Player.locomotion.MoveOnPlane(velocity);
        //        Player.transform.rotation *= Player.animatorHandler.Animator.deltaRotation;
        //    }
        //}

        #region Animation Events Arrow

        private void StringPullEvent()
        {
            if (_notPullStringPosition == null) return;
            if (_bowStringPosition == null) return;

            _bowStringPosition.position = _pullStringPosition.position;
            _bowStringPosition.SetParent(_pullStringPosition);
        }

        private void StringNotPullEvent()
        {
            ResetPullString();
        }

        public void ResetPullString()
        {
            if (_notPullStringPosition == null) return;
            if (_bowStringPosition == null) return;

            _bowStringPosition.position = _notPullStringPosition.position;
            _bowStringPosition.SetParent(_notPullStringPosition);
        }

        #endregion

        #region Animation Events Bow
        private void BowShootEvent()
        {
            if (_playerManager.PlayerArchery.bowWeapon.canUseBow)
            {
                _playerManager.PlayerArchery.bowWeapon.ShootArrow();
                _playerManager.PlayerArchery.arrowVisualization.SetActive(false);
            }
        }

        private void BowDrawArrowEvent()
        {
            if (_playerManager.PlayerArchery.bowWeapon.canUseBow)
                _playerManager.PlayerArchery.arrowVisualization.SetActive(true);
        }

        private void BowPullStringEvent()
        {
            if (_playerManager.PlayerArchery.bowWeapon.canUseBow)
                _playerManager.PlayerArchery.bowWeapon.PrepareArrow();
        }


        #endregion
    }
}