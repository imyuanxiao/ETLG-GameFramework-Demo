using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class AIAssist : MonoBehaviour
    {
        public GameObject AISpaceshipPrefab;
        public int AISpaceshipNum = 3;
        public int AISpaceshipHealth = 200;
        public int AISpaceshipAttack = 10;
        public float coolDown = 5f;
        private BossEnemyHealth health;
        private float timeElapsed = 0;
        private bool isReady;
        private bool isActive;
        private int AISpaceshipLeft;

        private void Awake() 
        {
            health = GetComponent<BossEnemyHealth>();
        }

        private void OnEnable() 
        {
            isReady = true;
            AISpaceshipLeft = AISpaceshipNum;
        }

        private void Update() 
        {
            if (health.CurrentHealth <= health.MaxHealth * 0.5 && isReady)
            {
                for (int i=0; i < AISpaceshipNum; i++)
                {
                    GameObject ai = ObjectPoolManager.SpawnObject(AISpaceshipPrefab, 
                            BattleManager.Instance.AISpaceshipSpawnPoints[i].position, transform.rotation, ObjectPoolManager.PoolType.GameObject);
                }
                isActive = true;
                isReady = false;
            }
            
            if (!isActive && !isReady) 
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed > coolDown)
                {
                    isReady = true;
                    timeElapsed = 0;
                    AISpaceshipLeft = AISpaceshipNum;
                }
            }
        } 
    }
}
