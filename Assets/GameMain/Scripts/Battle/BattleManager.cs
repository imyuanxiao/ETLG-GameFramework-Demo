using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class BattleManager : Singleton<BattleManager>
    {
        [Header("Battle Boundary")]
        public Transform leftBoundary;
        public Transform rightBoundary;
        public Transform upBoundary;
        public Transform bottomBoundary;
        
        [Header("Basic Battle Settings")]
        [SerializeField] private GameObject[] basicEnemyPrefab;
        [SerializeField] private Transform[] basicEnemySpawnPositions;
        [SerializeField] public Transform basicEnemyPassPosition;
        public float enemySpawnRate;
        public int basicEnemyNum;
        [HideInInspector] public int basicEnemyCnt = 0;
        [HideInInspector] public int basicEnemyKilled = 0;
        [HideInInspector] public int basicEnemyPassed = 0;

        [HideInInspector] public string bossType;

        private IEnumerator spawnBasicEnemiesCoroutine;

        protected override void Awake() 
        {
            base.Awake();
            spawnBasicEnemiesCoroutine = SpawnBasicEnemiesRoutine();
        }


        private void SpawnBasicEnemy()
        {
            if (basicEnemyCnt >= basicEnemyNum)
            {
                StopSpawnBasicEnemies();
                return;
            }

            int spawnPosIdx = Random.Range(0, basicEnemySpawnPositions.Length);
            int spawnEnemyIdx = Random.Range(0, basicEnemyPrefab.Length);
            ObjectPoolManager.SpawnObject(basicEnemyPrefab[spawnEnemyIdx], basicEnemySpawnPositions[spawnPosIdx].position, 
                                                            basicEnemySpawnPositions[spawnPosIdx].rotation, 
                                                            ObjectPoolManager.PoolType.GameObject);
            basicEnemyCnt++;
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
