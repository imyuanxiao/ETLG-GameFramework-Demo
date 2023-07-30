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


        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(SkillInfoUIChangeEventArgs.EventId, OnSkillInfoUIChange);
            GameEntry.Event.Subscribe(TipUIChangeEventArgs.EventId, OnTipUIChange);
            GameEntry.Event.Subscribe(SpaceshipChangeEventArgs.EventId, OnSpaceshipChange);

            this.procedureOwner = procedureOwner;
            this.changeScene = false;

            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);

            GameEntry.UI.OpenUIForm(EnumUIForm.UISpaceshipSelectForm);


            // opten tutorial

            


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
            GameEntry.Event.Unsubscribe(TipUIChangeEventArgs.EventId, OnTipUIChange);
            GameEntry.Event.Unsubscribe(SpaceshipChangeEventArgs.EventId, OnSpaceshipChange);
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

        private void OnSkillInfoUIChange(object sender, GameEventArgs e)
        {
            SkillInfoUIChangeEventArgs ne = (SkillInfoUIChangeEventArgs)e;
            if (ne == null)
                return;

            if (GameEntry.UI.HasUIForm(EnumUIForm.UISkillInfoForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UISkillInfoForm));
            }

            if (ne.Type == Constant.Type.UI_OPEN)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UISkillInfoForm);
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
                GameEntry.UI.OpenUIForm(EnumUIForm.UITipForm);
            }

        }

        private void OnSpaceshipChange(object sender, GameEventArgs e)
        {
            SpaceshipChangeEventArgs ne = (SpaceshipChangeEventArgs)e;
            if (ne == null)
                return;

            GameEntry.UI.OpenUIForm(EnumUIForm.UISpaceshipSelectForm);

        }


    }
}

