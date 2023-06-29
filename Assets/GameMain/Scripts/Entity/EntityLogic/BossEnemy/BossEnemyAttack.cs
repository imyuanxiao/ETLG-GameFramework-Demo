using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class BossEnemyAttack : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform middleBulletSpawnPosition;
        [SerializeField] private Transform[] bulletSpawnPositions;
        private float verticalFireRate;
        private float fanFireRate;
        private int fanBulletNum;
        private int fanFireRound;
        private float fanBulletAngel;
        private float spiralFireRate;

        private void OnEnable() 
        {
            verticalFireRate = 1.5f;
            fanFireRate = 2f;
            fanBulletNum = 9;
            fanFireRound = 5;
            fanBulletAngel = 20f;
            spiralFireRate = 0.2f;
            // VerticalFire();
            // FanFire();
            // SpiralFire();
            StartCoroutine(Fire());
        }

        private IEnumerator Fire() 
        {
            yield return VerticalFireCoroutine();
            yield return new WaitForSeconds(3f);
            yield return FanFireCoroutine();
            yield return new WaitForSeconds(3f);
            yield return SpiralFireCoroutine();
        }

        private void VerticalFire()
        {
            StartCoroutine(VerticalFireCoroutine());
        }

        private void FanFire()
        {
            StartCoroutine(FanFireCoroutine());
        }

        private void SpiralFire()
        {
            StartCoroutine(SpiralFireCoroutine());
        }

        private IEnumerator VerticalFireCoroutine()
        {
            int bulletNum = 20;
            float elapsedTime = 0;

            while (bulletNum > 0)
            {
                if (elapsedTime < verticalFireRate)
                {
                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    foreach (var spawnPoint in bulletSpawnPositions)
                    {
                        GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, spawnPoint.position, spawnPoint.rotation, 
                                                                ObjectPoolManager.PoolType.GameObject);
                        InitBossEnemyBullet(bullet.GetComponent<Bullet>(), new Vector3(0, 0, -1));
                        bulletNum--;
                    }
                    elapsedTime = 0f;
                }
                yield return null;
            }
        }

        private IEnumerator FanFireCoroutine()
        {
            int bulletNum = fanBulletNum * fanFireRound;
            float elapsedTime = 0;

            while (bulletNum > 0)
            {
                if (elapsedTime < fanFireRate)
                {
                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    Transform spawnPoint = middleBulletSpawnPosition;
                    for (int i=0; i < fanBulletNum; i++)
                    {
                        float angle = 180f - fanBulletAngel * (int)(fanBulletNum / 2) + fanBulletAngel * i;
                        GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, spawnPoint.position, spawnPoint.rotation, 
                                                                ObjectPoolManager.PoolType.GameObject);
                        bulletNum--;
                        bullet.transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
                        InitBossEnemyBullet(bullet.GetComponent<Bullet>(), bullet.transform.forward);
                    }
                    
                    elapsedTime = 0f;
                }
                yield return null;
            }
        }

        private IEnumerator SpiralFireCoroutine()
        {
            int bulletNum = 40;
            float elapsedTime = 0;
            int i = 0;
            bool changeDirection = false;

            while (bulletNum > 0)
            {
                if (elapsedTime < spiralFireRate)
                {
                    elapsedTime += Time.deltaTime;
                }
                else 
                {
                    Transform spawnPoint = middleBulletSpawnPosition;
                    float angle = 180f - fanBulletAngel * (int)(fanBulletNum / 2) + fanBulletAngel * i;
                    GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, spawnPoint.position, spawnPoint.rotation, 
                                                                ObjectPoolManager.PoolType.GameObject);
                    bulletNum--;
                    bullet.transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
                    InitBossEnemyBullet(bullet.GetComponent<Bullet>(), bullet.transform.forward);
                    elapsedTime = 0f;
                    if (i == fanBulletNum) { changeDirection = true; }
                    if (i == 0) { changeDirection = false; }

                    if (changeDirection) { i--; }
                    else { i++; }
                }
                yield return null;
            }
        }

        private void InitBossEnemyBullet(Bullet bullet, Vector3 direction)
        {
            bullet.damage = 20;
            bullet.flyingDirection = direction;
            bullet.flyingSpeed = 1000;
        }

        private void OnDisable() 
        {
            StopAllCoroutines();    
        }
    }
}
