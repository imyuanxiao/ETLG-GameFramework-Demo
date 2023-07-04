using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Procedure;
using ETLG.Data;
using System;
using GameFramework.Event;

namespace ETLG
{
    public class ProcedureIntermidateBattle : ProcedureBase
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
            
            changeScene = false;
            this.procedureOwner = procedureOwner;

            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Subscribe(BattleWinEventArgs.EventId, OnBattleWin);
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);

            // Debug.Log(BattleManager.Instance.bossType);

            entityLoader = EntityLoader.Create(this);

            // get player data
            PlayerData playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();
            // show player spaceship entity
            entityLoader.ShowEntity<EntitySpaceship>(playerData.initialSpaceship.EntityId, onShowPlayerSuccess, EntityDataSpaceship.Create(playerData));

            GameEntry.Event.Fire(this, ActiveBattleComponentEventArgs.Create());

            LoadBossEnemy();
            // bossEnemyData = GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.CloudComputingBoss);
            // entityLoader.ShowEntity<EntityBossEnemy>(bossEnemyData.EntityId, onBossEnemyShowSuccess, EntityDataBossEnemy.Create(bossEnemyData));
            // Debug.Log(bossEnemyData.NameId);
        }

        private void LoadBossEnemy()
        {
            switch (this.procedureOwner.GetData<VarString>("BossType"))
            {
                case "CloudComputing":
                    bossEnemyData = GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.CloudComputingBoss);
                    entityLoader.ShowEntity<EntityBossEnemy>(bossEnemyData.EntityId, onBossEnemyShowSuccess, EntityDataBossEnemy.Create(bossEnemyData));
                    break;
                case "Cybersecurity":
                    bossEnemyData = GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.CybersecurityBoss);
                    entityLoader.ShowEntity<EntityBossEnemy>(bossEnemyData.EntityId, onBossEnemyShowSuccess, EntityDataBossEnemy.Create(bossEnemyData));
                    break;
                case "AI":
                    bossEnemyData = GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.ArtificialIntelligenceBoss);
                    entityLoader.ShowEntity<EntityBossEnemy>(bossEnemyData.EntityId, onBossEnemyShowSuccess, EntityDataBossEnemy.Create(bossEnemyData));
                    break;
                case "DataScience":
                    bossEnemyData = GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.DataScienceBoss);
                    entityLoader.ShowEntity<EntityBossEnemy>(bossEnemyData.EntityId, onBossEnemyShowSuccess, EntityDataBossEnemy.Create(bossEnemyData));
                    break;
                case "Blockchain":
                    bossEnemyData = GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.BlockchainBoss);
                    entityLoader.ShowEntity<EntityBossEnemy>(bossEnemyData.EntityId, onBossEnemyShowSuccess, EntityDataBossEnemy.Create(bossEnemyData));
                    break;
                case "IoT":
                    bossEnemyData = GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.InternetofThingsBoss);
                    entityLoader.ShowEntity<EntityBossEnemy>(bossEnemyData.EntityId, onBossEnemyShowSuccess, EntityDataBossEnemy.Create(bossEnemyData));
                    break;
                case "Final":
                    bossEnemyData = GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.FinalBoss);
                    entityLoader.ShowEntity<EntityBossEnemy>(bossEnemyData.EntityId, onBossEnemyShowSuccess, EntityDataBossEnemy.Create(bossEnemyData));
                    break;
                default:
                    Log.Error("No boss of type: " + this.procedureOwner.GetData<VarString>("BossType"));
                    break;
            }
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

        private void onBossEnemyShowSuccess(Entity entity)
        {
            bossEnemyEntity = entity;
            BattleManager.Instance.bossEnemyEntity = entity;
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBossEnemyHealth, entity.GetComponent<Health>());
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

        private void onShowPlayerSuccess(Entity entity)
        {
            spaceShipEntity = entity;
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBattleInfo, entity.GetComponent<Health>());
        }
    }
}
