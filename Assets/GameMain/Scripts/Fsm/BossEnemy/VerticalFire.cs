using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

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

        private bool changeToCriticalHit = false;
        private bool changeToEnemyRespawn = false;
        private bool changeToInstantMove = false;

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
                    bossEnemyAttack.InitBossEnemyBullet(bullet.GetComponent<Bullet>(), new Vector3(0, 0, -1));
                    GameEntry.Sound.PlaySound(EnumSound.HandGun2);
                    bulletCnt++;
                }
            }
            if (bulletCnt >= fireRound * bulletSpawnPositions.Length)
            {
                ChangeState<FanFire>(fsm);
            }

            ChangeToSkillState(fsm);
        }

        private void ChangeToSkillState(IFsm<BossEnemyAttack> fsm)
        {
            if (changeToCriticalHit)
            {
                fsm.SetData<VarString>("returnState", "FanFire");
                ChangeState<CriticalHit>(fsm);
            }
            else if (changeToEnemyRespawn)
            {
                ChangeState<EnemyRespawn>(fsm);
            }
            else if (changeToInstantMove)
            {
                fsm.SetData<VarString>("returnState", "FanFire");
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
        }

        protected override void OnDestroy(IFsm<BossEnemyAttack> fsm)
        {
            base.OnDestroy(fsm);
        }
    }
}
