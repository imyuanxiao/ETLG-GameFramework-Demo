using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;
using GameFramework.Event;
using System;

namespace ETLG
{
    public class PlayerHealth : Health
    {
        private PlayerData playerData;

        private void OnEnable() 
        {
            playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();

            GameEntry.Event.Subscribe(PlayerHealthChangeEventArgs.EventId, OnPlayerHealthChange);
            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);

            MaxHealth = (int) playerData.initialSpaceship.Durability;
            CurrentHealth = (int) playerData.calculatedSpaceship.Durability;
            MaxShield = (int) playerData.initialSpaceship.Shields;
            CurrentShield = (int) playerData.calculatedSpaceship.Shields;

            // StartCoroutine(CheckDeath());
        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            PlayerDeadEventArgs ne = (PlayerDeadEventArgs) e;
            Debug.Log("Player Health: Player Dead");
        }

        private void OnPlayerHealthChange(object sender, GameEventArgs e)
        {
            PlayerHealthChangeEventArgs ne = (PlayerHealthChangeEventArgs) e;

            if (ne.CurrentHealth <= 0)
            {
                GameEntry.Event.Fire(this, PlayerDeadEventArgs.Create());
            }
        }

        public override void TakeDamage(int damage)
        {
            Debug.Log("Player's being hit");
            if (CurrentHealth <= 0)
            {
                return;
            }

            if (CurrentShield > 0)
            {
                CurrentShield = Mathf.Max(0, CurrentShield - damage);
            }
            else
            {
                CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
                GameEntry.Event.Fire(this, PlayerHealthChangeEventArgs.Create(CurrentHealth));
            }
            Debug.Log("Player CurrentShield: " + CurrentShield + " | " + "CurrentHealth: " + CurrentHealth);
            // playerData.CalculateStats();
        }


        protected override void OnDead()
        {

        }

        private void OnDisable() 
        {
            GameEntry.Event.Unsubscribe(PlayerHealthChangeEventArgs.EventId, OnPlayerHealthChange);
            GameEntry.Event.Unsubscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            StopAllCoroutines();    
        }
    }
}
