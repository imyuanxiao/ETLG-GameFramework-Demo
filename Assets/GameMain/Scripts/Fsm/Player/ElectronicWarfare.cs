using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;

namespace ETLG
{
    public class ElectronicWarfare : FsmState<SpaceshipAttack>
    {
        private float lastingTime;
        private float timeElapsed = 0f;

        protected override void OnInit(IFsm<SpaceshipAttack> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<SpaceshipAttack> fsm)
        {
            base.OnEnter(fsm);
            lastingTime = 5f;
            if (BattleManager.Instance.bossEnemyEntity != null)
            {
                BattleManager.Instance.bossEnemyEntity.GetComponent<BossEnemyAttack>().enabled = false;
            }
        }

        protected override void OnUpdate(IFsm<SpaceshipAttack> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (timeElapsed < lastingTime)
            {
                timeElapsed += elapseSeconds;
            }
            else 
            {
                Debug.Log("Skill Finished");
                ChangeState<DefaultSkill>(fsm);
            }
        }

        protected override void OnLeave(IFsm<SpaceshipAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            if (BattleManager.Instance.bossEnemyEntity != null)
            {
                BattleManager.Instance.bossEnemyEntity.GetComponent<BossEnemyAttack>().enabled = true;
            }
            lastingTime = 5f;
            timeElapsed = 0f;
        }

        protected override void OnDestroy(IFsm<SpaceshipAttack> fsm)
        {
            base.OnDestroy(fsm);
        }
    }
}

