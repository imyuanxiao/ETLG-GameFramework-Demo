using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class PlayerProjectile : Projectile
    {
        protected override void OnEnable() 
        {
            base.OnEnable();
            destoryTime = 2f;
            StartCoroutine(ReturnToPoolAfterTime());
        }
    }
}
