using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.DataTable;

namespace ETLG.Data
{
    public sealed class DataProjectile : DataBase
    {
        private IDataTable<DRProjectile> dtProjectile;
        private Dictionary<int, ProjectileData> dicProjectileData;

        protected override void OnInit()
        {
            base.OnInit();
        }

        protected override void OnPreload()
        {
            base.OnPreload();
            LoadDataTable("Projectile");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            dicProjectileData = new Dictionary<int, ProjectileData>();

            dtProjectile = GameEntry.DataTable.GetDataTable<DRProjectile>();

            if (dtProjectile == null)
                throw new System.Exception("Can not get data table Projectile");

            DRProjectile[] dRProjectiles = dtProjectile.GetAllDataRows();

            foreach (DRProjectile dRProjectile in dRProjectiles)
            {
                ProjectileData projectileData = new ProjectileData(dRProjectile);
                dicProjectileData.Add(dRProjectile.Id, projectileData);
            }
        }

        public ProjectileData GetProjectileData(int id)
        {
            if (dicProjectileData.ContainsKey(id)) 
            {
                return dicProjectileData[id];
            }
            return null;
        }

        public ProjectileData[] GetAllProjectileData() 
        {
            int index = 0;
            ProjectileData[] results = new ProjectileData[dicProjectileData.Count];
            foreach (ProjectileData projectileData in dicProjectileData.Values)
            {
                results[index++] = projectileData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GameEntry.DataTable.DestroyDataTable<DRProjectile>();

            dtProjectile = null;
            dicProjectileData = null;
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }
    }
}

