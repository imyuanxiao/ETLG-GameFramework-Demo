using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Fsm;
using ETLG.Data;

namespace ETLG
{
    public class SpaceshipAttack : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private GameObject IonBeam;
        [SerializeField] private GameObject Railgun;

        [SerializeField] private Transform bulletSpawnPosition;

        private SpaceshipController controller;
        private IFsm<SpaceshipAttack> m_Fsm = null;

        private void Awake() 
        {
            controller = GetComponent<SpaceshipController>();
        }

        private void OnEnable() 
        {
            m_Fsm = GameEntry.Fsm.CreateFsm("PlayerSpaceshipBattleSkill", this, new DefaultSkill(), 
                                                                                new CloudComputing(), 
                                                                                new ElectronicWarfare(),
                                                                                new Medicalsupport(GetComponent<PlayerHealth>()));
            m_Fsm.Start<DefaultSkill>();
        }

        private void Update() 
        {
            if (controller.FireInput)
            {
                GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
                InitPlayerBullet(bullet.GetComponent<Bullet>());
            }
        }

        private void InitPlayerBullet(Bullet bullet)
        {
            ProjectileData bulletData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int)EnumEntity.Bullet);
            PlayerCalculatedSpaceshipData spaceshipData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData;

            bullet.damage = (int) spaceshipData.Firepower + (int) bulletData.Damage;
            bullet.flyingDirection = new Vector3(0, 0, 1);
            bullet.flyingSpeed = bulletData.Speed * 1000;
        }

        private void InitPlayerMissile()
        {
            ProjectileData missileData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int)EnumEntity.Missile);
            PlayerCalculatedSpaceshipData spaceshipData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData;
        }

        private void InitPlayerIonBeam()
        {
            ProjectileData ionBeamData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int)EnumEntity.IonBeam);
            PlayerCalculatedSpaceshipData spaceshipData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData;
        }

        private void OnDisable() {
            GameEntry.Fsm.DestroyFsm<SpaceshipAttack>("PlayerSpaceshipBattleSkill");
        }
    }
}
