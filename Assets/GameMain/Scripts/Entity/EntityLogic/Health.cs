using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class Health : MonoBehaviour, IDamageable
    {
        public int MaxHealth;
        private int CurrentHealth;

        public int MaxShield;
        private int CurrentShield;

        private void OnEnable() 
        {
            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;
        }

        public bool IsDead()
        {
            return CurrentHealth <= 0;
        }

        public void TakeDamage(int damage)
        {
            if (CurrentShield > 0)
            {
                CurrentShield = Mathf.Max(0, CurrentShield - damage);
            }
            else
            {
                CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
            }
            Debug.Log("Shield: " + CurrentShield);
            Debug.Log("CurrentHealth: " + CurrentHealth);
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.CompareTag("PlayerBullet"))
            {
                Debug.Log("Enemy's being hit");
                TakeDamage(other.gameObject.GetComponent<Bullet>().damage);
                ObjectPoolManager.ReturnObjectToPool(other.gameObject);
            }
        }

        private void Update() 
        {
            if (IsDead())
            {
                CurrentHealth = MaxHealth;
                CurrentShield = MaxShield;
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}
