using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

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
                ObjectPoolManager.SpawnObject(bulletPrefab, bulletSpawnPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            }
        }
    }
}
