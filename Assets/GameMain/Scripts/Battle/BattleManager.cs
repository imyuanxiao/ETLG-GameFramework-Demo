using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class BattleManager : Singleton<BattleManager>
    {
        [SerializeField] private GameObject basicEnemyPrefab;
        [SerializeField] private Transform[] basicEnemySpawnPositions;

        public float enemySpawnRate;

        private void SpawnBasicEnemy()
        {
            int spawnPosIdx = Random.Range(0, basicEnemySpawnPositions.Length);
            ObjectPoolManager.SpawnObject(basicEnemyPrefab, basicEnemySpawnPositions[spawnPosIdx].position, 
                                                            basicEnemySpawnPositions[spawnPosIdx].rotation, 
                                                            ObjectPoolManager.PoolType.GameObject);
        }

        public void SpawnBasicEnemies()
        {
            StartCoroutine(SpawnBasicEnemiesRoutine());
        }

        public void StopSpawnBasicEnemies()
        {
            StopCoroutine(SpawnBasicEnemiesRoutine());
        }

        private IEnumerator SpawnBasicEnemiesRoutine()
        {
            float elapsedTime = 0f;

            while (true)
            {
                if (elapsedTime < enemySpawnRate)
                {
                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    SpawnBasicEnemy();
                    elapsedTime = 0f;
                }
                yield return null;
            }
        }
    }
}
