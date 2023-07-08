using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace ETLG
{
    public class FireWall : FsmState<SpaceshipAttack>
    {
        private PlayerHealth health;
        private float recoverRate = 0.01f;
        private int recoveryAmount = 50;
        private int recoveryCnt = 0;
        private float timeElapsed = 0;
        private bool changeToRespawnState;

        public FireWall(PlayerHealth health)
        {
            this.health = health;
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
            this.timeElapsed = 0f;
            this.recoveryCnt = 0;
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

            if (health.CurrentShield < health.MaxShield && recoveryCnt < recoveryAmount)
            {
                if (timeElapsed < recoverRate)
                {
                    timeElapsed += elapseSeconds;
                }
                else
                {
                    health.CurrentShield = Mathf.Min(health.MaxShield, health.CurrentShield + 5);
                    timeElapsed = 0f;
                    recoveryCnt += 5;
                }
            }
            if (recoveryCnt >= recoveryAmount || health.CurrentShield >= health.MaxShield)
            {
                ChangeState<DefaultSkill>(fsm);
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

