using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class Projectile : MonoBehaviour
    {
        protected float destoryTime;
        protected float flyingSpeed;
        protected Vector3 flyingDirection;
        [HideInInspector] public int damage;
        protected Rigidbody rb;

        private void Awake() 
        {
            rb = GetComponent<Rigidbody>();
        }

        protected IEnumerator ReturnToPoolAfterTime()
        {
            float elapsedTime = 0f;
            while (elapsedTime < destoryTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        private void OnDisable() 
        {
            StopAllCoroutines();    
        }
    }
}
