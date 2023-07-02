using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;

namespace ETLG
{
    public class BossEnemyHealth : Health
    {
        private BossEnemyController controller;

        private void Awake() 
        {
            controller = GetComponent<BossEnemyController>();
        }

        private void OnEnable() 
        {
            InitBossEnemy();
            StartCoroutine(CheckDeath()); 
            // Debug.Log("Boss Enemy Max Health: " + MaxHealth + " | Max Shield: " + MaxShield);
        }

        private void InitBossEnemy()
        {
            MaxHealth = (int) GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) controller.bossEnemyType).Durability;
            MaxShield = (int) GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) controller.bossEnemyType).Shields;
            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;
        }

        public override void TakeDamage(int damage)
        {
            if (CurrentShield > 0)
            {
                CurrentShield = Mathf.Max(0, CurrentShield - damage);
            }
            else
            {
                CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
            }
            // Debug.Log("Enemy CurrentShield: " + CurrentShield + " | " + "CurrentHealth: " + CurrentHealth);

        }

        protected override void OnDead()
        {
            GameEntry.Event.Fire(this, BattleWinEventArgs.Create(controller.bossEnemyType));
        }

        private void OnDisable() 
        {
            StopAllCoroutines();    
        }
    }
}
