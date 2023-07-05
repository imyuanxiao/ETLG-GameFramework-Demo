using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class InstantMove : FsmState<BossEnemyAttack>
    {
        private GameObject enemyObj;
        private Vector3 originalPos;
        private int currentRandomPointIdx = -1;

        public InstantMove(GameObject enemyObj)
        {
            this.enemyObj = enemyObj;
            this.originalPos = enemyObj.transform.position;
        }

        protected override void OnInit(IFsm<BossEnemyAttack> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<BossEnemyAttack> fsm)
        {
            base.OnEnter(fsm);
            this.originalPos = enemyObj.transform.position;
            this.currentRandomPointIdx = -1;

            Vector3 randomPos = GetRandomPoint();
            enemyObj.transform.position = randomPos;
        }

        protected override void OnUpdate(IFsm<BossEnemyAttack> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            ReturnToState(fsm);
        }

        protected override void OnLeave(IFsm<BossEnemyAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            this.originalPos = Vector3.zero;
            this.currentRandomPointIdx = -1;
        }

        protected override void OnDestroy(IFsm<BossEnemyAttack> fsm)
        {
            base.OnDestroy(fsm);
        }

        private Vector3 GetRandomPoint()
        {
            while (true)
            {
                int r = Random.Range(0, BattleManager.Instance.instantMovePoints.Length);
                if (r != currentRandomPointIdx)
                {
                    currentRandomPointIdx = r;
                    break;
                }
            }
            return BattleManager.Instance.instantMovePoints[currentRandomPointIdx].position;
        }

        private void ReturnToState(IFsm<BossEnemyAttack> fsm)
        {
            switch (fsm.GetData<VarString>("returnState"))
            {
                case "FanFire":
                    ChangeState<FanFire>(fsm);
                    break;
                case "SpiralFire":
                    ChangeState<SpiralFire>(fsm);
                    break;
                case "VerticalFire":
                    ChangeState<VerticalFire>(fsm);
                    break;
            }
        }
    }
}
