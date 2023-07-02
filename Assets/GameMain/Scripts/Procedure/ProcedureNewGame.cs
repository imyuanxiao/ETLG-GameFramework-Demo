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

        private int? skillInfoUIID;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Debug("进入 ProcedureNewGame 流程");
            base.OnEnter(procedureOwner);

            // 订阅事件
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);

            GameEntry.Event.Subscribe(SkillInfoOpenEventArgs.EventId, OnSkillInfoOpen);
            GameEntry.Event.Subscribe(SkillInfoCloseEventArgs.EventId, OnSkillInfoClose);

            this.procedureOwner = procedureOwner;
            this.changeScene = false;
            this.skillInfoUIID = null;

            // 播放BGM
            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);

            // 打开UI
            Log.Debug("打开选择飞船界面");
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

            // 取消订阅事件
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);

            GameEntry.Event.Unsubscribe(SkillInfoOpenEventArgs.EventId, OnSkillInfoOpen);
            GameEntry.Event.Unsubscribe(SkillInfoCloseEventArgs.EventId, OnSkillInfoClose);


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

        private void OnSkillInfoOpen(object sender, GameEventArgs e)
        {
            SkillInfoOpenEventArgs ne = (SkillInfoOpenEventArgs)e;
            if (ne == null)
                return;

            skillInfoUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UISkillInfoForm);

        }

        private void OnSkillInfoClose(object sender, GameEventArgs e)
        {
            SkillInfoCloseEventArgs ne = (SkillInfoCloseEventArgs)e;
            if (ne == null)
                return;

            if (skillInfoUIID != null)
            {
                GameEntry.UI.CloseUIForm((int)skillInfoUIID);
            }

            skillInfoUIID = null;

        }


    }
}

