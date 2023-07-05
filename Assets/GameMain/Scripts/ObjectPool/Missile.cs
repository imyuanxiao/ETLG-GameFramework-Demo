using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    // TODO : need to be fixed
    public class Missile : Projectile
    {
        [HideInInspector] public Transform target;
        public float rotationSpeed = 20f;
        // private float flyingTime;
        // private float timeElapsed;

        protected override void OnEnable() 
        {
            base.OnEnable();
            target = BattleManager.Instance.bossEnemyEntity.gameObject.transform;
            // flyingTime = 1.0f;
            // flyingSpeed = 10f;
            // timeElapsed = 0f;
            flyingDirection = Vector3.zero;
            // StartCoroutine(ChangeFlyingDirection(target));
        }

        // private IEnumerator ChangeFlyingDirection(Transform target) {
        //     while (Quaternion.Angle(transform.rotation, target.rotation) > 0.1) {
        //         float rotateSpeed = Quaternion.Angle(transform.rotation, target.rotation) / (flyingTime - timeElapsed);
        //         transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotateSpeed * Time.deltaTime);
        //         timeElapsed += Time.deltaTime;
        //         yield return null;
        //     }
        //     transform.rotation = target.rotation;
        // }

        private void FixedUpdate() 
        {
            // flyingSpeed = Vector3.Distance(target.position, transform.position) / (flyingTime - timeElapsed);
            // rb.velocity = flyingDirection * flyingSpeed * Time.deltaTime;

            // Calculate the direction towards the target
            Vector3 targetDirection = (target.position - transform.position).normalized;

            // Calculate the rotation towards the target
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Smoothly rotate the missile towards the target
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));

            // Apply forward force to the missile
            rb.AddForce(transform.forward * flyingSpeed, ForceMode.Acceleration);
            // rb.velocity = flyingDirection * flyingSpeed;

            if (IsOffScreen())
            {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}
