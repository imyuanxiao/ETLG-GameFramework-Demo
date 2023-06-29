using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
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

        private void Awake() 
        {
            controller = GetComponent<SpaceshipController>();
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
            SpaceshipData spaceshipData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship;

            bullet.damage = (int) spaceshipData.Firepower + (int) bulletData.Damage;
            bullet.flyingDirection = new Vector3(0, 0, 1);
            bullet.flyingSpeed = bulletData.Speed * 1000;
        }

        private void InitPlayerMissile()
        {
            ProjectileData missileData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int)EnumEntity.Missile);
            SpaceshipData spaceshipData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship;
        }

        private void InitPlayerIonBeam()
        {
            ProjectileData ionBeamData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int)EnumEntity.IonBeam);
            SpaceshipData spaceshipData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship;
        }
    }
}
