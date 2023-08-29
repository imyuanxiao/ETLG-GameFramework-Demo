using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace ETLG
{
    public class Respawn : MonoBehaviour
    {
        private BossEnemyHealth health;
        public int respawnTime = 1;
        private int respawnCnt = 0;

        private void Awake() 
        {
            health = GetComponent<BossEnemyHealth>();    
        }

        private void OnEnable() 
        {
            respawnCnt = 0;
        }


        private void Update() 
        {
            if (health.CurrentHealth <= 0 && respawnCnt < respawnTime)
            {
                respawnCnt++;
                GameEntry.Event.Fire(this, EnemyRespawnEventArgs.Create(health));
            }
            else if (health.CurrentHealth <= 0 && respawnCnt >= respawnTime)
            {
                GameEntry.Event.Fire(this, BattleWinEventArgs.Create(GetComponent<BossEnemyController>().bossEnemyType));
            }
        }
    }
}
