using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class Laser : Projectile
    {
        protected override void OnEnable() 
        {
            base.OnEnable();
            destoryTime = 2f;
        }

        private void Update() 
        {
            rb.velocity = flyingDirection * flyingSpeed * Time.deltaTime;
            if (IsOffScreen())
            {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}
