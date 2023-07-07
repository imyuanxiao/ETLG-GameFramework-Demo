using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETLG.Data
{
    public class AchievementData
    {
        private DRAchievement dRAchievement;

        public int Id
        {
            get
            {
                return dRAchievement.Id;
            }
        }

        public string Name
        {
            get
            {
                return dRAchievement.Name;
            }
        }

        public int TypeId
        {
            get
            {
                return dRAchievement.TypeId;
            }
        }

        public int[] Points
        {
            get
            {
                return dRAchievement.Points;
            }
        }

        public int[] Count
        {
            get
            {
                return dRAchievement.Count;
            }
        }

        public int ConditionId
        {
            get
            {
                return dRAchievement.ConditionId;
            }
        }
        public AchievementData(DRAchievement dRAchievement)
        {
            this.dRAchievement = dRAchievement;
        }
    }
}
