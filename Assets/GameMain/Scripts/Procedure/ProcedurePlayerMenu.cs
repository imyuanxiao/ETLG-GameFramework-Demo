using System.Collections;
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

        private int? skillInfoUIID;
        private int? skillUpgradeInfoUIID;
        private int? artifactInfoUIID;
        private int? moduleInfoUIID;
        private int? moduleEquipInfoUIID;

        private int? achievementUIID;

        private DataPlayer dataPlayer;
        private int? tipUIID;


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
            this.procedureOwner = procedureOwner;
            this.changeScene = false;

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            skillInfoUIID = null;
            skillUpgradeInfoUIID = null;
            artifactInfoUIID = null;
            tipUIID = null;

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

            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.UI.OpenUIForm(ne.UIFormID);

        }


        private void OnSkillInfoUIChange(object sender, GameEventArgs e)
        {
            SkillInfoUIChangeEventArgs ne = (SkillInfoUIChangeEventArgs)e;
            if (ne == null)
                return;


            if (ne.Type == Constant.Type.UI_OPEN && skillUpgradeInfoUIID == null)
            {
                skillInfoUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UISkillInfoForm);
            }

            if (ne.Type == Constant.Type.UI_CLOSE && skillUpgradeInfoUIID == null)
            {
                if (skillInfoUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)skillInfoUIID);
                }
                skillInfoUIID = null;
            }
        }

        private void OnSkillUpgradeInfoUIChange(object sender, GameEventArgs e)
        {
            SkillUpgradeInfoUIChangeEventArgs ne = (SkillUpgradeInfoUIChangeEventArgs)e;
            if (ne == null)
                return;

            if (ne.Type == Constant.Type.UI_OPEN)
            {
                if (skillUpgradeInfoUIID != null)
                {
                    return;
                }

                skillUpgradeInfoUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UISkillUpgradeInfoForm);
            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (skillUpgradeInfoUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)skillUpgradeInfoUIID);
                }
                skillUpgradeInfoUIID = null;

                if (skillInfoUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)skillInfoUIID);
                }
                skillInfoUIID = null;
            }
        }

        private void OnArtifactInfoUIChange(object sender, GameEventArgs e)
        {
            ArtifactInfoUIChangeEventArgs ne = (ArtifactInfoUIChangeEventArgs)e;
            if (ne == null)
                return;

            if (ne.Type == Constant.Type.UI_OPEN)
            {
                artifactInfoUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UIArtifactInfoForm);
            }
            else if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (artifactInfoUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)artifactInfoUIID);
                }
                artifactInfoUIID = null;
            }

        }

        private void OnTipUIChange(object sender, GameEventArgs e)
        {
            TipUIChangeEventArgs ne = (TipUIChangeEventArgs)e;
            if (ne == null)
                return;

            if (ne.Type == Constant.Type.UI_OPEN)
            {
                dataPlayer.tipUiPosition = ne.position;
                dataPlayer.tipTitle = ne.tipTitle;
                tipUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UITipForm);
            }

            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (tipUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)tipUIID);
                }
                tipUIID = null;
            }
        }



        private void OnModuleInfoUIChange(object sender, GameEventArgs e)
        {
            ModuleInfoUIChangeEventArgs ne = (ModuleInfoUIChangeEventArgs)e;
            if (ne == null)
                return;


            if (ne.Type == Constant.Type.UI_OPEN && moduleEquipInfoUIID == null)
            {
                moduleInfoUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UIModuleInfoForm);
            }

            if (ne.Type == Constant.Type.UI_CLOSE && moduleEquipInfoUIID == null)
            {
                if (moduleInfoUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)moduleInfoUIID);
                }
                moduleInfoUIID = null;
            }
        }

        private void OnModuleEquipUIChange(object sender, GameEventArgs e)
        {
            ModuleEquipUIchangeEventArgs ne = (ModuleEquipUIchangeEventArgs)e;
            if (ne == null)
                return;

            if (ne.Type == Constant.Type.UI_OPEN)
            {
                if (moduleEquipInfoUIID != null)
                {
                    return;
                }

                moduleEquipInfoUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UIModuleEquipForm);
            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (moduleEquipInfoUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)moduleEquipInfoUIID);
                }
                moduleEquipInfoUIID = null;

                if (moduleInfoUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)moduleInfoUIID);
                }
                moduleInfoUIID = null;
            }
        }

        public void OnEquippedModuleChanges(object sender, GameEventArgs e)
        {
            EquippedModuleChangesEventArgs ne = (EquippedModuleChangesEventArgs)e;
            if (ne == null)
                return;

            if (moduleEquipInfoUIID != null)
            {
                GameEntry.UI.CloseUIForm((int)moduleEquipInfoUIID);
            }
            moduleEquipInfoUIID = null;

            if (moduleInfoUIID != null)
            {
                GameEntry.UI.CloseUIForm((int)moduleInfoUIID);
            }
            moduleInfoUIID = null;
        }
        public void OnAchievementPoPUp(object sender, GameEventArgs e)
        {
            AchievementPopUpEventArgs ne = (AchievementPopUpEventArgs)e;
            if (ne == null)
                return;
            if (ne.Type == Constant.Type.UI_OPEN)
            {
                achievementUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UIAchievementPopUp);
            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (achievementUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)achievementUIID);
                }
                achievementUIID = null;
            }
        }
    }
}

