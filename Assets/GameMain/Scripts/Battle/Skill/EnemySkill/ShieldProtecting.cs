using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class ShieldProtecting : MonoBehaviour
    {
        public int shield = 500;
        public float coolDown = 3f;
        private float timeElapsed = 0;
        private bool isActive = false;
        private bool isReady = true;
        private BossEnemyHealth health;

        private void Awake() 
        {
            health = GetComponent<BossEnemyHealth>();
        }

        private void OnEnable() 
        {
            isReady = true;
        }

        private void Update() 
        {
            if (health.CurrentHealth <= health.MaxHealth * 0.5 && isReady)
            {
                health.MaxShield = shield;
                health.CurrentShield = shield;
                isActive = true;
                isReady = false;
            }
            if (isActive && health.CurrentShield <= 0)
            {
                isActive = false;
            }
            if (!isActive && !isReady) 
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed > coolDown)
                {
                    isReady = true;
                    timeElapsed = 0;
                }
            }
        } 
    }
}
