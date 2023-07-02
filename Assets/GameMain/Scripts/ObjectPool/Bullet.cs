using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class Bullet : Projectile
    {
        protected override void OnEnable() 
        {
            base.OnEnable();
            destoryTime = 4f;
            // StartCoroutine(ReturnToPoolAfterTime());
        }

        private void Update() 
        {
            rb.velocity = flyingDirection * flyingSpeed * Time.deltaTime;
            if (IsOffScreen())
            {
                Debug.Log("Off Screen!!!!");
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}
