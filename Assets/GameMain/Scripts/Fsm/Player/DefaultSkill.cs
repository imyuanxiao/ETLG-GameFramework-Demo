using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace ETLG
{
    public class DefaultSkill : FsmState<SpaceshipAttack>
    {
        private bool changeToRespawnState = false;

        public DefaultSkill()
        {

        }

        protected override void OnInit(IFsm<SpaceshipAttack> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<SpaceshipAttack> fsm)
        {
            base.OnEnter(fsm);
            
            GameEntry.Event.Subscribe(PlayerRespawnEventArgs.EventId, OnPlayerRespawn);

            this.changeToRespawnState = false;
        }

        private void OnPlayerRespawn(object sender, GameEventArgs e)
        {
            PlayerRespawnEventArgs ne = (PlayerRespawnEventArgs) e;
            if (ne == null)
                Log.Error("Invalid event [PlayerRespawnEventArgs]");

            changeToRespawnState = true;
        }

        protected override void OnUpdate(IFsm<SpaceshipAttack> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (changeToRespawnState)
            {
                ChangeState<PlayerRespawn>(fsm);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeState<CloudComputing>(fsm);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeState<ElectronicWarfare>(fsm);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeState<Medicalsupport>(fsm);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeState<FireWall>(fsm);
            }

        }

        protected override void OnLeave(IFsm<SpaceshipAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            GameEntry.Event.Unsubscribe(PlayerRespawnEventArgs.EventId, OnPlayerRespawn);
        }

        protected override void OnDestroy(IFsm<SpaceshipAttack> fsm)
        {
            base.OnDestroy(fsm);
        }
    }
}
