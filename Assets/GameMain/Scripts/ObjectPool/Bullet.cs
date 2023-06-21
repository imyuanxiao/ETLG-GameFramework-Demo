using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float destoryTime = 3;

        private void OnEnable() 
        {
            StartCoroutine(ReturnToPoolAfterTime());
        }

        private IEnumerator ReturnToPoolAfterTime()
        {
            float elapsedTime = 0f;
            while (elapsedTime < destoryTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
