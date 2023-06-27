using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG.Data
{
    public class BossEnemyData 
    {
        private DRBossEnemy dRBossEnemy;

        public int Id 
        {
            get 
            {
                return dRBossEnemy.Id;
            }
        }

        public int EntityId
        {
            get 
            {
                return dRBossEnemy.EntityId;
            }
        }

        public string NameId
        {
            get 
            {
                return dRBossEnemy.NameId;
            }
        }

        public string Type
        {
            get 
            {
                return dRBossEnemy.Type;
            }
        }

        public float Durability
        {
            get 
            {
                return dRBossEnemy.Durability;
            }
        }

        public float Shields
        {
            get 
            {
                return dRBossEnemy.Shields;
            }
        }

        public float Firepower
        {
            get 
            {
                return dRBossEnemy.Firepower;
            }
        }

        public float FireRate
        {
            get 
            {
                return dRBossEnemy.FireRate;
            }
        }

        public float Agility
        {
            get 
            {
                return dRBossEnemy.Agility;
            }
        }

        public float Speed
        {
            get 
            {
                return dRBossEnemy.Speed;
            }
        }

        public BossEnemyData(DRBossEnemy dRBossEnemy)
        {
            this.dRBossEnemy = dRBossEnemy;
        }
    }
}
