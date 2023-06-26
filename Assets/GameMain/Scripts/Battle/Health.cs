using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public abstract class Health : MonoBehaviour, IDamageable
    {
        public int MaxHealth;
        protected int CurrentHealth;

        public int MaxShield;
        protected int CurrentShield;

        public bool IsDead()
        {
            return CurrentHealth <= 0;
        }

        public abstract void TakeDamage(int damage);

        private void OnTriggerEnter(Collider other) 
        {
            // if (other.gameObject.CompareTag(targetBulletType))
            // {
                TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
                ObjectPoolManager.ReturnObjectToPool(other.gameObject);
            // }
        }

        protected abstract void OnDead();

        private void Update() 
        {
            if (IsDead())
            {
                OnDead();
            }
        }
    }
}
