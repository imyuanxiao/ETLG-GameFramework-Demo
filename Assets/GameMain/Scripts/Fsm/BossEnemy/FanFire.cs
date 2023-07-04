using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using ETLG.Data;
using GameFramework.Event;
using System;

namespace ETLG
{
    public class FanFire : FsmState<BossEnemyAttack>
    {
        private BossEnemyAttack bossEnemyAttack;
        private float fireRate;
        private int fireRound;
        private int bulletNum;  // how many bullets will be fired per round
        float bulletAngle;  // angle between every two nearest bullets
        private GameObject bulletPrefab;
        private Transform bulletSpawnPosition;

        private float elapsedTime = 0;
        private int fireRoundCnt = 0;

        private bool changeToCriticalHit = false;

        public FanFire(BossEnemyAttack bossEnemyAttack)
        {
            this.bossEnemyAttack = bossEnemyAttack;
            this.fireRate = bossEnemyAttack.fanFireRate;
            this.fireRound = bossEnemyAttack.fanFireRound;
            this.bulletNum = bossEnemyAttack.fanBulletNum;
            this.bulletAngle = bossEnemyAttack.fanBulletAngle;
            this.bulletPrefab = bossEnemyAttack.bulletPrefab;
            this.bulletSpawnPosition = bossEnemyAttack.middleBulletSpawnPosition;
            Rigidbody rb = bossEnemyAttack.gameObject.GetComponent<Rigidbody>();
            Debug.Log("Rigidbody : " + (rb != null).ToString());
        }

        protected override void OnInit(IFsm<BossEnemyAttack> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<BossEnemyAttack> fsm)
        {
            base.OnEnter(fsm);
            changeToCriticalHit = false;
            // listen for critical hit event
            GameEntry.Event.Subscribe(EnemyCriticalHitEventArgs.EventId, OnCriticalHit);
        }

        // if next attack is critical hit, then change to CriticalHit State
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

            if (elapsedTime < fireRate)
            {
                elapsedTime += elapseSeconds;
            }
            else 
            {
                elapsedTime = 0;
                for (int i=0; i < bulletNum; i++)
                {
                    float angle = 180f - bulletAngle * (int)(bulletNum / 2) + bulletAngle * i;
                    GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation, 
                                                                ObjectPoolManager.PoolType.GameObject);
                    bullet.transform.eulerAngles = new Vector3(0, angle, 0);
                    bossEnemyAttack.InitBossEnemyBullet(bullet.GetComponent<Bullet>(), bullet.transform.forward);
                }
                fireRoundCnt++;
            }
            if (fireRoundCnt >= fireRound)
            {
                ChangeState<SpiralFire>(fsm);
            }

            if (changeToCriticalHit)
            {
                ChangeState<CriticalHit>(fsm);
            }
        }

        protected override void OnLeave(IFsm<BossEnemyAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            GameEntry.Event.Unsubscribe(EnemyCriticalHitEventArgs.EventId, OnCriticalHit);

            fireRoundCnt = 0;
            elapsedTime = 0;
        }

        protected override void OnDestroy(IFsm<BossEnemyAttack> fsm)
        {
            base.OnDestroy(fsm);
        }
    }
}
