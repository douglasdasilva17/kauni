using Supertactic.GameEvents;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Supertactic
{
    public enum EDamageType
    {
        Normal,
        Fire,
    }

    public class Damageable : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        private int _health;

        public UnityEvent OnDeath;
        public UnityEvent TakeDamage;
        public Action<EDamageType> OnDamageComplete;
        bool _isDead = false;

        private void Start()
        {
            _health = maxHealth;
        }

        public void Hit(int damage, EDamageType damageType)
        {
            if (_health == 0)
            {
                if (!_isDead)
                {
                    GameEventsManager.instance.enemyEvents.EnemyDefeated();
                 
                    OnDeath?.Invoke();
                    
                    _isDead = true;
                }
                return;
            }

            _health = Mathf.Max(_health - damage, 0);

            TakeDamage?.Invoke();
            OnDamageComplete?.Invoke(damageType);
        }
    }
}