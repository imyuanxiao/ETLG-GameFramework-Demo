using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;

namespace ETLG
{
    public enum BossEnemyType { AI, IoT, CloudComputing, Blockchain, DataScience, Cybersecurity}
    public class BossEnemyController : MonoBehaviour
    {
        // public BossEnemyType bossEnemyType;
        public EnumEntity bossEnemyType;

        private void OnEnable() 
        {
            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Subscribe(BattleWinEventArgs.EventId, OnBattleWin);
        }

        private void OnBattleWin(object sender, GameEventArgs e)
        {
            Debug.Log("BossEnemyController: Battle Win");
            GetComponent<BossEnemyAttack>().StopAllCoroutines();
        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            Debug.Log("BossEnemyController: Player dead");
            GetComponent<BossEnemyAttack>().StopAllCoroutines();
        }

        private void OnDisable() 
        {
            GameEntry.Event.Unsubscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Unsubscribe(BattleWinEventArgs.EventId, OnBattleWin);
        }
    }
}
