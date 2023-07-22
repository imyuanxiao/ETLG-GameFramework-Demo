﻿using System.Collections;
using System.Collections.Generic;
using ETLG.Data;
using GameFramework.Event;

using GameFramework.Localization;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ETLG
{
    public class ProcedurePlayerMenu : ProcedureBase
    {
        private ProcedureOwner procedureOwner;
        private bool changeScene = false;

        private DataPlayer dataPlayer;
        private DataArtifact dataArtifact;
        private DataSkill dataSkill;
        private DataAchievement dataAchievement;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // subscribe events
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(SkillInfoUIChangeEventArgs.EventId, OnSkillInfoUIChange);
            GameEntry.Event.Subscribe(SkillUpgradeInfoUIChangeEventArgs.EventId, OnSkillUpgradeInfoUIChange);
            GameEntry.Event.Subscribe(ModuleInfoUIChangeEventArgs.EventId, OnModuleInfoUIChange);
            GameEntry.Event.Subscribe(ModuleEquipUIchangeEventArgs.EventId, OnModuleEquipUIChange);
            GameEntry.Event.Subscribe(ArtifactInfoUIChangeEventArgs.EventId, OnArtifactInfoUIChange);
            GameEntry.Event.Subscribe(TipUIChangeEventArgs.EventId, OnTipUIChange);
            GameEntry.Event.Subscribe(ChangePlayerMenuEventArgs.EventId, OnChangePlayerMenu);
            GameEntry.Event.Subscribe(EquippedModuleChangesEventArgs.EventId, OnEquippedModuleChanges);
            GameEntry.Event.Subscribe(AchievementPopUpEventArgs.EventId,OnAchievementPoPUp);
            GameEntry.Event.Subscribe(PlayerZoneUIChangeEventArgs.EventId, OnPlayerZoneUIChange);

            this.procedureOwner = procedureOwner;
            this.changeScene = false;
            LeaderboardManager.Instance.leaderboardData = null;
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataSkill = GameEntry.Data.GetData<DataSkill>();
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();
            ResetStates();

            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);

            GameEntry.UI.OpenUIForm(EnumUIForm.UISpaceshipCheckForm);


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

            // 取消订阅事件
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Unsubscribe(SkillInfoUIChangeEventArgs.EventId, OnSkillInfoUIChange);
            GameEntry.Event.Unsubscribe(SkillUpgradeInfoUIChangeEventArgs.EventId, OnSkillUpgradeInfoUIChange);
            GameEntry.Event.Unsubscribe(ArtifactInfoUIChangeEventArgs.EventId, OnArtifactInfoUIChange);
           
            GameEntry.Event.Unsubscribe(TipUIChangeEventArgs.EventId, OnTipUIChange);
            GameEntry.Event.Unsubscribe(ChangePlayerMenuEventArgs.EventId, OnChangePlayerMenu);

            GameEntry.Event.Unsubscribe(ModuleInfoUIChangeEventArgs.EventId, OnModuleInfoUIChange);
            GameEntry.Event.Unsubscribe(ModuleEquipUIchangeEventArgs.EventId, OnModuleEquipUIChange);

            GameEntry.Event.Unsubscribe(EquippedModuleChangesEventArgs.EventId, OnEquippedModuleChanges);

            GameEntry.Event.Unsubscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);
            GameEntry.Event.Unsubscribe(PlayerZoneUIChangeEventArgs.EventId, OnPlayerZoneUIChange);
            // 停止音乐
            GameEntry.Sound.StopMusic();



        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void OnChangeScene(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne == null)
                return;

            changeScene = true;
            procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.SceneId);
        }


        private void OnChangePlayerMenu(object sender, GameEventArgs e)
        {
            ChangePlayerMenuEventArgs ne = (ChangePlayerMenuEventArgs)e;
            if (ne == null)
                return;
            dataPlayer.currentSelectedPlayerMenu = ne.UIFormID;

            ResetStates();

            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.UI.OpenUIForm(ne.UIFormID);
        }

        private void ResetStates()
        {
            dataArtifact.lockCurrentModuleID = false;
            dataSkill.lockCurrentSkillID = false;
        }


        private void OnSkillInfoUIChange(object sender, GameEventArgs e)
        {
            SkillInfoUIChangeEventArgs ne = (SkillInfoUIChangeEventArgs)e;
            if (ne == null)
                return;

            if (ne.Type == Constant.Type.UI_CLOSE && !GameEntry.UI.HasUIForm(EnumUIForm.UISkillUpgradeInfoForm))
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UISkillInfoForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UISkillInfoForm));
                }
            }

            if (ne.Type == Constant.Type.UI_OPEN && !GameEntry.UI.HasUIForm(EnumUIForm.UISkillUpgradeInfoForm))
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UISkillInfoForm);
            }
        }

        private void OnSkillUpgradeInfoUIChange(object sender, GameEventArgs e)
        {
            SkillUpgradeInfoUIChangeEventArgs ne = (SkillUpgradeInfoUIChangeEventArgs)e;
            if (ne == null)
                return;


            if (ne.Type == Constant.Type.UI_OPEN)
            {
                if (!GameEntry.UI.HasUIForm(EnumUIForm.UISkillUpgradeInfoForm))
                {
                    GameEntry.UI.OpenUIForm(EnumUIForm.UISkillUpgradeInfoForm);
                };
                
            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UISkillUpgradeInfoForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UISkillUpgradeInfoForm));
                };
                if (GameEntry.UI.HasUIForm(EnumUIForm.UISkillInfoForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UISkillInfoForm));
                };
            }
        }

        private void OnArtifactInfoUIChange(object sender, GameEventArgs e)
        {
            ArtifactInfoUIChangeEventArgs ne = (ArtifactInfoUIChangeEventArgs)e;
            if (ne == null)
                return;


            if (GameEntry.UI.HasUIForm(EnumUIForm.UIArtifactInfoForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIArtifactInfoForm));
            }

            if (ne.Type == Constant.Type.UI_OPEN)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UIArtifactInfoForm);
            }

        }

        private void OnTipUIChange(object sender, GameEventArgs e)
        {
            TipUIChangeEventArgs ne = (TipUIChangeEventArgs)e;
            if (ne == null)
                return;

            if (GameEntry.UI.HasUIForm(EnumUIForm.UITipForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UITipForm));
            }

            if (ne.Type == Constant.Type.UI_OPEN)
            {
                dataPlayer.tipUiPosition = ne.position;
                dataPlayer.tipTitle = ne.tipTitle;
                if (ne.level != 0 && ne.achievementId != 0)
                {
                    dataAchievement.discriptionId = ne.achievementId;
                    dataAchievement.descriptionLevel = ne.level;
                }
                GameEntry.UI.OpenUIForm(EnumUIForm.UITipForm);
            }

        }



        private void OnModuleInfoUIChange(object sender, GameEventArgs e)
        {
            ModuleInfoUIChangeEventArgs ne = (ModuleInfoUIChangeEventArgs)e;
            if (ne == null)
                return;

            if (ne.Type == Constant.Type.UI_OPEN && !GameEntry.UI.HasUIForm(EnumUIForm.UIModuleEquipForm))
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UIModuleInfoForm);
            }

            if (ne.Type == Constant.Type.UI_CLOSE && !GameEntry.UI.HasUIForm(EnumUIForm.UIModuleEquipForm))
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UIModuleInfoForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIModuleInfoForm));
                }
            }
        }

        private void OnModuleEquipUIChange(object sender, GameEventArgs e)
        {
            ModuleEquipUIchangeEventArgs ne = (ModuleEquipUIchangeEventArgs)e;
            if (ne == null)
                return;



            if (ne.Type == Constant.Type.UI_OPEN)
            {
                if (!GameEntry.UI.HasUIForm(EnumUIForm.UIModuleEquipForm))
                {
                    GameEntry.UI.OpenUIForm(EnumUIForm.UIModuleEquipForm);
                };

            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UIModuleEquipForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIModuleEquipForm));
                };
                if (GameEntry.UI.HasUIForm(EnumUIForm.UIModuleInfoForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIModuleInfoForm));
                };
            }
        }

        public void OnEquippedModuleChanges(object sender, GameEventArgs e)
        {
            EquippedModuleChangesEventArgs ne = (EquippedModuleChangesEventArgs)e;
            if (ne == null)
                return;

            if (GameEntry.UI.HasUIForm(EnumUIForm.UIModuleEquipForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIModuleEquipForm));
            };
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIModuleInfoForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIModuleInfoForm));
            };
 
        }
        public void OnAchievementPoPUp(object sender, GameEventArgs e)
        {
            AchievementPopUpEventArgs ne = (AchievementPopUpEventArgs)e;
            if (ne == null)
                return;
            if (ne.Type == Constant.Type.UI_OPEN)
            {
                dataAchievement.cuurrentPopUpId = ne.achievementId;
                if (!dataPlayer.GetPlayerData().isAchievementAchieved(ne.count))
                {
                    dataPlayer.GetPlayerData().UpdatePlayerAchievementData(ne.achievementId, dataAchievement.GetNextLevel(ne.achievementId, ne.count));
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
            public void OnPlayerZoneUIChange(object sender, GameEventArgs e)
        {
            PlayerZoneUIChangeEventArgs ne = (PlayerZoneUIChangeEventArgs)e;
            if (ne == null)
                return;
            if (ne.Type == Constant.Type.UI_OPEN)
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UIPlayerZoneForm))
                {
                    GameEntry.UI.GetUIForm(EnumUIForm.UIPlayerZoneForm).GetComponent<UIPlayerZoneForm>().updateData();
                }
                else
                {
                    GameEntry.UI.OpenUIForm(EnumUIForm.UIPlayerZoneForm);
                }
            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UIPlayerZoneForm))
                {
                    GameEntry.UI.GetUIForm(EnumUIForm.UIPlayerZoneForm).Close();
                }
            }
        }
    }
}

