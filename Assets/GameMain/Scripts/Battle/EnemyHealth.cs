using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;

namespace ETLG
{
    public class EnemyHealth : Health
    {
        private void OnEnable() 
        {
            // if current procedure is basic battle procedure
            MaxHealth = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship.Firepower * 2;
            MaxShield = 0;

            // if current procedure is intermidate battle procedure
            // MaxHealth = ;
            // MaxShielf = ;

            // if current procedure is final battle procedure
            // MaxHealth = ;
            // MaxShielf = ;

            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;

            Debug.Log("Enemy Max Health: " + MaxHealth + " / " + GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship.Firepower + " | " + "Enemy Max Shield: " + MaxShield);
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
            Debug.Log("Enemy Shield: " + CurrentShield);
            Debug.Log("Enemy CurrentHealth: " + CurrentHealth);
        }

        protected override void OnDead()
        {
            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
