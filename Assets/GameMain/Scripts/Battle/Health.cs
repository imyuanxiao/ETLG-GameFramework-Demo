using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;

namespace ETLG
{
    public abstract class Health : MonoBehaviour, IDamageable
    {
        [HideInInspector] public int MaxHealth;
        [HideInInspector] public int CurrentHealth;

        [HideInInspector] public int MaxShield;
        [HideInInspector] public int CurrentShield;

        public bool IsDead()
        {
            return CurrentHealth <= 0;
        }

        public abstract void TakeDamage(int damage);

        private void OnTriggerEnter(Collider other) 
        {
            GameObject bulletPrefab = other.gameObject;
            TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
            ObjectPoolManager.SpawnObject(BattleManager.Instance.hitFXPrefab, bulletPrefab.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystem);
            ObjectPoolManager.ReturnObjectToPool(bulletPrefab); 
        }

        protected abstract void OnDead();

        public IEnumerator CheckDeath()
        {
            while (true) 
            {
                if (!IsDead())
                {
                    yield return null;
                }
                else 
                {
                    OnDead();
                    yield break;
                }
            }
        }
    }
}
