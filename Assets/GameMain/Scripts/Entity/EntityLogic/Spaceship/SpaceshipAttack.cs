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
        [SerializeField] private GameObject ionBeamPrefab;
        [SerializeField] private GameObject railgunPrefab;

        [SerializeField] private Transform bulletSpawnPosition;

        private SpaceshipController controller;
        private IFsm<SpaceshipAttack> m_Fsm = null;

        private void Awake() 
        {
            controller = GetComponent<SpaceshipController>();
        }

        private void OnEnable() 
        {
            m_Fsm = GameEntry.Fsm.CreateFsm("PlayerSpaceshipBattleSkill", this, new DefaultSkill(GetComponent<SpaceshipSkill>()), 
                                                                                new CloudComputing(), 
                                                                                new ElectronicWarfare(),
                                                                                new Medicalsupport(GetComponent<PlayerHealth>()),
                                                                                new FireWall(GetComponent<PlayerHealth>()),
                                                                                new PlayerRespawn(GetComponent<PlayerHealth>()),
                                                                                new PlayerAIAssist(GetComponent<SpaceshipSkill>()));
            m_Fsm.Start<DefaultSkill>();
        }

        private void Update() 
        {
            if (controller.FireInput)
            {
                GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
                InitPlayerBullet(bullet.GetComponent<Bullet>());
                GameEntry.Sound.PlaySound(EnumSound.BulletImpact14);
            }
            // fire equiped weapon
            if (Input.GetKeyDown(KeyCode.E))
            {
                FireMissile();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                FireLaser();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                FireRailgun();
            }
        }

        private void FireMissile()
        {
            Transform target = null;
            if (GameEntry.Procedure.CurrentProcedure is ProcedureIntermidateBattle || GameEntry.Procedure.CurrentProcedure is ProcedureFinalBattle)
            {
                target = BattleManager.Instance.bossEnemyEntity.gameObject.transform;
                if (target != null)
                {
                    GameObject missile = ObjectPoolManager.SpawnObject(missilePrefab, bulletSpawnPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
                    InitPlayerMissile(missile.GetComponent<Missile>(), target);
                    GameEntry.Sound.PlaySound(EnumSound.Flaregun);
                }
            }
            else if (GameEntry.Procedure.CurrentProcedure is ProcedureBasicBattle)
            {
                target = FindObjectOfType<BasicEnemyController>()?.gameObject.transform;
                if (target != null)
                {
                    GameObject missile = ObjectPoolManager.SpawnObject(missilePrefab, bulletSpawnPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
                    InitPlayerMissile(missile.GetComponent<Missile>(), target);
                    GameEntry.Sound.PlaySound(EnumSound.Flaregun);
                }
            }
        }

        private void FireLaser()
        {
            Transform target = null;
            if (GameEntry.Procedure.CurrentProcedure is ProcedureIntermidateBattle || GameEntry.Procedure.CurrentProcedure is ProcedureFinalBattle)
            {
                target = BattleManager.Instance.bossEnemyEntity.gameObject.transform;
                if (target != null)
                {
                    GameObject laser = ObjectPoolManager.SpawnObject(laserPrefab, bulletSpawnPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
                    InitPlayerLaser(laser.GetComponent<Laser>(), target);
                    GameEntry.Sound.PlaySound(EnumSound.Flaregun);
                }
            }
            else if (GameEntry.Procedure.CurrentProcedure is ProcedureBasicBattle)
            {
                target = FindObjectOfType<BasicEnemyController>()?.gameObject.transform;
                if (target != null)
                {
                    GameObject laser = ObjectPoolManager.SpawnObject(laserPrefab, bulletSpawnPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
                    InitPlayerLaser(laser.GetComponent<Laser>(), target);
                    GameEntry.Sound.PlaySound(EnumSound.Flaregun);
                }
            }
        }

        private void FireRailgun()
        {
            for (int i=0; i < 3; i++)
            {
                GameObject railgun = ObjectPoolManager.SpawnObject(railgunPrefab, bulletSpawnPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
                
                float angle = 360f - 10 * (3 / 2) + 10 * i;
                railgun.transform.eulerAngles = new Vector3(0, angle, 0);
                InitPlayerRailgun(railgun.GetComponent<Railgun>());
                GameEntry.Sound.PlaySound(EnumSound.RailgunShot6);
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

        private void InitPlayerMissile(Missile missile, Transform target)
        {
            ProjectileData missileData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int)EnumEntity.Missile);
            PlayerCalculatedSpaceshipData spaceshipData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData;
            
            missile.target = target;
            missile.damage = (int) spaceshipData.Firepower + (int) missileData.Damage;
            missile.flyingDirection = missile.transform.forward;
            missile.flyingSpeed = missileData.Speed * 160;
        }

        private void InitPlayerLaser(Laser laser, Transform target)
        {
            ProjectileData laserData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int) EnumEntity.Laser);
            PlayerCalculatedSpaceshipData spaceshipData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData;

            laser.transform.LookAt(target);
            laser.damage = (int) spaceshipData.Firepower + (int) laserData.Damage;
            laser.flyingDirection = laser.transform.forward;
            laser.flyingSpeed = 4000;
        }

        private void InitPlayerRailgun(Railgun railgun)
        {
            ProjectileData railgunData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int) EnumEntity.Railgun);
            PlayerCalculatedSpaceshipData spaceshipData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData;
        
            railgun.damage = (int) spaceshipData.Firepower + (int) railgunData.Damage;
            railgun.flyingDirection = railgun.transform.forward;
            railgun.flyingSpeed = 4000;
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
