using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ETLG.Data;

namespace ETLG
{
    public class BasicEnemyAttack : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;

        [SerializeField] private Transform bulletSpawnPosition;
        private BasicEnemyController controller;
        private float fireRate;

        private void Awake() 
        {
            controller = GetComponent<BasicEnemyController>();
        }

        private void OnEnable() 
        {
            // fireRate = 1.5f;
            InitBasicEnemy();
            StartCoroutine(Fire());
        }

        private void InitBasicEnemy()
        {
            if (controller.basicEnemyType == BasicEnemyType.BasicEnemy1)
            {
                fireRate = 1.5f;
            }
            else if (controller.basicEnemyType == BasicEnemyType.BasicEnemy2)
            {
                fireRate = 2f;
            }
            else 
            {
                Log.Error("No Basic Enemy of type [" + controller.basicEnemyType.ToString() + "]");
            }
        }

        private IEnumerator Fire()
        {
            float elapsedTime = fireRate;

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
