using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class BattleManager : Singleton<BattleManager>
    {
        [SerializeField] private GameObject[] basicEnemyPrefab;
        [SerializeField] private Transform[] basicEnemySpawnPositions;
        public float enemySpawnRate;

        private IEnumerator spawnBasicEnemiesCoroutine;

        protected override void Awake() 
        {
            base.Awake();
            spawnBasicEnemiesCoroutine = SpawnBasicEnemiesRoutine();
        }

        private void SpawnBasicEnemy()
        {
            int spawnPosIdx = Random.Range(0, basicEnemySpawnPositions.Length);
            int spawnEnemyIdx = Random.Range(0, basicEnemyPrefab.Length);
            ObjectPoolManager.SpawnObject(basicEnemyPrefab[spawnEnemyIdx], basicEnemySpawnPositions[spawnPosIdx].position, 
                                                            basicEnemySpawnPositions[spawnPosIdx].rotation, 
                                                            ObjectPoolManager.PoolType.GameObject);
        }

        public void SpawnBasicEnemies()
        {
            StartCoroutine(spawnBasicEnemiesCoroutine);
        }

        public void StopSpawnBasicEnemies()
        {
            StopCoroutine(spawnBasicEnemiesCoroutine);
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
