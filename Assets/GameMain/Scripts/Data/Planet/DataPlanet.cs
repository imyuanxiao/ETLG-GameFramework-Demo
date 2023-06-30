using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace ETLG.Data
{
    public sealed class DataPlanet : DataBase
    {
        // 星球数据读取
        private IDataTable<DRPlanet> dtPlanets;

        // 数据键值对载体，通过ID获取星球信息
        private Dictionary<int, PlanetData> dicPlanetData;

        // 目前调用的星球ID
        public int currentPlanetID;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Planet");

        }

        protected override void OnLoad()
        {
            // 获取预加载的 LandingPoint.txt 里的数据
            dtPlanets = GameEntry.DataTable.GetDataTable<DRPlanet>();

            DRPlanet[] dRPlanets = dtPlanets.GetAllDataRows();

            DataLandingPoint dataLandingPoint = GameEntry.Data.GetData<DataLandingPoint>();

            dicPlanetData = new Dictionary<int, PlanetData>(); 

            foreach (var dRPlanet in dRPlanets)
            {
                LandingpointData[] landingpoints = new LandingpointData[dRPlanet.LandingPoints.Length];

                for(int i = 0; i < dRPlanet.LandingPoints.Length; i++)
                {
                    LandingpointData landingpointData = dataLandingPoint.GetLandingPointData(dRPlanet.LandingPoints[i]);

                    if (landingpointData == null)
                    {
                        throw new System.Exception(string.Format("Can not find landingpoint id '{0}' in DataTable LandingPoint.", dRPlanet.LandingPoints[i]));
                    }
                    landingpoints[i] = landingpointData;
                }

                dicPlanetData.Add(dRPlanet.Id, new PlanetData(dRPlanet, landingpoints));

            }

        }

        protected override void OnUnload()
        {


        }

        protected override void OnShutdown()
        {

        }

        public PlanetData GetPlanetData(int id)
        {
            if (!dicPlanetData.ContainsKey(id))
            {
                Log.Error("Can not find Planet data id '{0}'.", id);
                return null;
            }

            return dicPlanetData[id];
        }


        public PlanetData GetCurrentPlanetData()
        {
            return dicPlanetData[currentPlanetID];
        }

    }
}


