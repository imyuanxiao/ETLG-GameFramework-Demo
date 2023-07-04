using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace ETLG.Data
{
    public sealed class DataLandingPoint : DataBase
    {
        // 登陆点数据读取
        private IDataTable<DRLandingPoint> dtLandingPoints;

        // 数据键值对载体，通过ID获取登陆点信息
        private Dictionary<int, LandingpointData> dicLandingPointData;

        public int currentLandingPointID;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("LandingPoint");

        }

        protected override void OnLoad()
        {
            // 获取预加载的 LandingPoint.txt 里的数据
            dtLandingPoints = GameEntry.DataTable.GetDataTable<DRLandingPoint>();

            DRLandingPoint[] dRLandingPoints = dtLandingPoints.GetAllDataRows();

            DataNPC dataNPC = GameEntry.Data.GetData<DataNPC>();

            dicLandingPointData = new Dictionary<int, LandingpointData>(); 

            foreach (var dRLandingPoint in dRLandingPoints)
            {
                NPCData[] npcs = new NPCData[dRLandingPoint.NPCsID.Length];

                for(int i = 0; i < dRLandingPoint.NPCsID.Length; i++)
                {
                    NPCData npcData = dataNPC.GetNPCData(dRLandingPoint.NPCsID[i]);

                    if (npcData == null)
                    {
                        throw new System.Exception(string.Format("Can not find npc id '{0}' in DataTable NPC.", dRLandingPoint.NPCsID[i]));
                    }
                    npcs[i] = npcData;
                }

                dicLandingPointData.Add(dRLandingPoint.Id, new LandingpointData(dRLandingPoint, npcs));

            }

        }

        protected override void OnUnload()
        {


        }

        protected override void OnShutdown()
        {

        }

        public LandingpointData GetLandingPointData(int id)
        {
            if (!dicLandingPointData.ContainsKey(id))
            {
                Log.Error("Can not find Landing point data id '{0}'.", id);
                return null;
            }

            return dicLandingPointData[id];
        }

        public LandingpointData GetCurrentLandingPointData()
        {
            //为测试，这里直接 赋值 101，应该是动态变化的

            // currentLandingPointID = 101;

            if (!dicLandingPointData.ContainsKey(currentLandingPointID))
            {
                Log.Error("Can not find Landing point data id '{0}'.", currentLandingPointID);
                return null;
            }
            return dicLandingPointData[currentLandingPointID];
        }


    }
}


