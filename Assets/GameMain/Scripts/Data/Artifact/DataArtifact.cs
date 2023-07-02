using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace ETLG.Data
{
    public sealed class DataArtifact : DataBase
    {
        // 道具数据读取
        private IDataTable<DRArtifact> dtArtifacts;

        // 模块道具数据读取
        private IDataTable<DRModule> dtModules;

        // 根据ID获取道具基本信息（Artifact.txt里的）
        private Dictionary<int, DRArtifact> dicArtifactBaseData;

        // 数据键值对载体，通过ID获取道具信息
        private Dictionary<int, ArtifactDataBase> dicArtifactData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
            LoadDataTable("Artifact");
            LoadDataTable("Module");
        }

        protected override void OnLoad()
        {

            // 获取预加载的 Artifact.txt 里的数据
            dtArtifacts = GameEntry.DataTable.GetDataTable<DRArtifact>();
            // 获取所有Artifact行数据
            DRArtifact[] dRArtifactBases = dtArtifacts.GetAllDataRows();
            if (dtArtifacts == null)
                throw new System.Exception("Can not get data table Artifact");
            dicArtifactBaseData = new Dictionary<int, DRArtifact>();
            foreach (DRArtifact dArtifactBase in dRArtifactBases)
            {
                if (dicArtifactBaseData.ContainsKey(dArtifactBase.Id))
                {
                    throw new System.Exception(string.Format("Data Artifact id '{0}' duplicate.", dArtifactBase.Id));
                }
                dicArtifactBaseData.Add(dArtifactBase.Id, dArtifactBase);
            }


            // 获取预加载的 Module.txt 里的数据
            dtModules = GameEntry.DataTable.GetDataTable<DRModule>();
            // 获取所有Module行数据
            DRModule[] dRModules = dtModules.GetAllDataRows();
            if (dtModules == null)
                throw new System.Exception("Can not get data table Module");

            // 把所有 ArtifactModule 存到键值对数据结构中
            dicArtifactData = new Dictionary<int, ArtifactDataBase>();

            foreach (var dRModule in dRModules)
            {
                if (dicArtifactData.ContainsKey(dRModule.Id))
                {
                    throw new System.Exception(string.Format("Data module id '{0}' duplicate.", dRModule.Id));
                }

                dicArtifactData.Add(dRModule.ArtifactID, new ArtifactModuleData(dicArtifactBaseData[dRModule.ArtifactID], dRModule));
                // 用过Artifact里的Module类的数据可以移除
                dicArtifactBaseData.Remove(dRModule.ArtifactID);
            }

            // 其他有特殊属性的道具，比如能量之类的



            // 剩下没有特殊属性的道具
            foreach (var dRArtifactBase in dicArtifactBaseData)
            {
                dicArtifactData.Add(dRArtifactBase.Key, new ArtifactCommonData(dRArtifactBase.Value));
            }


        }



        protected override void OnUnload()
        {
        }

        protected override void OnShutdown()
        {

        }

        public ArtifactDataBase GetArtifactData(int id)
        {
            if (!dicArtifactData.ContainsKey(id))
            {
                Log.Error("Can not find quest data id '{0}'.", id);
                return null;
            }
            return dicArtifactData[id];
        }

    }
}


