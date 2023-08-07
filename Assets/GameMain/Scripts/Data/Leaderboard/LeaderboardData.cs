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
        public float data { get; set; }
        public int ExploredChapter { get; set; }

        public float bossTime;

        public LeaderboardData()
        {

        }
        public LeaderboardData(int Id)
        {
            this.Id = Id;
            //
        }
        public LeaderboardData(int Id,string name,float data)
        {
            this.Id = Id;
            this.Name = name;
            this.data = data;
        }
    }
}

