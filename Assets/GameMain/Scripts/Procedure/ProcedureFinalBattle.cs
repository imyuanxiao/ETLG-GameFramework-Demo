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
    public class ProcedureFinalBattle : ProcedureBase
    {
        private EntityLoader entityLoader;
        private Entity spaceShipEntity;
        private BossEnemyData bossEnemyData;
        private Entity bossEnemyEntity;
        private bool changeScene;
        private ProcedureOwner procedureOwner;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            this.changeScene = false;
            this.procedureOwner = procedureOwner;

            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Subscribe(BattleWinEventArgs.EventId, OnBattleWin);
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);

            entityLoader = EntityLoader.Create(this);

            // get player data
            PlayerData playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();
            // show player spaceship entity
            entityLoader.ShowEntity<EntitySpaceship>(playerData.initialSpaceship.EntityId, onShowSuccess, EntityDataSpaceship.Create(playerData));

            // GameEntry.Event.Fire(this, ActiveBattleComponentEventArgs.Create());

            bossEnemyData = GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.FinalBoss);
            entityLoader.ShowEntity<EntityBossEnemy>(bossEnemyData.EntityId, onBossEnemyShowSuccess, EntityDataBossEnemy.Create(bossEnemyData));
        
            GameEntry.Sound.PlayMusic(EnumSound.MenuBGM);
        }

        private void onBossEnemyShowSuccess(Entity entity)
        {
            bossEnemyEntity = entity;
            BattleManager.Instance.bossEnemyEntity = entity;
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBossEnemyHealth, entity.GetComponent<Health>());
        }

        private void OnChangeScene(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs) e;
            if (ne == null)
                return;

            changeScene = true;
            procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.SceneId);
        }

        private void OnBattleWin(object sender, GameEventArgs e)
        {
            BattleWinEventArgs ne = (BattleWinEventArgs) e;
            // PlayerData playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBattleWin); //, playerData);
            entityLoader.HideEntity(bossEnemyEntity);
        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            PlayerDeadEventArgs ne = (PlayerDeadEventArgs) e;
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBasicBattleLost);
            entityLoader.HideEntity(bossEnemyEntity);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (changeScene)
            {
                ChangeState<ProcedureLoadingScene>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Fire(this, DeactiveBattleComponentEventArgs.Create());
            entityLoader.HideEntity(spaceShipEntity);
            // entityLoader.HideEntity(bossEnemyEntity);

            GameEntry.Event.Unsubscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Unsubscribe(BattleWinEventArgs.EventId, OnBattleWin);
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
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
    }
}
