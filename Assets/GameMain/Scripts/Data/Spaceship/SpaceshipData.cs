using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using System;

namespace ETLG.Data
{
    public class SpaceshipData
    {
        private DRSpaceship dRSpaceship;

        //private SkillData[] skillDatas;


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

        // 获取本地化文件里对应的描述信息
        public string Description
        {
            get
            {
                return GameEntry.Localization.GetString(dRSpaceship.NameId + "Description");
            }
        }

        public string Type
        {
            get
            {
                return dRSpaceship.Type;
            }
        }

        public string SType
        {
            get
            {
                return dRSpaceship.SType;
            }
        }

        public string SSize
        {
            get
            {
                return dRSpaceship.SSize;
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

        public int[] SkillIds
        {
            get
            {
                return dRSpaceship.Skills;
            }
        }

        public SpaceshipData(DRSpaceship dRSpaceship)
        {
            this.dRSpaceship = dRSpaceship;
        }

    }
}

