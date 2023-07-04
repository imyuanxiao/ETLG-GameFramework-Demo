using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using GameFramework.Event;

namespace ETLG
{
    public class EnemyRespawn : FsmState<BossEnemyAttack>
    {
        private BossEnemyHealth bossEnemyHealth;

        public EnemyRespawn(BossEnemyHealth bossEnemyHealth)
        {
            this.bossEnemyHealth = bossEnemyHealth;
        }

        protected override void OnInit(IFsm<BossEnemyAttack> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<BossEnemyAttack> fsm)
        {
            base.OnEnter(fsm);
            bossEnemyHealth.CurrentHealth = bossEnemyHealth.MaxHealth;
            bossEnemyHealth.StopAllCoroutines();
            bossEnemyHealth.StartCoroutine(bossEnemyHealth.CheckDeath());
        }

        protected override void OnUpdate(IFsm<BossEnemyAttack> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            ChangeState<VerticalFire>(fsm);
        }

        protected override void OnLeave(IFsm<BossEnemyAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        protected override void OnDestroy(IFsm<BossEnemyAttack> fsm)
        {
            base.OnDestroy(fsm);
        }
    }
}
