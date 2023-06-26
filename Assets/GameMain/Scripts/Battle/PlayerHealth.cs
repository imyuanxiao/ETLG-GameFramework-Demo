using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;

namespace ETLG
{
    public class PlayerHealth : Health
    {
        private PlayerData playerData;

        private void OnEnable() 
        {
            playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();

            MaxHealth = (int) playerData.initialSpaceship.Durability;
            CurrentHealth = (int) playerData.calculatedSpaceship.Durability;
            MaxShield = (int) playerData.initialSpaceship.Shields;
            CurrentShield = (int) playerData.calculatedSpaceship.Shields;
        }

        public override void TakeDamage(int damage)
        {
            Debug.Log("Player's being hit");
            playerData.CalculateStats();
        }


        protected override void OnDead()
        {
            Debug.Log("Player Dead");
        }
    }
}
