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

        private SkillData[] skillDatas;


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

        public int ProjectileId
        {
            get
            {
                return dRSpaceship.ProjectileId;
            }
        }

        public SpaceshipData(DRSpaceship dRSpaceship, SkillData[] skillDatas)
        {
            this.dRSpaceship = dRSpaceship;
            this.skillDatas = skillDatas;
        }

        // 获取飞船初始技能信息
        public SkillData GetSkillData(int num)
        {
            if (skillDatas == null || num > GetMaxNum())
                return null;

            return skillDatas[num];
        }

        public int GetMaxNum()
        {
            return skillDatas == null ? 0 : skillDatas.Length - 1;
        }

    }
}

