using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;

namespace ETLG
{
    public class EnemyProjectile : Projectile
    {
        protected override void OnEnable() 
        {
            base.OnEnable();
            destoryTime = 4f;
            StartCoroutine(ReturnToPoolAfterTime());
        }
    }
}
