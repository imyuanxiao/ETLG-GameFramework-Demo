using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;

namespace ETLG
{
    public class CriticalAttack : MonoBehaviour
    {
        public float coolDown = 5f;
        private float elapsedTime = 0f;
        private BossEnemyController bossEnemyController;

        private void Awake() 
        {
            bossEnemyController = GetComponent<BossEnemyController>();    
        }

        private void OnEnable() 
        {
            elapsedTime = 0f;
        }

        private void Update() 
        {
            if (elapsedTime < coolDown)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                elapsedTime = 0f;
                Debug.Log("Fire CriticalHit Event!!!");
                GameEntry.Event.Fire(this, EnemyCriticalHitEventArgs.Create(bossEnemyController.bossEnemyType));
            }
        }

        private void OnDisable()
        {

        }
    }
}
