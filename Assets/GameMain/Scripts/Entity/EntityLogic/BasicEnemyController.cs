using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;

namespace ETLG
{
    public enum BasicEnemyType {BasicEnemy1, BasicEnemy2, BasicEnemy3} 
    public class BasicEnemyController : MonoBehaviour //, IBattleLostObserver
    {
        public BasicEnemyType basicEnemyType;

        private void OnEnable() 
        {
            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void OnDisable() 
        {
            GameEntry.Event.Unsubscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
        }
    }
}
