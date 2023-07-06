using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    // TODO : need to be fixed
    public class Missile : Projectile
    {
        [HideInInspector] public Transform target;


        protected override void OnEnable() 
        {
            base.OnEnable();
        }

        private void Update() 
        {
            if (target != null && target.gameObject.activeSelf == true)
            {
                transform.LookAt(target);
            }
            rb.velocity = transform.forward * flyingSpeed * Time.deltaTime;
            if (IsOffScreen())
            {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}
