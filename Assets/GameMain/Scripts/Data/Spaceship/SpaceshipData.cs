using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace ETLG.Data
{
    public class SpaceshipData
    {
        private DRSpaceship dRSpaceship;
        // private ProjectileData projectileData;

        // 获取ID
        public int Id
        {
            get
            {
                return dRSpaceship.Id;
            }
        }

        // 获取本地化文件里对应的属性名
        public string NameId
        {
            get
            {
                return GameEntry.Localization.GetString(dRSpaceship.NameId);
            }
        }

        public string Type
        {
            get
            {
                return dRSpaceship.Type;
            }
        }

        public int EntityId
        {
            get
            {
                return dRSpaceship.EntityId;
            }
        }


        public float Energy
        {
            get
            {
                return dRSpaceship.Energy;
            }
        }

        public float Durability
        {
            get
            {
                return dRSpaceship.Durability;
            }
        }
        public float Shields
        {
            get
            {
                return dRSpaceship.Shields;
            }
        }
        public float Firepower
        {
            get
            {
                return dRSpaceship.Firepower;
            }
        }
        public float FireRate
        {
            get
            {
                return dRSpaceship.FireRate;
            }
        }

        public float Agility
        {
            get
            {
                return dRSpaceship.Agility;
            }
        }        
        
        public float Speed
        {
            get
            {
                return dRSpaceship.Speed;
            }
        }        
        
        public float Dogde
        {
            get
            {
                return dRSpaceship.Dogde;
            }
        }       
        
        public float Detection
        {
            get
            {
                return dRSpaceship.Detection;
            }
        }        
        
        public int Capacity
        {
            get
            {
                return dRSpaceship.Capacity;
            }
        }

        
        public int Skill
        {
            get
            {
                return dRSpaceship.Skill;
            }
        }

        public int ProjectileId
        {
            get
            {
                return dRSpaceship.ProjectileId;
            }
        }

        // public ProjectileData ProjectileData
        // {
        //     get
        //     {
        //         return projectileData;
        //     }
        // }

        // public SpaceshipData(DRSpaceship dRSpaceship, ProjectileData projectileData)
        // {
        //     this.dRSpaceship = dRSpaceship;
        //     this.projectileData = projectileData;
        // }
        
        public SpaceshipData(DRSpaceship dRSpaceship)
        {
            this.dRSpaceship = dRSpaceship;
        }

    }
}


