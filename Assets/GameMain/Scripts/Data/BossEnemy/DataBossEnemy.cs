using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.DataTable;

namespace ETLG.Data
{
    public class DataBossEnemy : DataBase
    {
        private IDataTable<DRBossEnemy> dtBossEnemy;
        private Dictionary<int, BossEnemyData> dicBossEnemyData;

        protected override void OnInit()
        {
            base.OnInit();
        }

        protected override void OnPreload()
        {
            base.OnPreload();
            LoadDataTable("BossEnemy");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            dicBossEnemyData = new Dictionary<int, BossEnemyData>();

            dtBossEnemy = GameEntry.DataTable.GetDataTable<DRBossEnemy>();

            if (dtBossEnemy == null)
                throw new System.Exception("Can not get data table BossEnemy");

            DRBossEnemy[] dRBossEnemies = dtBossEnemy.GetAllDataRows();

            foreach (DRBossEnemy dRBossEnemy in dRBossEnemies)
            {
                BossEnemyData bossEnemyData = new BossEnemyData(dRBossEnemy);
                dicBossEnemyData.Add(dRBossEnemy.Id, bossEnemyData);
            }
        }

        public BossEnemyData GetBossEnemyData(int id) 
        {
            if (dicBossEnemyData.ContainsKey(id))
            {
                return dicBossEnemyData[id];
            }
            return null;
        }

        public BossEnemyData[] GetAllBossEnemyData() 
        {
            int index = 0;
            BossEnemyData[] results = new BossEnemyData[dicBossEnemyData.Count];
            foreach (BossEnemyData bossEnemyData in dicBossEnemyData.Values)
            {
                results[index++] = bossEnemyData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GameEntry.DataTable.DestroyDataTable<DRBossEnemy>();

            dtBossEnemy = null;
            dicBossEnemyData = null;
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }
    }
}
