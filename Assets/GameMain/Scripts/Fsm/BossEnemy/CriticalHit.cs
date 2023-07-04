using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class CriticalHit : FsmState<BossEnemyAttack>
    {
        private BossEnemyAttack bossEnemyAttack;
        private GameObject laserPrefab;
        private Transform laserSpawnPos;
        private GameObject target;

        public CriticalHit(BossEnemyAttack bossEnemyAttack)
        {
            this.bossEnemyAttack = bossEnemyAttack;
        }

        protected override void OnInit(IFsm<BossEnemyAttack> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<BossEnemyAttack> fsm)
        {
            base.OnEnter(fsm);
            Debug.Log("Enter Critical Hit State");
            this.laserPrefab = bossEnemyAttack.laserPrefab;
            this.laserSpawnPos = bossEnemyAttack.middleBulletSpawnPosition;
            this.target = GameObject.FindWithTag("Player");
            if (target == null)
            {
                Log.Error("No Player Found");
            }
        }

        protected override void OnUpdate(IFsm<BossEnemyAttack> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            Vector3 direction = target.transform.position - bossEnemyAttack.gameObject.transform.position;
            GameObject laser = ObjectPoolManager.SpawnObject(laserPrefab, laserSpawnPos.position, laserSpawnPos.rotation, ObjectPoolManager.PoolType.GameObject);
            laser.transform.LookAt(target.transform);
            bossEnemyAttack.InitBossEnemyLaser(laser.GetComponent<Laser>(), laser.transform.forward);
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
