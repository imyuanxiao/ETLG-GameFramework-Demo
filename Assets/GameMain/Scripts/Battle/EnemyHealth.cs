using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using UnityEngine;
using ETLG.Data;
using GameFramework.Event;
using System;

namespace ETLG
{
    public class EnemyHealth : Health
    {
        private BasicEnemyController controller;

        private void Awake() 
        {
            controller = GetComponent<BasicEnemyController>();
        }

        private void OnEnable() 
        {
            InitBasicBattleEnemy();

            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;

            StartCoroutine(CheckDeath());    
        }


        private void InitBasicBattleEnemy()
        {
            if (controller.basicEnemyType == BasicEnemyType.BasicEnemy1)
            {
                // MaxHealth = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower * 2;
                // MaxHealth = ((ProcedureBasicBattle) GameEntry.Procedure.GetProcedure<ProcedureBasicBattle>()).basicEnemyHealthBase * 2;
                MaxHealth = BattleManager.Instance.basicEnemyHealthBase * 2;
                MaxShield = 0;
            }
            else if (controller.basicEnemyType == BasicEnemyType.BasicEnemy2)
            {
                // MaxHealth = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower * 3;
                // MaxHealth = ((ProcedureBasicBattle) GameEntry.Procedure.GetProcedure<ProcedureBasicBattle>()).basicEnemyHealthBase * 3;
                MaxHealth = BattleManager.Instance.basicEnemyHealthBase * 3;
                MaxShield = 0;
            }
            else if (controller.basicEnemyType == BasicEnemyType.AI)
            {
                MaxHealth = BattleManager.Instance.basicEnemyHealthBase;
                MaxShield = 0;
            }
            else 
            {
                Log.Error("No Basic Enemy of type [" + controller.basicEnemyType.ToString() + "]");
            }
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
            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;

            if (controller.basicEnemyType == BasicEnemyType.AI)
            {
                GameEntry.Event.Fire(this, AISpaceshipDestroyedEventArgs.Create());
            }
            else
            {
                BattleManager.Instance.basicEnemyKilled++;
            }
            ObjectPoolManager.SpawnObject(BattleManager.Instance.explodeFXPrefab, transform.position, transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void OnDisable() 
        {
            StopAllCoroutines();    
        }
    }
}
