using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;

namespace ETLG
{
    public class BossEnemyAttack : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] public GameObject bulletPrefab;
        [SerializeField] public GameObject laserPrefab;
        [SerializeField] public Transform middleBulletSpawnPosition;
        [SerializeField] public Transform[] bulletSpawnPositions;
        public Rigidbody rb;
        private IFsm<BossEnemyAttack> m_Fsm = null;

        [Header("Vertical Fire Settings")]
        [SerializeField] public float verticalFireRate = 1.5f;
        [SerializeField] public int verticalFireRound = 5;

        [Header("Fan Fire Settings")]
        [SerializeField] public float fanFireRate = 2f;
        [SerializeField] public int fanBulletNum = 9;
        [SerializeField] public int fanFireRound = 5;
        [SerializeField] public float fanBulletAngle = 20f;

        [Header("Spiral Fire Settings")]
        [SerializeField] public float spiralFireRate = 0.2f;
        [SerializeField] public int spiralBulletNum = 9;
        [SerializeField] public int spiralBulletRound = 5;
        [SerializeField] public float spiralBulletAngle = 20f;

        private void Awake() 
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnEnable() 
        {
            m_Fsm = GameEntry.Fsm.CreateFsm("BossEnemyAttackFsm", this, 
                new VerticalFire(GetComponent<BossEnemyAttack>()),  //(verticalFireRate, verticalFireRound, bulletPrefab, bulletSpawnPositions), 
                new FanFire(GetComponent<BossEnemyAttack>()), // (fanFireRate, fanFireRound, fanBulletNum, fanBulletAngle, bulletPrefab, middleBulletSpawnPosition), 
                new SpiralFire(GetComponent<BossEnemyAttack>()),
                new CriticalHit(GetComponent<BossEnemyAttack>()));
            m_Fsm.Start<VerticalFire>();
        }

        public void InitBossEnemyBullet(Bullet bullet, Vector3 direction)
        {
            bullet.damage = 20; // (int) GameEntry.Data.GetData<DataBossEnemy>().GetBossEnemyData((int) EnumEntity.CloudComputingBoss).Firepower;
            bullet.flyingDirection = direction;
            bullet.flyingSpeed = 1000;
        }

        public void InitBossEnemyLaser(Laser laser, Vector3 direction)
        {
            laser.damage = 200;
            laser.flyingDirection = direction;
            laser.flyingSpeed = 4000;
        }

        private void OnDisable() 
        {
            StopAllCoroutines();
            GameEntry.Fsm.DestroyFsm<BossEnemyAttack>("BossEnemyAttackFsm");
        }
    }
}
