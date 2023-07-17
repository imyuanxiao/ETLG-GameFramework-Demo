using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class ReturnParticlesToPool : MonoBehaviour
    {
        private void OnParticleSystemStopped() 
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
