using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;

namespace ETLG
{
    public class VerticalFire : FsmState<BossEnemyAttack>
    {
        private float fireRate;
        private int fireRound;
        private GameObject bulletPrefab;
        private Transform[] bulletSpawnPositions;
        
        private float elapsedTime = 0;
        private int bulletCnt = 0;
        private BossEnemyAttack bossEnemyAttack;

        public VerticalFire(BossEnemyAttack bossEnemyAttack)
        {
            this.bossEnemyAttack = bossEnemyAttack;
            this.fireRate = bossEnemyAttack.verticalFireRate;
            this.fireRound = bossEnemyAttack.verticalFireRound;
            this.bulletPrefab = bossEnemyAttack.bulletPrefab;
            this.bulletSpawnPositions = bossEnemyAttack.bulletSpawnPositions;
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

            if (elapsedTime < fireRate)
            {
                elapsedTime += elapseSeconds;
            }
            else
            {
                elapsedTime = 0f;
                foreach (var spawnPoint in bulletSpawnPositions)
                {
                    GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, spawnPoint.position, spawnPoint.rotation, 
                                                                ObjectPoolManager.PoolType.GameObject);
                    InitBossEnemyBullet(bullet.GetComponent<Bullet>(), new Vector3(0, 0, -1));
                    bulletCnt++;
                }
            }
            if (bulletCnt >= fireRound * bulletSpawnPositions.Length)
            {
                ChangeState<FanFire>(fsm);
            }
        }

        protected override void OnLeave(IFsm<BossEnemyAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            elapsedTime = 0;
            bulletCnt = 0;
        }

        protected override void OnDestroy(IFsm<BossEnemyAttack> fsm)
        {
            base.OnDestroy(fsm);
        }

        private void InitBossEnemyBullet(Bullet bullet, Vector3 direction)
        {
            bullet.damage = 20; // (int) GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.CloudComputingBoss).Firepower;
            bullet.flyingDirection = direction;
            bullet.flyingSpeed = 1000;
        }
    }
}
