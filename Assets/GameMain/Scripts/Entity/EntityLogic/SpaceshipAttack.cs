using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.ObjectPool;

namespace ETLG
{
    public class SpaceshipAttack : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
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
