using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;

namespace ETLG
{
    public class BasicEnemyAttack : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;

        [SerializeField] private Transform bulletSpawnPosition;

        private float fireRate;

        private void OnEnable() 
        {
            fireRate = 1.5f;
            StartCoroutine(Fire());
        }

        private IEnumerator Fire()
        {
            float elapsedTime = 0f;

            while (true)
            {
                if (elapsedTime < fireRate)
                {
                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    ObjectPoolManager.SpawnObject(bulletPrefab, bulletSpawnPosition.position, 
                                                                bulletSpawnPosition.rotation, 
                                                                ObjectPoolManager.PoolType.GameObject);
                    elapsedTime = 0f;
                }
                yield return null;
            }
        }

        private void OnDisable() 
        {
            StopAllCoroutines();
        }
    }
}
