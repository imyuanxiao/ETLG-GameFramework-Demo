using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
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
            GameEntry.Event.Subscribe(AISpaceshipDestroyedEventArgs.EventId, OnAISpaceshipDestoryed);

            isReady = true;
            AISpaceshipLeft = AISpaceshipNum;
        }

        private void OnAISpaceshipDestoryed(object sender, GameEventArgs e)
        {
            AISpaceshipDestroyedEventArgs ne = (AISpaceshipDestroyedEventArgs) e;
            if (ne == null)
                return;
            AISpaceshipLeft--;
        }

        private void Update() 
        {
            // if enemy health is below 50% and skill is ready
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
            // if all AI spaceship were destroyed by player
            if (AISpaceshipLeft <= 0 && isActive)
            {
                isActive = false;
            }
            // if enemy health is below 50% but no AI spaceship in scene (being destoryed)
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

        private void OnDisable() 
        {
            GameEntry.Event.Unsubscribe(AISpaceshipDestroyedEventArgs.EventId, OnAISpaceshipDestoryed);    
        }
    }
}
