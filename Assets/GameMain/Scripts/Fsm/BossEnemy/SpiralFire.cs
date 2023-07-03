using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;

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
        }

        protected override void OnLeave(IFsm<BossEnemyAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
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
