using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;

namespace ETLG
{
    public class EnemyBullet : EnemyProjectile
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            flyingDirection = new Vector3(0, 0, -1);
            flyingSpeed = 1000;

            // if current procedure is basic battle procedure
            damage = (int) ((int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship.Durability * 0.1);

            // if current procedure is intermidate battle procedure
            // damage = 

            // if current procedure is final battle procedure
            // damage = 

            Debug.Log("Enemy Bullet Damage: " + damage + " / " + GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship.Durability);
        }

        private void Update() 
        {
            rb.velocity = flyingDirection * flyingSpeed * Time.deltaTime;
        }
    }
}
