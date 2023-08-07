using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using ETLG.Data;
using UnityEngine.Networking;
using ETLG;

namespace ETLG.Data
{
    public class DataBackend : DataBase
    {
        public string responseData;
        public string avatorId;
        //排行榜
        public string leaderboard_url = "http://localhost:9527/profile/rank";
        //登录注册
        public string Login_url = "http://localhost:9527/auth/login";
        public string Register_url = "http://localhost:9527/auth/register";
        public string currentUser_url = "http://localhost:9527/auth/currentUser";
        public string saveDownload_url = "http://localhost:9527/profile/saveDownload";
        public string getProfileById_url = "http://localhost:9527/profile/user/";
        public string profileUpdate_url = "http://localhost:9527/profile/update";
        public string profilePassword_url = "http://localhost:9527/profile/password";
        //Authorization token
        public string authorization;
        //用于回调函数
        public delegate void IsLoginDetectionCallback(bool isLoggedIn);

        public string message;
        public UserProfile userProfile;
        public Dictionary<string, string> jsonStrDic;

        public UserData currentUser;

        public int loginType;

        public LeaderboardData selectedRank;
        public List<LeaderboardData> rankList;
        protected override void OnInit()
        {
            userProfile = new UserProfile();
            currentUser = new UserData();
            rankList = new List<LeaderboardData>();
        }
       
        [System.Serializable]
        public class UserData
        {
            public int id;
            public string username;
            public string avatar;
            public string nickName;
        }
        [System.Serializable]
        public class UserProfile
        {
            public int avatar;
            public string nickName;
            public string playerScore;
            public string achievement;
            public string learningProgress;
            public float boss1;
            public float boss2;
            public float boss3;
            public float boss4;
            public float boss5;
            public float boss6;
            public float boss7;
        }
    }
}
