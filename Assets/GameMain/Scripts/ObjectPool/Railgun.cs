using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class Railgun : Projectile
    {
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
