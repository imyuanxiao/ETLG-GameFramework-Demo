using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
namespace ETLG.Data
{
    public class DataAchievement : DataBase
    {
        private IDataTable<DRAchievement> dtAchievement;
        private Dictionary<int, AchievementData> dicAchievementData;
  
        protected override void OnInit()
        {

        }
        protected override void OnPreload()
        {
            base.OnPreload();
            LoadDataTable("Achievement");
        }
        protected override void OnLoad()
        {
            base.OnLoad();

            dicAchievementData = new Dictionary<int, AchievementData>();

            dtAchievement = GameEntry.DataTable.GetDataTable<DRAchievement>();

            if (dtAchievement == null)
                throw new System.Exception("Can not get data table New");

            DRAchievement[] dRAchievements = dtAchievement.GetAllDataRows();

            foreach (DRAchievement dRAchievement in dRAchievements)
            {
                AchievementData newData = new AchievementData(dRAchievement);
                dicAchievementData.Add(dRAchievement.Id, newData);
   
            }
       
        }
        protected override void OnUnload()
        {
            base.OnUnload();

            GameEntry.DataTable.DestroyDataTable<DRAchievement>();

            dtAchievement = null;
            dicAchievementData = null;

        }
        public AchievementData GetDataById(int id)
        {
            if (dicAchievementData.ContainsKey(id))
            {
                return dicAchievementData[id];
            }
            return null;
        }

        public AchievementData[] GetAllNewData()
        {
            int index = 0;
            AchievementData[] results = new AchievementData[dicAchievementData.Count];
            foreach (AchievementData newData in dicAchievementData.Values)
            {
                results[index++] = newData;
            }

            return results;
        }
        public List<AchievementData> GetDatasByTypeId(int typeId)
        {
            List<AchievementData> results = new List<AchievementData>();
            foreach (AchievementData data in dicAchievementData.Values)
            {
                if (data.TypeId == typeId)
                {
                    results.Add(data);
                }
            }
            return results;
        }
        public int GetAchievementCount()
        {
            //只显示减去隐藏成就的数量
            int result = 0;
            foreach(AchievementData achievementData in dicAchievementData.Values)
            {
                if(achievementData.TypeId!=Constant.Type.ACHV_HIDDEN)
                {
                    result++;
                }
            }
            return result;
        }
    }
}

