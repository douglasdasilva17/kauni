using UnityEngine;
using Supertactic.Input;

namespace Supertactic.Animal
{
    public class FAnimalAttack : MonoBehaviour
    {
        [SerializeField] private InputReader _controller;
        [SerializeField] private float _fireRate = 0.4f;
        [SerializeField] private Animator _animator;

        private float fireTime;
        private bool _isFired;

        private void Start()
        {
            _controller.EnablePlayerActions();
            _controller.Fire += HandleOnFire;
        }

        private void OnDisable()
        {
            _controller.Fire -= HandleOnFire;
        }

        private void HandleOnFire(bool isFiring)
        {
            if (_isFired)
                return;

            if (isFiring)
            {
                _isFired = true;
                _animator.CrossFade("Bite", 0.2f);
            }
        }

        private void Update()
        {
            if (_isFired)
            {
                fireTime += Time.deltaTime;
                if (fireTime > _fireRate)
                {
                    _isFired = false;
                    _animator.Play("Idle");
                    fireTime = 0;
                }
            }
        }
    }
}
