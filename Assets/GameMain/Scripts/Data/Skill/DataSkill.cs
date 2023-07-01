using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using GameFramework;

namespace ETLG.Data
{

    public sealed class DataSkill : DataBase
    {

        // 技能行数据读取脚本
        private IDataTable<DRSkill> dtSkill;
        // 技能等级行数据读取脚本
        private IDataTable<DRSkillLevel> dtSkillLevel;

        // 技能行数据载体脚本，通过ID获取技能信息
        private Dictionary<int, SkillData> dicSkillData;

        // 技能等级行数据载体脚本，这里名字叫 xxData 只是为了和上面统一，实际里面并没有存 SkillLevelData，因为暂时没必要多写一个xxData脚本
        private Dictionary<int, DRSkillLevel> dicSkillLevelData;

        // 根据层数对技能分类
        private Dictionary<int, List<SkillData>> dicSkillDataLayers;


        protected override void OnInit()
        {

        }

        // 预加载数据配置表
        protected override void OnPreload()
        {
            LoadDataTable("Skill");
            LoadDataTable("SkillLevel");
        }

        // 对预加载数据进行进一步处理
        protected override void OnLoad()
        {

            // 初始化每层技能列表
            dicSkillDataLayers = new Dictionary<int, List<SkillData>>();
            for (int i = 0; i < 6; i++)
            {
                dicSkillDataLayers[i] = new List<SkillData>();
            }

            // 获取预加载的 Skill.txt 里的数据
            dtSkill = GameEntry.DataTable.GetDataTable<DRSkill>();

            if (dtSkill == null)
                throw new System.Exception("Can not get data table Skill");

            // 获取预加载的 SkillLevel.txt 里的数据
            dtSkillLevel = GameEntry.DataTable.GetDataTable<DRSkillLevel>();

            if (dtSkillLevel == null)
                throw new System.Exception("Can not get data table SkillLevel");

            // 把所有 SkillLevel 存到键值对数据结构中
            dicSkillLevelData = new Dictionary<int, DRSkillLevel>();

            // 获取预加载的所有 SkillLevel 数据行数据
            DRSkillLevel[] dRSkillLevels = dtSkillLevel.GetAllDataRows();

            foreach (var dRSkillLevel in dRSkillLevels)
            {
                if (dicSkillLevelData.ContainsKey(dRSkillLevel.Id))
                {
                    throw new System.Exception(string.Format("Data skill level id '{0}' duplicate.", dRSkillLevel.Id));
                }
                dicSkillLevelData.Add(dRSkillLevel.Id, dRSkillLevel);
            }


            // 获取预加载的所有 Skill 数据行数据
            DRSkill[] drSkills = dtSkill.GetAllDataRows();

            // 新建技能表（id为键，技能数据为值），把 DRSkill 和 DRSkillLevel 数据都存到 SkillData里
            dicSkillData = new Dictionary<int, SkillData>();

            // 将 Skill 表里对应的 SkillLevel 数据处理成数组，加到 SkillData 对象里
            foreach (var drSkill in drSkills)
            {

                DRSkillLevel[] tmpDRSkillLevels = new DRSkillLevel[drSkill.Levels.Length];

                for (int i = 0; i < drSkill.Levels.Length; i++)
                {
                    if (!dicSkillLevelData.ContainsKey(drSkill.Levels[i]))
                    {
                        throw new System.Exception(string.Format("Can not find skill level id '{0}' in DataTable SkillLevel.", drSkill.Levels[i]));
                    }

                    tmpDRSkillLevels[i] = dicSkillLevelData[drSkill.Levels[i]];
                }

                SkillData skillData = new SkillData(drSkill, tmpDRSkillLevels);
                dicSkillData.Add(drSkill.Id, skillData);

                // 把技能加到对应层数的列表中
                dicSkillDataLayers[drSkill.Location[0]].Add(skillData);

            }

        }

        // 获取技能层的所有技能数据
        public List<SkillData> GetSkillDataLayer(int layer)
        {
            if (!dicSkillDataLayers.ContainsKey(layer))
            {
                Log.Error("Can not find skill data layer id '{0}'.", layer);
                return null;
            }

            return dicSkillDataLayers[layer];
        }


        public SkillData GetSkillData(int id)
        {
            if (!dicSkillData.ContainsKey(id))
            {
                Log.Error("Can not find skill data id '{0}'.", id);
                return null;
            }

            return dicSkillData[id];
        }

        public SkillData[] GetAllSkillData()
        {
            int index = 0;
            SkillData[] results = new SkillData[dicSkillData.Count];
            foreach (var skillData in dicSkillData.Values)
            {
                results[index++] = skillData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRSkill>();
            GameEntry.DataTable.DestroyDataTable<DRSkillLevel>();


            dicSkillData = null;
            dicSkillLevelData = null;

        }

        protected override void OnShutdown()
        {
        }
    }

}