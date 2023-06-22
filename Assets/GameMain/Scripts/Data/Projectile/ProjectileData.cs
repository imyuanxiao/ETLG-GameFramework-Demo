using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG.Data
{
    public class ProjectileData
    {
        private DRProjectile dRProjectile;

        public int Id
        {
            get 
            {
                return dRProjectile.Id;
            }
        }

        public int EntityId
        {
            get 
            {
                return dRProjectile.EntityId;
            }
        }

        public string ProjectileType
        {
            get 
            {
                return dRProjectile.ProjectileType;
            }
        }

        public float Speed
        {
            get
            {
                return dRProjectile.Speed;
            }
        }

        public float Damage
        {
            get
            {
                return dRProjectile.Damage;
            }
        }

        public float SplashDamage
        {
            get
            {
                return dRProjectile.SplashDamage;
            }
        }

        public float SplashRange
        {
            get 
            {
                return dRProjectile.SplashRange;
            }
        }

        public ProjectileData(DRProjectile dRProjectile)
        {
            this.dRProjectile = dRProjectile;
        }
    }
}
