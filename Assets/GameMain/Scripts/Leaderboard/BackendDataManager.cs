using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using ETLG.Data;
using UnityEngine.Networking;
using ETLG;
namespace ETLG
{
    public class BackendDataManager : Singleton<BackendDataManager>
    {
        public string responseData;
        private List<LeaderboardData> rankList;
        //排行榜
        private string leaderboard_url = "https://github.com/xw22087/rbac/tree/main/rbac-backend/src/main/java/com/imyuanxiao/rbac/controller/api/profile/getRank";
        //登录注册
        private string Login_url ;
        private string Register_url;
        public int errorType;
        public bool isNewFetch;
        protected override void Awake()
        {
            base.Awake();
            rankList = new List<LeaderboardData>();
        }
        public List<LeaderboardData> GetRankData(int pageNumber, int pageSize, int rankMode)
        {
            StartCoroutine(GetRankDataRoutine(pageNumber, pageSize, rankMode));
            return rankList;
        }

        private IEnumerator GetRankDataRoutine(int pageNumber, int pageSize, int rankMode)
        {
            // 创建POST请求的表单数据
            WWWForm form = new WWWForm();
            form.AddField("pageNumber", pageNumber);
            form.AddField("pageSize", pageSize);
            form.AddField("rankMode", rankMode);
            isNewFetch = true;
            using (UnityWebRequest www = UnityWebRequest.Post(leaderboard_url, form))
            {
                yield return www.SendWebRequest();
                Debug.Log(www.result);
                if (www.result == UnityWebRequest.Result.Success)
                {
                    errorType = 0;
                    // 获取API响应数据
                    string responseJson = www.downloadHandler.text;

                    // 解析JSON响应数据
                    List<List<object>> rankData = JsonUtility.FromJson<RankData>(responseJson).rankList;

                    // 处理排行榜数据
                    foreach (List<object> rowData in rankData)
                    {
                        LeaderboardData data = new LeaderboardData();
                        string userName = (string)rowData[0];
                        data.Name = userName;
                        int id = (int)rowData[1];
                        data.Id = id;
                        int spaceshipScore = (int)rowData[2];
                        data.SpaceshipScore = spaceshipScore;
                        int achievementPoint = (int)rowData[3];
                        data.AchievementScore = achievementPoint;
                        if(rankMode>1)
                        {
                            float boss = (int)rowData[4];
                            //data.boss
                        }
                        Debug.Log("User Name: " + userName + ", Spaceship Score: " + spaceshipScore);
                    }
                }
                else
                {
                    // API请求失败
                    HandleErrorMessages(www);
                    GameEntry.Event.Fire(this, ErrorMessagePopPUpEventArgs.Create());
                }
            }
        }
        private void HandleLogin()
        {

        }
        private void HandleRegister()
        {

        }
        private void HandleErrorMessages(UnityWebRequest www)
        {
            Debug.LogError("Error: " + www.error);
            if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                errorType = Constant.Type.ERROR_NETWORK;
            }
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                errorType = Constant.Type.ERROR_SERVER;
            }
            if (www.result == UnityWebRequest.Result.DataProcessingError)
            {
                errorType = Constant.Type.ERROR_DATA;
            }
        }
        [System.Serializable]
        private class RankData
        {
            public List<List<object>> rankList;
        }
    }
}
