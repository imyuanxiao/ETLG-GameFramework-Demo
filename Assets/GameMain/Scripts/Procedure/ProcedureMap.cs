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
    public class ProcedureMap : ProcedureBase
    {
        private ProcedureOwner procedureOwner;
        private bool changeScene = false;


        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Debug("进入 ProcedureMap 流程");
            base.OnEnter(procedureOwner);

            // 订阅事件
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            
            this.procedureOwner = procedureOwner;
            this.changeScene = false;

            // 播放BGM
            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);

            // 打开UI
            Log.Debug("此处应打开 Map 场景界面");
            GameEntry.UI.OpenUIForm(EnumUIForm.UIMapInfoForm);

        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);


            if (changeScene)
            {
                ChangeState<ProcedureLoadingScene>(procedureOwner);
            }

            // Switch to battle scene and battle procedure (for test purpose only)
            //   1. switch to basic battle
            if (Input.GetKeyDown(KeyCode.C))
            {
                procedureOwner.SetData<VarString>("BattleType", "BasicBattle");
                GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Battle")));
            }
            //  2. switch to intermidate battle (mini boss battle)
            else if (Input.GetKeyDown(KeyCode.V))
            {
                procedureOwner.SetData<VarString>("BattleType", "IntermidateBattle");
                GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Battle")));
            }
            //  3. switch to final boss battle
            else if (Input.GetKeyDown(KeyCode.B))
            {
                procedureOwner.SetData<VarString>("BattleType", "FinalBattle");
                GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Battle")));
            }

        }


        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            // 取消订阅事件
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);

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

    }
}

