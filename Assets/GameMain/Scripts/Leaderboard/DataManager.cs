using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using ETLG.Data;
using UnityEngine.Networking;
namespace ETLG
{
    public class DataManager : Singleton<DataManager>
    {
        public string responseData;
        List<LeaderboardData> rankList;
        //排行榜
        private string leaderboard_url = "https://github.com/xw22087/rbac/tree/main/rbac-backend/src/main/java/com/imyuanxiao/rbac/controller/api/profile/getRank";


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

            using (UnityWebRequest www = UnityWebRequest.Post(leaderboard_url, form))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
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
                    Debug.LogError("Error: " + www.error);
                }
            }
        }
        [System.Serializable]
        private class RankData
        {
            public List<List<object>> rankList;
        }
    }
}
