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

            StoreRawSpaceshipAttribute();
            Debug.Log("Accuracy = " + procedureOwner.GetData<VarInt32>("Accuracy"));
            SetSpaceshipAttribute(procedureOwner.GetData<VarInt32>("Accuracy"));

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
                    SetBossDefeatTime(0, timeUsed);
                    break;
                case "CyberSecurity":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.ElectronicWarfare, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_Cybersecurity, 1);
                    SetBossDefeatTime(1, timeUsed);
                    break;
                case "DataScience":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.EnergyBoost, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_DataScience, 1);
                    SetBossDefeatTime(2, timeUsed);
                    break;
                case "AI":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.AdaptiveIntelligentDefense, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_AI, 1);
                    SetBossDefeatTime(3, timeUsed);
                    break;
                case "Blockchain":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.BlockchainResurgence, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_Blockchain, 1);
                    SetBossDefeatTime(4, timeUsed);
                    break;
                case "IoT":
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int) EnumSkill.MedicalSupport, 0);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.KnowledgeFragments_IoT, 1);
                    SetBossDefeatTime(5, timeUsed);
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

            ResetSpaceshipAttribute();

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

        private void StoreRawSpaceshipAttribute()
        {
            PlayerCalculatedSpaceshipData rawData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData;
            this.procedureOwner.SetData<VarDouble>(Constant.Type.ATTR_Agility.ToString(), (double) rawData.Agility);
            this.procedureOwner.SetData<VarDouble>(Constant.Type.ATTR_Durability.ToString(), (double) rawData.Durability);
            this.procedureOwner.SetData<VarDouble>(Constant.Type.ATTR_Energy.ToString(), (double) rawData.Energy);
            this.procedureOwner.SetData<VarDouble>(Constant.Type.ATTR_Firepower.ToString(), (double) rawData.Firepower);
            this.procedureOwner.SetData<VarDouble>(Constant.Type.ATTR_Firerate.ToString(), (double) rawData.FireRate);
            this.procedureOwner.SetData<VarDouble>(Constant.Type.ATTR_Shields.ToString(), (double) rawData.Shields);
        }

        private void SetSpaceshipAttribute(int accuracy)
        {
            float boostScale = Mathf.Max(1, 1 + (float)(accuracy - 50) / 100f);
            Debug.Log("Boost Scale = " + boostScale);

            PlayerCalculatedSpaceshipData data = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData;

            string planetType = this.procedureOwner.GetData<VarString>("BossType");
            switch (planetType)
            {
                case "CloudComputing":
                    data.Firepower *= boostScale;
                    data.Energy *= boostScale;
                    break;
                case "AI":
                    data.Energy *= boostScale;
                    data.Firepower *= boostScale;
                    break;
                case "CyberSecurity":
                    data.Shields *= boostScale;
                    data.Durability *= boostScale;
                    break;
                case "DataScience":
                    data.Firepower *= boostScale;
                    data.Agility *= boostScale;
                    break;
                case "Blockchain":
                    data.Durability *= boostScale;
                    data.Shields *= boostScale;
                    break;
                case "IoT":
                    data.Agility *= boostScale;
                    data.Durability *= boostScale;
                    break;
            }

            // data.Agility *= boostScale;
            // data.Durability *= boostScale;
            // data.Energy *= boostScale;
            // data.Firepower *= boostScale;
            // data.FireRate *= boostScale;
            // data.Shields *= boostScale;
        }

        private void ResetSpaceshipAttribute()
        {
            PlayerCalculatedSpaceshipData data = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData;

            data.Agility = (float) this.procedureOwner.GetData<VarDouble>(Constant.Type.ATTR_Agility.ToString());
            data.Durability = (float) this.procedureOwner.GetData<VarDouble>(Constant.Type.ATTR_Durability.ToString());
            data.Energy = (float) this.procedureOwner.GetData<VarDouble>(Constant.Type.ATTR_Energy.ToString());
            data.Firepower = (float) this.procedureOwner.GetData<VarDouble>(Constant.Type.ATTR_Firepower.ToString());
            data.FireRate = (float) this.procedureOwner.GetData<VarDouble>(Constant.Type.ATTR_Firerate.ToString());
            data.Shields = (float) this.procedureOwner.GetData<VarDouble>(Constant.Type.ATTR_Shields.ToString());
        }

        private void SetBossDefeatTime(int id, float timeUsed)
        {
            float currentTime = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().bossDefeatTime[id];
            if (currentTime < 0 || currentTime > timeUsed)
            {
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().bossDefeatTime[id] = timeUsed;
            }
        }
    }
}
