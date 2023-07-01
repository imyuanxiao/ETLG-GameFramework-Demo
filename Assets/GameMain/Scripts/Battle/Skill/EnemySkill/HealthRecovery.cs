using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class HealthRecovery : MonoBehaviour
    {
        public int recoverAmount;
        public float recoverRate;
        private int maxHealth;
        private int currentHealth;
        private BossEnemyHealth health;

        private void Awake() 
        {
            health = GetComponent<BossEnemyHealth>();
        }

        private void OnEnable() 
        {
            // StartCoroutine(RecoverHealth());
            maxHealth = health.MaxHealth;
            currentHealth = health.CurrentHealth;
            StartCoroutine(RecoverHealth());
        }

        private IEnumerator RecoverHealth()
        {
            float timeElasped = 0;
            while (true) 
            {
                maxHealth = health.MaxHealth;
                currentHealth = health.CurrentHealth;
                if (currentHealth < maxHealth && currentHealth > 0)
                {
                    if (timeElasped < recoverRate)
                    {
                        timeElasped += Time.deltaTime;
                        yield return null;
                    }
                    else 
                    {
                        currentHealth = Mathf.Min(maxHealth, currentHealth + recoverAmount);
                        health.CurrentHealth = currentHealth;
                        timeElasped = 0;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void OnDisable() 
        {
            StopAllCoroutines();
        }
    }
}
