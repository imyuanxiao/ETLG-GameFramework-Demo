using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class Missile : Projectile
    {
        [HideInInspector] public Transform target;
        [HideInInspector] public GameObject missileFlare;


        protected override void OnEnable() 
        {
            base.OnEnable();
            missileFlare = ObjectPoolManager.SpawnObject(BattleManager.Instance.missileFlarePrefab, transform.position, transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);
        }

        private void Update() 
        {
            if (target != null && target.gameObject.activeSelf == true)
            {
                transform.LookAt(target);
            }
            rb.velocity = transform.forward * flyingSpeed * Time.deltaTime;
            missileFlare.transform.position = transform.position;
            if (IsOffScreen())
            {
                ObjectPoolManager.ReturnObjectToPool(missileFlare);
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            ObjectPoolManager.ReturnObjectToPool(missileFlare);
        }
    }
}
