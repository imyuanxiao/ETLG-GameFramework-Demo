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
            // if current procedure is basic battle procedure
            InitBasicBattleEnemy();

            // if current procedure is intermidate battle procedure
            // MaxHealth = ;
            // MaxShield = ;

            // if current procedure is final battle procedure
            // MaxHealth = ;
            // MaxShield = ;

            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;

            StartCoroutine(CheckDeath());    

            // Debug.Log("Enemy Max Health: " + MaxHealth + " / " + GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship.Firepower + " | " + "Enemy Max Shield: " + MaxShield);
        }


        private void InitBasicBattleEnemy()
        {
            if (controller.basicEnemyType == BasicEnemyType.BasicEnemy1)
            {
                // MaxHealth = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship.Firepower * 2;
                MaxHealth = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower * 2;
                MaxShield = 0;
            }
            else if (controller.basicEnemyType == BasicEnemyType.BasicEnemy2)
            {
                // MaxHealth = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship.Firepower * 3;
                MaxHealth = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower * 3;
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
            // Debug.Log("Enemy CurrentShield: " + CurrentShield + " | " + "CurrentHealth: " + CurrentHealth);
        }

        protected override void OnDead()
        {
            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;
            BattleManager.Instance.basicEnemyKilled++;
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void OnDisable() 
        {
            StopAllCoroutines();    
        }
    }
}
