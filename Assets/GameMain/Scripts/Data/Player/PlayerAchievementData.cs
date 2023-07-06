using System.Collections.Generic;

namespace ETLG.Data
{
    public sealed class PlayerAchievementData
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public bool IsUnlocked { get; set; }
        public int Level { get; set; }
        public int Progress { get; set; }

        public int activeState { get; set; }
        public PlayerAchievementData(AchievementData achievementData)
        {
            this.Id = achievementData.Id;
            this.Level = achievementData.Points.Length;
            this.TypeId = achievementData.TypeId;
            this.Level = achievementData.Count.Length;
            this.IsUnlocked = false;
            if(achievementData.TypeId!=Constant.Type.ACHV_LEADERSHIP)
            {
                this.Progress = 0;
            }
        }

    }   
}
