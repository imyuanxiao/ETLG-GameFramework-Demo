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
    public class ProcedureNewGame : ProcedureBase
    {


        private ProcedureOwner procedureOwner;
        private bool changeScene = false;

        private DataPlayer dataPlayer;
        private int? skillInfoUIID;
        private int? tipUIID;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            // 订阅事件
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);

            GameEntry.Event.Subscribe(SkillInfoUIChangeEventArgs.EventId, OnSkillInfoUIChange);
            //GameEntry.Event.Subscribe(SkillInfoCloseEventArgs.EventId, OnSkillInfoClose);

            GameEntry.Event.Subscribe(TipUIChangeEventArgs.EventId, OnTipUIChange);
           // GameEntry.Event.Subscribe(TipCloseEventArgs.EventId, OnTipClose);

            GameEntry.Event.Subscribe(SpaceshipChangeEventArgs.EventId, OnSpaceshipChange);


            this.procedureOwner = procedureOwner;
            this.changeScene = false;

            this.skillInfoUIID = null;
            this.tipUIID = null;


            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);

            GameEntry.UI.OpenUIForm(EnumUIForm.UISpaceshipSelectForm);

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

            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);

            GameEntry.Event.Unsubscribe(SkillInfoUIChangeEventArgs.EventId, OnSkillInfoUIChange);
           // GameEntry.Event.Unsubscribe(SkillInfoCloseEventArgs.EventId, OnSkillInfoClose);

            GameEntry.Event.Unsubscribe(TipUIChangeEventArgs.EventId, OnTipUIChange);
            //GameEntry.Event.Unsubscribe(TipCloseEventArgs.EventId, OnTipClose);

            GameEntry.Event.Unsubscribe(SpaceshipChangeEventArgs.EventId, OnSpaceshipChange);

            GameEntry.Sound.StopMusic();

            this.skillInfoUIID = null;
            this.tipUIID = null;

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

        private void OnSkillInfoUIChange(object sender, GameEventArgs e)
        {
            SkillInfoUIChangeEventArgs ne = (SkillInfoUIChangeEventArgs)e;
            if (ne == null)
                return;

            if(ne.Type == Constant.Type.UI_OPEN)
            {
                skillInfoUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UISkillInfoForm);
            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (skillInfoUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)skillInfoUIID);
                }

                skillInfoUIID = null;
            }

        }

/*        private void OnSkillInfoClose(object sender, GameEventArgs e)
        {
            SkillInfoCloseEventArgs ne = (SkillInfoCloseEventArgs)e;
            if (ne == null)
                return;

            if (skillInfoUIID != null)
            {
                GameEntry.UI.CloseUIForm((int)skillInfoUIID);
            }

            skillInfoUIID = null;

        }*/

        private void OnTipUIChange(object sender, GameEventArgs e)
        {
            TipUIChangeEventArgs ne = (TipUIChangeEventArgs)e;
            if (ne == null)
                return;

            if(ne.Type == Constant.Type.UI_OPEN)
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
/*
        private void OnTipClose(object sender, GameEventArgs e)
        {
            TipCloseEventArgs ne = (TipCloseEventArgs)e;
            if (ne == null)
                return;

            if (tipUIID != null)
            {
                GameEntry.UI.CloseUIForm((int)tipUIID);
            }

            tipUIID = null;

        }*/

        private void OnSpaceshipChange(object sender, GameEventArgs e)
        {
            SpaceshipChangeEventArgs ne = (SpaceshipChangeEventArgs)e;
            if (ne == null)
                return;

            GameEntry.UI.OpenUIForm(EnumUIForm.UISpaceshipSelectForm);

        }


    }
}

