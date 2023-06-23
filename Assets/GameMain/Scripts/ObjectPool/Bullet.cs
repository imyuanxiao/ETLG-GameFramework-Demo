using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float destoryTime = 2;
        [SerializeField] private Vector3 flyingDirection;

        public int damage;

        private Rigidbody rb;
        private ProjectileData bulletData;

        private void Awake() 
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnEnable() 
        {
            bulletData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int)EnumEntity.Bullet);
            // damage = (int) bulletData.Damage;
            StartCoroutine(ReturnToPoolAfterTime());
        }

        private void Update() 
        {
            rb.velocity = flyingDirection * bulletData.Speed * Time.deltaTime * 1000;
        }



        private IEnumerator ReturnToPoolAfterTime()
        {
            float elapsedTime = 0f;
            while (elapsedTime < destoryTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
