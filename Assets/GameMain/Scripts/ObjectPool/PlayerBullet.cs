using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;

namespace ETLG
{
    public class PlayerBullet : PlayerProjectile
    {
        private ProjectileData bulletData;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            flyingDirection = new Vector3(0, 0, 1);
            bulletData = GameEntry.Data.GetData<DataProjectile>().GetProjectileData((int)EnumEntity.Bullet);
            damage = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().calculatedSpaceship.Firepower
                            + (int) bulletData.Damage;
        }

        private void Update() 
        {
            rb.velocity = flyingDirection * bulletData.Speed * Time.deltaTime * 1000;
        }
    }
}
