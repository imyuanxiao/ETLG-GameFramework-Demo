using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace ETLG
{
    public class PlayerAIShip : MonoBehaviour
    {
        [HideInInspector] public float lastingTime;
        private float timeElapsed;
        public GameObject missilePrefab;
        public Transform missileSpawnPoint;
        private int damage;
        private float fireRate;
        private float fireCnt;
        private Transform target;

        private void OnEnable() 
        {
            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Subscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Subscribe(BattleWinEventArgs.EventId, OnBattleWin);
            GameEntry.Event.Subscribe(BasicBattleWinEventArgs.EventId, OnBasicBattleWin);

            timeElapsed = 0f;
            fireCnt = 0f;
            fireRate = 2.0f;
            damage = (int) (GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower * 0.2);
        }

        private void OnBasicBattleWin(object sender, GameEventArgs e)
        {
            BasicBattleWinEventArgs ne = (BasicBattleWinEventArgs) e;
            if (ne == null)
                Log.Error("Invalid Event [BasciBattleWinEventArgs]");
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void OnGamePause(object sender, GameEventArgs e)
        {
            
        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            PlayerDeadEventArgs ne = (PlayerDeadEventArgs) e;
            if (ne == null)
                Log.Error("Invalid Event [PlayerDeadEventArgs]");
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void OnBattleWin(object sender, GameEventArgs e)
        {
            BattleWinEventArgs ne = (BattleWinEventArgs)e;
            if (ne == null)
            {
                Log.Error("Invalid Event [BattleWinEventArgs]");
            }
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void Fire() 
        {
            GameObject missile = ObjectPoolManager.SpawnObject(missilePrefab, missileSpawnPoint.position, gameObject.transform.rotation, ObjectPoolManager.PoolType.GameObject);
            SetMissileTarget();
            InitPlayerAIMissile(missile.GetComponent<Missile>(), target);
        }

        private void InitPlayerAIMissile(Missile missile, Transform target)
        {
            ProjectileData missileData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int)EnumEntity.Missile);
            
            missile.target = target;
            missile.damage = this.damage;
            missile.flyingDirection = missile.transform.forward;
            missile.flyingSpeed = missileData.Speed * 160;
        }

        private void SetMissileTarget()
        {
            target = null;
            if (GameEntry.Procedure.CurrentProcedure is ProcedureIntermidateBattle || GameEntry.Procedure.CurrentProcedure is ProcedureFinalBattle)
            {
                target = BattleManager.Instance.bossEnemyEntity.gameObject.transform;
            }
            else if (GameEntry.Procedure.CurrentProcedure is ProcedureBasicBattle)
            {
                target = FindObjectOfType<BasicEnemyController>()?.gameObject.transform;
            }
        }

        private void Update() 
        {
            if (timeElapsed < lastingTime)
            {
                timeElapsed += Time.deltaTime;
            }
            else
            {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }

            if (fireCnt < fireRate)
            {
                fireCnt += Time.deltaTime;
            }
            else
            {
                fireCnt = 0f;
                Fire();
            }
        }

        private void OnDisable() 
        {
            GameEntry.Event.Unsubscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Unsubscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Unsubscribe(BattleWinEventArgs.EventId, OnBattleWin);
            GameEntry.Event.Unsubscribe(BasicBattleWinEventArgs.EventId, OnBasicBattleWin);

            timeElapsed = 0f;
            fireCnt = 0f;
        }
    }
}
