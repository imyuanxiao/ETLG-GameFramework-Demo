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
        private float startTime;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            changeScene = false;
            this.procedureOwner = procedureOwner;
            this.startTime = Time.time;

            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Subscribe(BattleWinEventArgs.EventId, OnBattleWin);
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Subscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);

            // Debug.Log(BattleManager.Instance.bossType);

            entityLoader = EntityLoader.Create(this);

            // get player data
            PlayerData playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();
            // show player spaceship entity
            entityLoader.ShowEntity<EntitySpaceship>(playerData.initialSpaceship.EntityId, onShowPlayerSuccess, EntityDataSpaceship.Create(playerData));

            // GameEntry.Event.Fire(this, ActiveBattleComponentEventArgs.Create());

            LoadBossEnemy();

            GameEntry.Sound.PlayMusic(EnumSound.MenuBGM);
        }

        private void LoadBossEnemy()
        {
            switch (this.procedureOwner.GetData<VarString>("BossType"))
            {
                case "CloudComputing":
                    bossEnemyData = GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.CloudComputingBoss);
                    entityLoader.ShowEntity<EntityBossEnemy>(bossEnemyData.EntityId, onBossEnemyShowSuccess, EntityDataBossEnemy.Create(bossEnemyData));
                    break;
                case "CyberSecurity":
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

            float timeUsed = Time.time - this.startTime;
            Debug.Log("Time Used: " + timeUsed);

            // PlayerData playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBattleWin); //, playerData);
            entityLoader.HideEntity(bossEnemyEntity);

            // Unlock battle skills, award Knowledge Fragments
            switch (this.procedureOwner.GetData<VarString>("BossType"))
            {
                case "CloudComputing":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.EdgeComputing, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_CloudComputing, 1);
                    break;
                case "CyberSecurity":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.ElectronicWarfare, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_Cybersecurity, 1);
                    break;
                case "DataScience":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.EnergyBoost, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_DataScience, 1);
                    break;
                case "AI":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.AdaptiveIntelligentDefense, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_AI, 1);
                    break;
                case "Blockchain":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.BlockchainResurgence, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_Blockchain, 1);
                    break;
                case "IoT":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.MedicalSupport, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_IoT, 1);
                    break;
                default:
                    break;
            }
            GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(6001, 1));
            GameEntry.Data.GetData<DataPlayer>().GetPlayerData().battleVictoryCount++;
            GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(6002, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().battleVictoryCount));
        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            PlayerDeadEventArgs ne = (PlayerDeadEventArgs) e;
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBasicBattleLost);
            entityLoader.HideEntity(bossEnemyEntity);
            GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(6003, 1));
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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameEntry.Event.Fire(this, GamePauseEventArgs.Create(EnumUIForm.UIPausePanelForm));
            }

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
            GameEntry.Event.Unsubscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Unsubscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);
        }

        private void OnGamePause(object sender, GameEventArgs e)
        {
            GamePauseEventArgs ne = (GamePauseEventArgs) e;
            GameEntry.UI.OpenUIForm(ne.UIPauseId);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
        

        private void onShowPlayerSuccess(Entity entity)
        {
            spaceShipEntity = entity;
            GameEntry.Event.Fire(this, ActiveBattleComponentEventArgs.Create());
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBattleInfo, entity.GetComponent<Health>());
        }

        public void OnAchievementPoPUp(object sender, GameEventArgs e)
        {
            AchievementPopUpEventArgs ne = (AchievementPopUpEventArgs)e;
            if (ne == null)
                return;
            if (ne.Type == Constant.Type.UI_OPEN)
            {
                GameEntry.Data.GetData<DataAchievement>().cuurrentPopUpId = ne.achievementId;
                if (!GameEntry.Data.GetData<DataPlayer>().GetPlayerData().isAchievementAchieved(ne.count))
                {
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().UpdatePlayerAchievementData(ne.achievementId, 
                                                                                GameEntry.Data.GetData<DataAchievement>().GetNextLevel(ne.achievementId, ne.count));
                    GameEntry.UI.OpenUIForm(EnumUIForm.UIAchievementPopUp);

                }
            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UIAchievementPopUp))
                {
                    GameEntry.UI.GetUIForm(EnumUIForm.UIAchievementPopUp).Close();
                }
            }
        }
    }
}
