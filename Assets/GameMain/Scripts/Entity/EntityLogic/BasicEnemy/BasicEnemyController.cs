using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;

namespace ETLG
{
    public enum BasicEnemyType {BasicEnemy1, BasicEnemy2, BasicEnemy3, AI} 
    public class BasicEnemyController : MonoBehaviour
    {
        public BasicEnemyType basicEnemyType;

        private void OnEnable() 
        {
            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Subscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Subscribe(BattleWinEventArgs.EventId, OnBattleWin);
            if (GetComponent<BasicEnemyAttack>() != null)
            {
                GetComponent<BasicEnemyAttack>().enabled = true;
            }
        }

        private void OnBattleWin(object sender, GameEventArgs e)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void OnGamePause(object sender, GameEventArgs e)
        {
            // GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void OnDisable() 
        {
            GameEntry.Event.Unsubscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Unsubscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Unsubscribe(BattleWinEventArgs.EventId, OnBattleWin);
        }

        private void Update() 
        {
            // Check if basic enemy has reached the bottom boundary
            if (transform.position.z <= BattleManager.Instance.basicEnemyPassPosition.position.z)
            {
                BattleManager.Instance.basicEnemyPassed++;
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}
