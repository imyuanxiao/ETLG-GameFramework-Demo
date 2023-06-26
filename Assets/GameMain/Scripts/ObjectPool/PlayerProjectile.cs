using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class PlayerProjectile : Projectile
    {
        protected virtual void OnEnable() 
        {
            destoryTime = 2f;
            StartCoroutine(ReturnToPoolAfterTime());
        }
    }
}
