using System;
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
    public class ProcedureBattle : ProcedureBase
    {
        private ProcedureOwner procedureOwner;
        private bool changeScene = false;
        private Dictionary<string, string> battleData;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(EnterBattleEventArgs.EventId, OnEnterBattle);
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);

            this.procedureOwner = procedureOwner;
            this.changeScene = false;
            string battleType = procedureOwner.GetData<VarString>("BattleType");
            string bossType = procedureOwner.GetData<VarString>("BossType");
            int accuracy = procedureOwner.GetData<VarInt32>("Accuracy");

            if (battleType == "IntermidateBattle")
            {
                BattleManager.Instance.bossType = procedureOwner.GetData<VarString>("BossType");
            }

            battleData = new Dictionary<string, string>();
            battleData.Add("BattleType", battleType);
            battleData.Add("BossType", bossType);
            battleData.Add("Accuracy", accuracy.ToString());

            GameEntry.Data.GetData<DataTutorial>().OpenGroupTutorials(Constant.Type.TUTORIAL_BATTLE);

            // show battle introduction UI, pass in the battle type and boss type
            if (!GameEntry.UI.HasUIForm(EnumUIForm.UITutorialForm))
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UIBattleIntro, battleData);
            }

            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);
        }

        // called when Player clicked skip button on UIBattleIntro
        private void OnChangeScene(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne == null)
                return;

            changeScene = true;
            procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.SceneId);
        }

        // called when player clicked continue button on UIBattleIntro
        private void OnEnterBattle(object sender, GameEventArgs e)
        {
            EnterBattleEventArgs ne = (EnterBattleEventArgs) e;
            if (ne == null)
                Log.Error("Invalid Event [EnterBattleEventArgs]");
            
            switch (ne.BattleType)
            {
                case "BasicBattle":
                    ChangeState<ProcedureBasicBattle>(procedureOwner);
                    break;
                case "IntermidateBattle":
                    ChangeState<ProcedureIntermidateBattle>(procedureOwner);
                    break;
                case "FinalBattle":
                    ChangeState<ProcedureFinalBattle>(procedureOwner);
                    break;
                default:
                    Log.Error("No Procedure Named + [" + ne.BattleType + "]");
                    break;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            if (changeScene)
            {
                ChangeState<ProcedureLoadingScene>(procedureOwner);
            }

            if (!GameEntry.UI.HasUIForm(EnumUIForm.UITutorialForm) && !GameEntry.UI.HasUIForm(EnumUIForm.UIBattleIntro))
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UIBattleIntro, battleData);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            battleData = null;
            GameEntry.Event.Unsubscribe(EnterBattleEventArgs.EventId, OnEnterBattle);
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);

            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.Sound.StopMusic();
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}

