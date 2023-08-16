using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class Bullet : Projectile
    {
        public TrailRenderer trail;
        protected override void OnEnable() 
        {
            base.OnEnable();
            destoryTime = 4f;
        }

        private void Update() 
        {
            if (trail != null && !trail.gameObject.activeSelf)
            {
                trail.gameObject.SetActive(true);
            }
            rb.velocity = flyingDirection * flyingSpeed * Time.deltaTime;
            if (IsOffScreen())
            {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}
