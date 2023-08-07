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
        private List<LeaderboardData> rankList;
        //排行榜
        public string leaderboard_url = "http://localhost:9527/profile/rank";
        //登录注册
        public string Login_url = "http://localhost:9527/auth/login";
        public string Register_url = "http://localhost:9527/auth/register";
        public string currentUser_url = "http://localhost:9527/auth/currentUser";
        public string saveDownload_url = "http://localhost:9527/profile/saveDownload";
        public string getProfileById_url = "http://localhost:9527/profile/";
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

        protected override void OnInit()
        {
            userProfile = new UserProfile();
            currentUser = new UserData();
        }
        [System.Serializable]
        private class RankResponseData
        {
            public List<List<object>> rankList;
        }
        private class ErrorMessage
        {
            public int code;
            public string message;
            public string data;
        }
        [System.Serializable]
        private class SaveDataClass
        {
            public string playerScore;
            public string BossDefeatTime;
            public string achievementScore;
            public string learningProgress;
        }
        [System.Serializable]
        private class LoginData
        {
            public string username;
            public string password;
        }
        [System.Serializable]
        private class ProfilePassword
        {
            public string oldPassword;
            public string newPassword;
        }
        [System.Serializable]
        private class ResponseData
        {
            public bool success;
            public int errorCode;
            public string errorMessage;
            public string data;
        }
        [System.Serializable]
        private class CurrentUserResponseData
        {
            public bool success;
            public int errorCode;
            public string errorMessage;
            public UserData data;
        }
        [System.Serializable]
        private class UserProfileResponseData
        {
            public bool success;
            public int errorCode;
            public string errorMessage;
            public UserProfile data;
        }
        [System.Serializable]
        private class LoginResponseData
        {
            public bool success;
            public int errorCode;
            public string errorMessage;
            public DataWrapper data;
        }
        [System.Serializable]
        private class DataWrapper
        {
            public UserData userVO;
            public string token;
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
        [System.Serializable]
        private class UserInfo
        {
            public string nickName;
            public int avatar;

        }
    }
}
