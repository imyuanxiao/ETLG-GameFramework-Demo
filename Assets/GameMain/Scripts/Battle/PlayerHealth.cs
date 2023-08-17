using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;
using GameFramework.Event;
using UnityGameFramework.Runtime;
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

            MaxHealth = (int) playerData.playerCalculatedSpaceshipData.Durability;
            MaxShield = (int) playerData.playerCalculatedSpaceshipData.Shields;
            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;
        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            PlayerDeadEventArgs ne = (PlayerDeadEventArgs) e;
        }

        private void OnPlayerHealthChange(object sender, GameEventArgs e)
        {
            PlayerHealthChangeEventArgs ne = (PlayerHealthChangeEventArgs) e;

            if (ne == null)
                Log.Error("Invalid event [OnplayerHealthChangeEventArgs]");

            if (ne.CurrentHealth <= 0)
            {
                // if player can respawn
                if (GetComponent<SpaceshipSkill>().canRespawn && GetComponent<SpaceshipSkill>().respawnCnt < 1) 
                {
                    GetComponent<SpaceshipSkill>().respawnCnt++;
                    GameEntry.Event.Fire(this, PlayerRespawnEventArgs.Create(this));
                }
                // if player can't respawn
                else 
                {
                    ObjectPoolManager.SpawnObject(BattleManager.Instance.explodeFXPrefab, transform.position, transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);
                    GameEntry.Event.Fire(this, PlayerDeadEventArgs.Create());
                }
            }
        }

        public override void TakeDamage(int damage)
        {
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
