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
            GameEntry.Event.Subscribe(PlanetSceneClickEventArgs.EventId, OnPlanetSceneClick);
            GameEntry.Event.Subscribe(NPCDialogOpenEventArgs.EventId, OnNPCDialogOpen);


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
                procedureOwner.SetData<VarString>("BossType", "CloudComputing");
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
            GameEntry.Event.Unsubscribe(PlanetSceneClickEventArgs.EventId, OnPlanetSceneClick);
            GameEntry.Event.Unsubscribe(NPCDialogOpenEventArgs.EventId, OnNPCDialogOpen);


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

        private void OnPlanetSceneClick(object sender, GameEventArgs e)
        {

            PlanetSceneClickEventArgs ne = (PlanetSceneClickEventArgs)e;
            if (ne == null)
                return;

            
            // 打开 planetScene UI
            GameEntry.UI.OpenUIForm(EnumUIForm.UIPlanetSceneForm);

        }
        private void OnNPCDialogOpen(object sender, GameEventArgs e)
        {

            NPCDialogOpenEventArgs ne = (NPCDialogOpenEventArgs)e;
            if (ne == null)
                return;

            // 根据 Type值打开不同 UI
            if(ne.Type == Constant.Event.NPC_TALK)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UINPCDialogForm);
            }

        }


    }
}

