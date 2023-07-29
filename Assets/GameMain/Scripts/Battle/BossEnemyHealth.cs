using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;

namespace ETLG
{
    public class BossEnemyHealth : Health
    {
        private BossEnemyController controller;
        private int difficulty;

        private void Awake() 
        {
            controller = GetComponent<BossEnemyController>();
        }

        private void OnEnable() 
        {
            this.difficulty = SaveManager.Instance.difficulty;
            InitBossEnemy();
            StartCoroutine(CheckDeath()); 
            Debug.Log("Difficulty: " + this.difficulty);
        }

        private void InitBossEnemy()
        {
            MaxHealth = (int) GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) controller.bossEnemyType).Durability + difficulty * 5;
            MaxShield = (int) GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) controller.bossEnemyType).Shields + difficulty * 5;
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
        }

        protected override void OnDead()
        {
            if (controller.bossEnemyType == EnumEntity.FinalBoss)
            {
                GetComponent<FinalBossSkill>().stage = 2;
            }

            if (GetComponent<Respawn>() == null || !GetComponent<Respawn>().isActiveAndEnabled)
            {
                ObjectPoolManager.SpawnObject(BattleManager.Instance.explodeFXPrefab, transform.position, transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);
                GameEntry.Event.Fire(this, BattleWinEventArgs.Create(controller.bossEnemyType));
            }
        }

        private void OnDisable() 
        {
            StopAllCoroutines();    
        }
    }
}
