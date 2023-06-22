using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;

namespace ETLG
{
    public class FireWall : FsmState<EntitySpaceship>
    {
        protected override void OnInit(IFsm<EntitySpaceship> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<EntitySpaceship> fsm)
        {
            base.OnEnter(fsm);
        }

        protected override void OnUpdate(IFsm<EntitySpaceship> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<EntitySpaceship> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        protected override void OnDestroy(IFsm<EntitySpaceship> fsm)
        {
            base.OnDestroy(fsm);
        }
    }

}

