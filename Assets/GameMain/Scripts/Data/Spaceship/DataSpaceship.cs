using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using GameFramework.Event;

namespace ETLG.Data
{
    // 继承自 DataBase，表示这是一个管理数据加载的类
    public sealed class DataSpaceship : DataBase
    {

        // DRSpaceship 是每一行数据
        private IDataTable<DRSpaceship> dtSpaceship;

        private Dictionary<int, SpaceshipData> dicSpaceshipData;

        // 需要从技能数据管理类获取技能数据
        private DataSkill dataSkill;


        protected override void OnInit()
        {

        }

        // 预加载阶段，加载名为 "Spaceship" 的数据表，即Spaceship.txt
        protected override void OnPreload()
        {
            LoadDataTable("Spaceship");
        }

        // 加载阶段加载所有数据行
        protected override void OnLoad()
        {
            // 初始化技能数据管理类
            dataSkill = GameEntry.Data.GetData<DataSkill>();

            dtSpaceship = GameEntry.DataTable.GetDataTable<DRSpaceship>();
            if (dtSpaceship == null)
                throw new System.Exception("Can not get data table Spaceship");


            dicSpaceshipData = new Dictionary<int, SpaceshipData>();

            // 所有飞船数据，此处后续应该加上获取飞船固有技能、飞船固有发射物等逻辑
            DRSpaceship[] dRSpaceships = dtSpaceship.GetAllDataRows();

            foreach (var drSpaceship in dRSpaceships)
            {

                SkillData[] dataSkills = new SkillData[drSpaceship.Skills.Length];

                for (int i = 0; i < drSpaceship.Skills.Length; i++)
                {
                    SkillData skillData = dataSkill.GetSkillData(drSpaceship.Skills[i]);
                    if (skillData == null)
                    {
                        throw new System.Exception(string.Format("Can not find skill id '{0}' in DataTable Skill.", drSpaceship.Skills[i]));
                    }

                    dataSkills[i] = skillData;
                }

                SpaceshipData spaceshipData = new SpaceshipData(drSpaceship, dataSkills);
                dicSpaceshipData.Add(drSpaceship.Id, spaceshipData);
            }

        }

        public SpaceshipData GetSpaceshipData(int id)
        {
            if (dicSpaceshipData.ContainsKey(id))
            {
                return dicSpaceshipData[id];
            }

            return null;
        }

        public SpaceshipData[] GetAllSpaceshipData()
        {
            int index = 0;
            SpaceshipData[] results = new SpaceshipData[dicSpaceshipData.Count];
            foreach (var spaceshipData in dicSpaceshipData.Values)
            {
                results[index++] = spaceshipData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRSpaceship>();

            dtSpaceship = null;
            dicSpaceshipData = null;
        }

        protected override void OnShutdown()
        {
        }
    }
}