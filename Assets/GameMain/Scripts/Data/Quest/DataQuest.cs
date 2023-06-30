using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace ETLG.Data
{
    public sealed class DataQuest : DataBase
    {
        // 任务数据读取
        private IDataTable<DRQuest> dtQuests;

        // 任务数据键值对载体，通过ID获取任务信息
        private Dictionary<int, QuestData> dicQuestData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Quest");
        }

        protected override void OnLoad()
        {
            // 获取预加载的 Quest.txt 里的数据
            dtQuests = GameEntry.DataTable.GetDataTable<DRQuest>();

            if (dtQuests == null)
                throw new System.Exception("Can not get data table Quest");

            // 把所有 Quest 存到键值对数据结构中
            dicQuestData = new Dictionary<int, QuestData>();

            // 获取所有任务数据
            DRQuest[] dRQuests = dtQuests.GetAllDataRows();

            foreach (var dRQuest in dRQuests)
            {
                if (dicQuestData.ContainsKey(dRQuest.Id))
                {
                    throw new System.Exception(string.Format("Data skill level id '{0}' duplicate.", dRQuest.Id));
                }
                dicQuestData.Add(dRQuest.Id, new QuestData(dRQuest));
            }


        }



        protected override void OnUnload()
        {
        }

        protected override void OnShutdown()
        {

        }



        public QuestData GetQuestData(int id)
        {
            if (!dicQuestData.ContainsKey(id))
            {
                Log.Error("Can not find quest data id '{0}'.", id);
                return null;
            }

            return dicQuestData[id];
        }

    }
}


