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
        private int difficulty;

        private void Awake() 
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnEnable() 
        {
            m_Fsm = GameEntry.Fsm.CreateFsm("BossEnemyAttackFsm", this, 
                new VerticalFire(GetComponent<BossEnemyAttack>()), 
                new FanFire(GetComponent<BossEnemyAttack>()), 
                new SpiralFire(GetComponent<BossEnemyAttack>()),
                new CriticalHit(GetComponent<BossEnemyAttack>()),
                new EnemyRespawn(GetComponent<BossEnemyHealth>()),
                new InstantMove(gameObject));
            m_Fsm.Start<VerticalFire>();
            this.difficulty = SaveManager.Instance.difficulty;
        }

        public void InitBossEnemyBullet(Bullet bullet, Vector3 direction)
        {
            bullet.damage = 20 + 5 * this.difficulty;
            bullet.flyingDirection = direction;
            bullet.flyingSpeed = 1000;
        }

        public void InitBossEnemyLaser(Laser laser, Vector3 direction)
        {
            laser.damage = 150 + 5 * this.difficulty;
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
