using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;

namespace ETLG
{
    public class PlayerRespawn : FsmState<SpaceshipAttack>
    {
        private PlayerHealth health;
        private int recoveryAmount;
        private int recoveryCnt;

        public PlayerRespawn(PlayerHealth health)
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
            this.recoveryCnt = 0;
            this.recoveryAmount = this.health.MaxHealth;
        }

        protected override void OnUpdate(IFsm<SpaceshipAttack> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (this.health.CurrentHealth < this.health.MaxHealth && this.recoveryCnt < this.recoveryAmount)
            {
                this.health.CurrentHealth = Mathf.Min(this.health.MaxHealth, this.health.CurrentHealth + 10);
                this.recoveryCnt += 10;
            }
            else
            {
                ChangeState<DefaultSkill>(fsm);
            }
        }

        protected override void OnLeave(IFsm<SpaceshipAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            this.recoveryCnt = 0;
        }

        protected override void OnDestroy(IFsm<SpaceshipAttack> fsm)
        {
            base.OnDestroy(fsm);
        }
    }
}
