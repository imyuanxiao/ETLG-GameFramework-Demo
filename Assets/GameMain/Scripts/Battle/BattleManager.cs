using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using ETLG.Data;

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
        [HideInInspector] public Entity bossEnemyEntity;
        [HideInInspector] public int basicEnemyAttackBase;
        [HideInInspector] public int basicEnemyHealthBase;

        [Header("Enemy AI Spaceships")]
        public Transform[] AISpaceshipSpawnPoints;

        private IEnumerator spawnBasicEnemiesCoroutine;

        [Header("Player AI Spaceships")]
        public Transform[] PlayerAIShipSpawnPoints;

        [Header("Instant Move Settings")]
        public Transform[] instantMovePoints;

        [Header("FX")]
        public GameObject explodeFXPrefab;
        public GameObject hitFXPrefab;
        public GameObject muzzleFlashPrefab;
        public GameObject missileFlarePrefab;
        public GameObject cloudComputingSkillFX;

        protected override void Awake() 
        {
            base.Awake();
            spawnBasicEnemiesCoroutine = SpawnBasicEnemiesRoutine();
            basicEnemyAttackBase = (int) ((int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Durability * 0.1);
            basicEnemyHealthBase = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower;
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
