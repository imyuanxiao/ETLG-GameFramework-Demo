using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETLG.Data
{
    public class LeaderboardData
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Rank { get; set; }
        public int AchievementScore { get; set; }
        public float SpaceshipScore { get; set; }
        public float Boss_AI { get; set; }
        public float Boss_CloudComputing { get; set; }
        public float Boss_Blockchain { get; set; }
        public float Boss_Cybersecurity { get; set; }
        public float Boss_DataScience { get; set; }
        public float Boss_IoT { get; set; }
        public float Boss_Final { get; set; }
        public int ExploredChapter { get; set; }

        public float bossTime;
        /// <summary>
        /// 后端数据库连上之后
        /// public string Name { get; set; }
        /// public int Id { get; set; }
        /// public int AvatarId { get; set; }
        /// public int Rank { get; set; }
        /// public int AchievementScore { get; set; }
        /// public float SpaceshipScore { get; set; }
        /// public int exploredChapter { get; set; }
        ///public int dataScore;
        ///public float bossTime;
        /// 
        /// </summary>

        public LeaderboardData()
        {

        }
        public LeaderboardData(int Id)
        {
            this.Id = Id;
            //
        }
        public LeaderboardData(string Name,int Id, int AchievementScore, int SpaceshipScore, float Boss_AI, float Boss_CloudComputing, float Boss_Blockchain, float Boss_Cybersecurity, float Boss_DataScience, float Boss_IoT,float Boss_Final)
        {
            this.Name = Name;
            this.Id = Id;
            this.AchievementScore = AchievementScore;
            this.SpaceshipScore = SpaceshipScore;
            this.Boss_AI = Boss_AI;
            this.Boss_Blockchain = Boss_Blockchain;
            this.Boss_CloudComputing = Boss_CloudComputing;
            this.Boss_Cybersecurity = Boss_Cybersecurity;
            this.Boss_DataScience = Boss_DataScience;
            this.Boss_IoT = Boss_IoT;
            this.Boss_Final = Boss_Final;
        }
    }
}

