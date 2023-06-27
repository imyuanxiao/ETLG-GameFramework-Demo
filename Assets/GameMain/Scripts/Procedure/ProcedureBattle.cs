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
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            string battleType = procedureOwner.GetData<VarString>("BattleType");

            if (battleType == "IntermidateBattle")
            {
                BattleManager.Instance.bossType = procedureOwner.GetData<VarString>("BossType");
            }

            switch (battleType)
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
                    Log.Error("No Procedure Named + [" + battleType + "]");
                    break;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}

