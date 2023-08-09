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
        //ÅÅÐÐ°ñ
        public string leaderboard_url = "http://16.170.243.113:9527/profile/rank";
        //µÇÂ¼×¢²á
        public string Login_url = "http://16.170.243.113:9527/auth/login";
        public string Register_url = "http://16.170.243.113:9527/auth/register";
        public string currentUser_url = "http://16.170.243.113:9527/auth/currentUser";
        public string saveDownload_url = "http://16.170.243.113:9527/profile/saveDownload";
        public string saveUpload_url = "http://16.170.243.113:9527/profile/saveUpload";
        public string getProfileById_url = "http://16.170.243.113:9527/profile/user/";
        public string profileUpdate_url = "http://16.170.243.113:9527/profile/update";
        public string profilePassword_url = "http://16.170.243.113:9527/profile/password";

        public string local_leaderboard_url = "http://localhost:9527/profile/rank";
        //µÇÂ¼×¢²á
        public string local_Login_url = "http://localhost:9527/auth/login";
        public string local_Register_url = "http://localhost:9527/auth/register";
        public string local_currentUser_url = "http://localhost:9527/auth/currentUser";
        public string local_saveDownload_url = "http://localhost:9527/profile/saveDownload";
        public string local_saveUpload_url = "http://localhost:9527/profile/saveUpload";
        public string local_getProfileById_url = "http://localhost:9527/profile/user/";
        public string local_profileUpdate_url = "http://localhost:9527/profile/update";
        public string local_profilePassword_url = "http://localhost:9527/profile/password";
        //Authorization token
        public string authorization;
        //ÓÃÓÚ»Øµ÷º¯Êý
        public delegate void IsLoginDetectionCallback(bool isLoggedIn);

        public string message;
        public UserProfile userProfile;
        public Dictionary<string, string> downLoadjsonStrDic;

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
       
        public string GetLoginUrl()
        {
            if(BackendDataManager.Instance.connectType == Constant.Type.BACK_REOMTE)
            {
                return Login_url;
            }
            return local_Login_url;
        }
        public string GetRegisterUrl()
        {
            if (BackendDataManager.Instance.connectType == Constant.Type.BACK_REOMTE)
            {
                return Register_url;
            }
            return local_Register_url;
        }
        public string GetLeaderboardUrl()
        {
            if (BackendDataManager.Instance.connectType == Constant.Type.BACK_REOMTE)
            {
                return leaderboard_url;
            }
            return local_leaderboard_url;
        }
        public string GetCurrentUserUrl()
        {
            if (BackendDataManager.Instance.connectType == Constant.Type.BACK_REOMTE)
            {
                return currentUser_url;
            }
            return local_currentUser_url;
        }
        public string GetSaveDownloadUrl()
        {
            if (BackendDataManager.Instance.connectType == Constant.Type.BACK_REOMTE)
            {
                return saveDownload_url;
            }
            return local_saveDownload_url;
        }
        public string GetSaveUploadUrl()
        {
            if (BackendDataManager.Instance.connectType == Constant.Type.BACK_REOMTE)
            {
                return saveUpload_url;
            }
            return local_saveUpload_url;
        }
        public string GetGetProfileByIdUrl()
        {
            if (BackendDataManager.Instance.connectType == Constant.Type.BACK_REOMTE)
            {
                return getProfileById_url;
            }
            return local_getProfileById_url;
        }
        public string GetProfileUpdateUrl()
        {
            if (BackendDataManager.Instance.connectType == Constant.Type.BACK_REOMTE)
            {
                return profileUpdate_url;
            }
            return local_profileUpdate_url;
        }
        public string GeProfilePasswordUrl()
        {
            if (BackendDataManager.Instance.connectType == Constant.Type.BACK_REOMTE)
            {
                return profilePassword_url;
            }
            return local_profilePassword_url;
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
