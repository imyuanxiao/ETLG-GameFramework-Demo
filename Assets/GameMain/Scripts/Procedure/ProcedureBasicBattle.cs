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
        private float startTime = 0f;

        private Queue<AchievementPopUpEventArgs> popupQueue = new Queue<AchievementPopUpEventArgs>();
        private bool isAchievementShowing;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            this.procedureOwner = procedureOwner;
            this.changeScene = false;
            this.isWinning = false;
            this.startTime = Time.time;

            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Subscribe(BasicBattleWinEventArgs.EventId, OnBasicBattleWin);
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Subscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);

            StoreRawSpaceshipAttribute();
            SetSpaceshipAttribute(procedureOwner.GetData<VarInt32>("Accuracy"));

            entityLoader = EntityLoader.Create(this);

            // get player data
            PlayerData playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();
            // show player spaceship entity
            entityLoader.ShowEntity<EntitySpaceship>(playerData.initialSpaceship.EntityId, onShowSuccess, EntityDataSpaceship.Create(playerData));

            BattleManager.Instance.SpawnBasicEnemies();

            GameEntry.Sound.PlayMusic(EnumSound.MenuBGM);
        }

        private void OnBasicBattleWin(object sender, GameEventArgs e)
        {
            float timeUsed = Time.time - this.startTime;
            
            Dictionary<string, int> result = new Dictionary<string, int>();
            BasicBattleWinEventArgs ne = (BasicBattleWinEventArgs) e;

            GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact((int) EnumArtifact.Money, ne.BasicEnemyKilled * 100);
            
            result.Add("Killed", ne.BasicEnemyKilled);
            result.Add("Passed", ne.BasicEnemyPassed);
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBattleWin, result);
        }

        private void OnGamePause(object sender, GameEventArgs e)
        {
            GamePauseEventArgs ne = (GamePauseEventArgs) e;
            GameEntry.UI.OpenUIForm(ne.UIPauseId);
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

            BattleManager.Instance.StopSpawnBasicEnemies();
            GameEntry.UI.OpenUIForm(EnumUIForm.UIBasicBattleLost);
            GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(6003, 1));
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
            
            ResetSpaceshipAttribute();
            GameEntry.Event.Fire(this, DeactiveBattleComponentEventArgs.Create());
            entityLoader.HideEntity(spaceShipEntity);
            entityLoader.Clear();
            GameEntry.Event.Unsubscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Unsubscribe(GamePauseEventArgs.EventId, OnGamePause);
            GameEntry.Event.Unsubscribe(BasicBattleWinEventArgs.EventId, OnBasicBattleWin);
            GameEntry.Event.Unsubscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);
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
        public void OnAchievementPoPUp(object sender, GameEventArgs e)
        {
            AchievementPopUpEventArgs ne = (AchievementPopUpEventArgs)e;
            if (ne == null)
                return;
            if (ne.Type == Constant.Type.UI_OPEN)
            {
                if (GameEntry.Data.GetData<DataPlayer>().GetPlayerData().isAchievementShouldAchieved(ne.achievementId,ne.count))
                {
                    popupQueue.Enqueue(ne);
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().UpdatePlayerAchievementData(ne.achievementId, 
                        GameEntry.Data.GetData<DataAchievement>().GetNextLevel(ne.achievementId, ne.count));
                    if (!isAchievementShowing)
                    {
                        isAchievementShowing = true;
                        ne = popupQueue.Dequeue();
                        GameEntry.Data.GetData<DataAchievement>().cuurrentPopUpId = ne.achievementId;
                        GameEntry.UI.OpenUIForm(EnumUIForm.UIAchievementPopUp);
                    }
                }
            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (popupQueue.Count == 0 && GameEntry.UI.HasUIForm(EnumUIForm.UIAchievementPopUp))
                {
                    GameEntry.UI.GetUIForm(EnumUIForm.UIAchievementPopUp).Close();
                    isAchievementShowing = false;
                }
                if (popupQueue.Count > 0)
                {
                    AchievementPopUpEventArgs popupArgs = popupQueue.Dequeue();
                    GameEntry.Data.GetData<DataAchievement>().cuurrentPopUpId = popupArgs.achievementId;
                    // ����UI
                    GameEntry.Event.Fire(this, AchievementMultiplesPopUpEventArgs.Create());
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
    }
}
