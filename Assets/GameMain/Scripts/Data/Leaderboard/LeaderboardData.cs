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

        public LeaderboardData(int Id,string name,float data)
        {
            this.Id = Id;
            this.Name = name;
            this.data = data;
        }
    }
}

