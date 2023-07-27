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
        List<List<object>> rankData;
        //排行榜
        public enum RankMode
        {
            Score = 0,
            Achievement = 1,
            Chapter = 2
        }

        private string leaderboard_url = "https://github.com/xw22087/rbac/tree/main/rbac-backend/src/main/java/com/imyuanxiao/rbac/controller/api/profile/getRank";


        protected override void Awake()
        {
            base.Awake();
        }
        public List<List<object>> GetRankData(int pageNumber, int pageSize, RankMode rankMode)
        {
            StartCoroutine(GetRankDataRoutine(pageNumber, pageSize, (int)rankMode));
            return rankData;
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
                    rankData = JsonUtility.FromJson<RankData>(responseJson).rankList;

                    // 处理排行榜数据
                    foreach (List<object> rowData in rankData)
                    {
                        int userName = (int)rowData[0];
                        int rankValue = (int)rowData[1];
                        Debug.Log("User Name: " + userName + ", Rank Value: " + rankValue);
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
        /* private IEnumerator FetchDataCoroutine(string url)
         {
             WWWForm form = new WWWForm();
             form.AddField("pageNumber", pageNumber);
             form.AddField("pageSize", pageSize);
             form.AddField("rankMode", rankMode);

             using (UnityWebRequest www = UnityWebRequest.Post(leaderboard_url, form))
             {
                 yield return www.SendWebRequest();

                 if (www.result == UnityWebRequest.Result.Success)
                 {
                     // 获取数据成功
                     string responseJson = www.downloadHandler.text;
                     // 解析JSON数据
                     List<List<object>> rankList = JsonUtility.toBean(responseJson, typeof(List<List<object>>)) as List<List<object>>;

                     // 处理排行榜数据
                     foreach (List<object> rowData in rankList)
                     {
                         int userName = int.Parse(rowData[0].ToString());
                         int rankData = int.Parse(rowData[1].ToString());
                         Debug.Log("User Name: " + userName + ", Rank Data: " + rankData);
                     }
                 }
                 else
                 {
                     // 获取数据失败
                     Debug.LogError("Error: " + www.error);
                 }
             }

         }
         */
    }
}
