using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Procedure;
using ETLG.Data;
using GameFramework.Event;
using System;

namespace ETLG
{
    public class ProcedureBasicBattle : ProcedureBase
    {
        private EntityLoader entityLoader;
        private Entity spaceShipEntity;
        private ProcedureOwner procedureOwner;
        private bool changeScene = false;
        private bool isWinning = false;

        // public int basicEnemyAttackBase;
        // public int basicEnemyHealthBase;


        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            this.procedureOwner = procedureOwner;
            this.changeScene = false;

            // calculate basic enemy's attack power and health value
            // basicEnemyAttackBase = (int) ((int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Durability * 0.1);
            // basicEnemyHealthBase = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower;

            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Subscribe(BasicBattleWinEventArgs.EventId, OnBasicBattleWin);
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(GamePauseEventArgs.EventId, OnGamePause);

            entityLoader = EntityLoader.Create(this);

            // get player data
            PlayerData playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();
            // show player spaceship entity
            entityLoader.ShowEntity<EntitySpaceship>(playerData.initialSpaceship.EntityId, onShowSuccess, EntityDataSpaceship.Create(playerData));

            // GameEntry.Event.Fire(this, ActiveBattleComponentEventArgs.Create());

            // BattleManager.Instance.SetEnemiesData();
            BattleManager.Instance.SpawnBasicEnemies();

            GameEntry.Sound.PlayMusic(EnumSound.MenuBGM);
        }

        private void OnBasicBattleWin(object sender, GameEventArgs e)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            BasicBattleWinEventArgs ne = (BasicBattleWinEventArgs) e;
            result.Add("Killed", ne.BasicEnemyKilled);
            result.Add("Passed", ne.BasicEnemyPassed);
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBattleWin, result);
            Debug.Log("Enemy Killed: " + ne.BasicEnemyKilled + " | Enemy Passed: " + ne.BasicEnemyPassed);
        }

        private void OnGamePause(object sender, GameEventArgs e)
        {
            GamePauseEventArgs ne = (GamePauseEventArgs) e;
            GameEntry.UI.OpenUIForm(ne.UIPauseId);
            // Time.timeScale = 0;
        }

        private void OnChangeScene(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne == null)
                return;

            changeScene = true;
            procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.SceneId);
        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            PlayerDeadEventArgs ne = (PlayerDeadEventArgs) e;
            Debug.Log("ProcedureBasicBattle: Player Dead");
            BattleManager.Instance.StopSpawnBasicEnemies();
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBasicBattleLost);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameEntry.Event.Fire(this, GamePauseEventArgs.Create(EnumUIForm.UIPausePanelForm));
            }

            if (changeScene)
            {
                ChangeState<ProcedureLoadingScene>(procedureOwner);
            }

            CheckWinning();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            
            GameEntry.Event.Fire(this, DeactiveBattleComponentEventArgs.Create());
            entityLoader.HideEntity(spaceShipEntity);
            entityLoader.Clear();
            GameEntry.Event.Unsubscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Unsubscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Unsubscribe(BasicBattleWinEventArgs.EventId, OnBasicBattleWin);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void onShowSuccess(Entity entity)
        {
            spaceShipEntity = entity;
            GameEntry.Event.Fire(this, ActiveBattleComponentEventArgs.Create());
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBattleInfo, entity.GetComponent<Health>());
        }

        private void CheckWinning()
        {
            if (isWinning) { return; }
            int basicEnemyKilled = BattleManager.Instance.basicEnemyKilled;
            int basicEnemyPassed = BattleManager.Instance.basicEnemyPassed;

            if (basicEnemyKilled + basicEnemyPassed >= BattleManager.Instance.basicEnemyNum)
            {
                isWinning = true;
                GameEntry.Event.Fire(this, BasicBattleWinEventArgs.Create(basicEnemyKilled, basicEnemyPassed));
            }
        }
    }
}
