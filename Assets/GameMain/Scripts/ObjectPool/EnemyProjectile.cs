using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;

namespace ETLG
{
    public class EnemyProjectile : Projectile
    {
        protected virtual void OnEnable() 
        {
            destoryTime = 2f;
            StartCoroutine(ReturnToPoolAfterTime());
        }
    }
}
