using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace ETLG.Data
{
    public sealed class DataNPC : DataBase
    {
        // NPC数据读取
        private IDataTable<DRNPC> dtNpcs;

        // NPC数据键值对载体，通过ID获取NPC信息
        private Dictionary<int, NPCData> dicNPCData;

        public int currentNPCId { get; set; }

        public Vector3 RewardUIPosition { get; set; }

        protected override void OnInit()
        {
            currentNPCId = 1101;
        }

        protected override void OnPreload()
        {
            LoadDataTable("NPC");

        }

        protected override void OnLoad()
        {
            // 获取预加载的 NPC.txt 里的数据
            dtNpcs = GameEntry.DataTable.GetDataTable<DRNPC>();

            DRNPC[] dRNPCs = dtNpcs.GetAllDataRows();

            //DataQuest dataQuest = GameEntry.Data.GetData<DataQuest>();

            dicNPCData = new Dictionary<int, NPCData>(); 

            foreach (var dRNPC in dRNPCs)
            {
                /*        QuestData[] quests = new QuestData[dRNPC.Quests.Length];

                        for(int i = 0; i < dRNPC.Quests.Length; i++)
                        {
                            QuestData questData = dataQuest.GetQuestData(dRNPC.Quests[i]);

                            if (questData == null)
                            {
                                throw new System.Exception(string.Format("Can not find quest id '{0}' in DataTable Quest.", dRNPC.Quests[i]));
                            }
                            quests[i] = questData;
                        }

                        dicNPCData.Add(dRNPC.Id, new NPCData(dRNPC, quests));*/
                dicNPCData.Add(dRNPC.Id, new NPCData(dRNPC)); 

            }

        }

        protected override void OnUnload()
        {


        }

        protected override void OnShutdown()
        {

        }

        public NPCData GetNPCData(int id)
        {
            if (!dicNPCData.ContainsKey(id))
            {
                Log.Error("Can not find NPC data id '{0}'.", id);
                return null;
            }

            return dicNPCData[id];
        }

        public NPCData GetCurrentNPCData()
        {
            if (!dicNPCData.ContainsKey(currentNPCId))
            {
                Log.Error("Can not find NPC data id '{0}'.", currentNPCId);
                return null;
            }

            return dicNPCData[currentNPCId];
        }

        public List<int> getAllNPCsID()
        {
            List<int> allNPCId = new List<int>();
            foreach(KeyValuePair<int, NPCData> kvp in dicNPCData)
            {
                allNPCId.Add(kvp.Key);

            }
            return allNPCId;
        }
    }
}


