using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace ETLG
{
    public class SpiralFire : FsmState<BossEnemyAttack>
    {
        private BossEnemyAttack bossEnemyAttack;
        private GameObject bulletPrefab;
        private float fireRate;
        private int bulletNum;
        private int bulletRound;
        private float bulletAngle;

        private float elapsedTime = 0;
        private int bulletCnt = 0;
        private bool changeDirection = false;
        private int totalBulletNum;

        private bool changeToCriticalHit = false;
        private bool changeToEnemyRespawn = false;
        private bool changeToInstantMove = false;

        public SpiralFire(BossEnemyAttack bossEnemyAttack)
        {
            this.bossEnemyAttack = bossEnemyAttack;
            this.bulletNum = bossEnemyAttack.spiralBulletNum;
            this.fireRate = bossEnemyAttack.spiralFireRate;
            this.bulletRound = bossEnemyAttack.spiralBulletRound;
            this.bulletAngle = bossEnemyAttack.spiralBulletAngle;
            this.totalBulletNum = this.bulletNum * this.bulletRound;
            this.bulletPrefab = bossEnemyAttack.bulletPrefab;
        }

        protected override void OnInit(IFsm<BossEnemyAttack> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<BossEnemyAttack> fsm)
        {
            base.OnEnter(fsm);

            changeToCriticalHit = false;
            changeToEnemyRespawn = false;
            changeToInstantMove = false;

            // listen for critical hit event
            GameEntry.Event.Subscribe(EnemyCriticalHitEventArgs.EventId, OnCriticalHit);
            GameEntry.Event.Subscribe(EnemyRespawnEventArgs.EventId, OnEnemyRespawn);
            GameEntry.Event.Subscribe(EnemyInstantMoveEventArgs.EventId, OnInstantMove);
        }

        private void OnInstantMove(object sender, GameEventArgs e)
        {
            EnemyInstantMoveEventArgs ne = (EnemyInstantMoveEventArgs) e;
            if (ne == null)
                return;
            changeToInstantMove = true;
        }

        private void OnEnemyRespawn(object sender, GameEventArgs e)
        {
            EnemyRespawnEventArgs ne = (EnemyRespawnEventArgs) e;
            if (ne == null)
            {
                Log.Error("Invalid Event : EnemyRespawnEventArgs");
            }
            changeToEnemyRespawn = true;
        }

        private void OnCriticalHit(object sender, GameEventArgs e)
        {
            EnemyCriticalHitEventArgs ne = (EnemyCriticalHitEventArgs) e;
            if (ne == null)
                return;
            changeToCriticalHit = true;
        }

        protected override void OnUpdate(IFsm<BossEnemyAttack> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (elapsedTime < bossEnemyAttack.spiralFireRate)
            {
                elapsedTime += elapseSeconds;
            }
            else
            {
                elapsedTime = 0;
                Transform spawnPoint = bossEnemyAttack.middleBulletSpawnPosition;
                float angle = 180f - bulletAngle * (int)(bulletNum / 2) + bulletAngle * bulletCnt;
                GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, spawnPoint.position, spawnPoint.rotation, 
                                                                ObjectPoolManager.PoolType.GameObject);
                totalBulletNum--;
                bullet.transform.eulerAngles = new Vector3(0, angle, 0);
                bossEnemyAttack.InitBossEnemyBullet(bullet.GetComponent<Bullet>(), bullet.transform.forward);

                if (bulletCnt == bulletNum) { changeDirection = true; }
                if (bulletCnt == 0) { changeDirection = false; }

                if (changeDirection) { bulletCnt--; }
                else { bulletCnt++; }
            }
            if (totalBulletNum <= 0)
            {
                ChangeState<VerticalFire>(fsm);
            }

            ChangeToSkillState(fsm);
        }

        private void ChangeToSkillState(IFsm<BossEnemyAttack> fsm)
        {
            if (changeToCriticalHit)
            {
                fsm.SetData<VarString>("returnState", "VerticalFire");
                ChangeState<CriticalHit>(fsm);
            }
            else if (changeToEnemyRespawn)
            {
                ChangeState<EnemyRespawn>(fsm);
            }
            else if (changeToInstantMove)
            {
                fsm.SetData<VarString>("returnState", "VerticalFire");
                ChangeState<InstantMove>(fsm);
            }
        }

        protected override void OnLeave(IFsm<BossEnemyAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            GameEntry.Event.Unsubscribe(EnemyCriticalHitEventArgs.EventId, OnCriticalHit);
            GameEntry.Event.Unsubscribe(EnemyRespawnEventArgs.EventId, OnEnemyRespawn);
            GameEntry.Event.Unsubscribe(EnemyInstantMoveEventArgs.EventId, OnInstantMove);
            elapsedTime = 0;
            bulletCnt = 0;
            this.totalBulletNum = this.bulletNum * this.bulletRound;
        }

        protected override void OnDestroy(IFsm<BossEnemyAttack> fsm)
        {
            base.OnDestroy(fsm);
        }
    }
}
