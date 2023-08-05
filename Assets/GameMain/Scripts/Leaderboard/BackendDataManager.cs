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
        public string avatorId;
        private List<LeaderboardData> rankList;
        //排行榜
        private string leaderboard_url = "http://localhost:9527/profile/rank";
        //登录注册
        private string Login_url= "http://localhost:9527/auth/login";
        private string Register_url = "http://localhost:9527/auth/register";
        private string currentUser_url = "http://localhost:9527/auth/currentUser";
        private string saveDownload_url = "http://localhost:9527/profile/saveDownload";
        //Authorization token
        private string authorization;
        public int errorType;
        public bool isNewFetch;
        public bool isSave;
        public bool isSuccess;
        public string message;
        Dictionary<string, string> jsonStrDic;

        public UserData currentUser;

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

                // 处理请求完成后的逻辑
                if (www.result == UnityWebRequest.Result.Success)
                {
                    // ...（原有代码不变）
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
                            if (rankMode > 1)
                            {
                                float boss = (int)rowData[4];
                                //data.boss
                            }
                            Debug.Log("User Name: " + userName + ", Spaceship Score: " + spaceshipScore);
                        }
                    }
                    else
                    {
                        HandleErrorMessages(www);
                        GameEntry.Event.Fire(this, ErrorMessagePopPUpEventArgs.Create());
                    }
                }
            } 

        }
        private void GetLogIn(string userName, string password)
        {
            currentUser = null;
            StartCoroutine(PostLogInUserRoutine(userName, password));
        }
        private IEnumerator PostLogInUserRoutine(string userName, string password)
        {
            // 创建POST请求的表单数据
            isNewFetch = true;
            isSuccess = false;
            // 构建要发送的数据对象
            LoginData loginData = new LoginData
            {
                username = userName,
                password = password
            };

            // 将数据对象转换为 JSON 格式
            string jsonData = JsonUtility.ToJson(loginData);

            using (UnityWebRequest request = new UnityWebRequest(Login_url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                isSuccess = true;
                var asyncOperation = request.SendWebRequest();

                yield return asyncOperation;

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string responseJson = request.downloadHandler.text;
                    LoginResponseData responseData = JsonUtility.FromJson<LoginResponseData>(responseJson);
                    if (responseData.success)
                    {
                        currentUser = responseData.data.userVO;
                        authorization = responseData.data.token;
                        
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_LOGIN_SUCCESS));
                    }
                    else
                    {
                        ErrorResponseData erroResponseData = JsonUtility.FromJson<ErrorResponseData>(responseJson);
                        message = erroResponseData.data;
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_LOGIN_FAILED));
                    }
                    
                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                    HandleErrorMessages(request);
                }
            }
        }

        public void HandleSave(Dictionary<string, string> jsonStrDic)
        {
            this.jsonStrDic = jsonStrDic;
        }
        public void HandleLoad()
        {
            //先判断是否登录
            //如果没登陆就开登录UI
            IsLoginDetection();
            GetSaveDownload();
            
        }
        public void HandleProfile()
        {
            //登录
            IsLoginDetection();
            
            
        }
        public void HandleLogIn(string userName, string password)
        {
            GetLogIn(userName, password);
            //load data

        }
        public void HandleRegister(string userName, string password)
        {
            GetRegister(userName, password);
            //先saveData，传到云端
        }
        private void GetRegister(string userName, string password)
        {
            currentUser = null;
            message = null;

            StartCoroutine(GetRegisterRoutine(userName, password));
        }
        private IEnumerator GetRegisterRoutine(string userName, string password)
        {
            isNewFetch = true;
            isSuccess = false;
            LoginData loginData = new LoginData
            {
                username = userName,
                password = password
            };

            // 将数据对象转换为 JSON 格式
            string jsonData = JsonUtility.ToJson(loginData);

            // 创建一个 UnityWebRequest 对象，并设置相关属性
            UnityWebRequest request = new UnityWebRequest(Register_url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // 发送请求并等待响应
            var asyncOperation = request.SendWebRequest();

            // 处理请求的响应
            yield return asyncOperation;

            if (request.result == UnityWebRequest.Result.Success)
            {
                // 请求成功，处理响应数据
                isSuccess = true;
                errorType = 0;
            }
            else
            {
                // 请求失败，处理错误
                Debug.LogError("Error: " + request.error);
                HandleErrorMessages(request);
                
            }
        }
        private void IsLoginDetection()
        {
            if (authorization == null)
            {
                authorization = null;
                GameEntry.UI.OpenUIForm(EnumUIForm.UILoginForm);
            }
            else
            {
                GetCurrentUser();
            }
           
        }
        public void GetSaveDownload()
        {
            StartCoroutine(GetSaveDownloadRoutine());
        }
        private IEnumerator GetSaveDownloadRoutine()
        {
            isNewFetch = true;

            using (UnityWebRequest www = UnityWebRequest.Get(saveDownload_url))
            {
                www.SetRequestHeader("Authorization", authorization);
                yield return www.SendWebRequest();
                Debug.Log(www.result);
                if (www.result == UnityWebRequest.Result.Success)
                {
                    isSuccess = true;
                    errorType = 0;
                    string responseJson = www.downloadHandler.text;
                    ErrorResponseData errorData = JsonUtility.FromJson<ErrorResponseData>(responseJson);
                    if(errorData.success)
                    {
                        if(errorData.data!="null")
                        {
                            SaveDataClass data = JsonUtility.FromJson<SaveDataClass>(responseJson);

                            // Convert the data class properties into a Dictionary<string, int>
                            Dictionary<string, string> dictionary = new Dictionary<string, string>
                       {
                        { "playerScore", data.playerScore },
                        { "achievementScore", data.achievementScore },
                        { "learningProgress", data.learningProgress },
                        { "BossDefeatTime",data.BossDefeatTime}
                            // You can handle BossDefeatTime separately if needed
                            // It is currently stored as a string, so you might need additional parsing.
                         };
                            jsonStrDic = dictionary;
                            GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_SAVE_DOWNLOAD_SUCCESS));
                        }
                        else
                        {
                            GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_SAVE_DOWNLOAD_NULL));
                        }
                        
                      
                    }
                    else
                    {
                        Debug.LogError("Unexpected error in BackendDataManager.GetSaveDownloadRoutine");
                    }
                    
                }
                else
                {
                    HandleErrorMessages(www);
                }
            }
        }
        private void GetCurrentUser()
        {
            StartCoroutine(GetCurrentUserRoutine());
        }
        private IEnumerator GetCurrentUserRoutine()
        {
            // 创建POST请求的表单数据
            WWWForm form = new WWWForm();
            isNewFetch = true;
            using (UnityWebRequest www = UnityWebRequest.Post(currentUser_url, form))
            {
                www.SetRequestHeader("Authorization", authorization);
                yield return www.SendWebRequest();
                Debug.Log(www.result);
                if (www.result == UnityWebRequest.Result.Success)
                {
                    errorType = 0;
                    // 获取API响应数据
                    string responseJson = www.downloadHandler.text;
                    ErrorResponseData data = JsonUtility.FromJson<ErrorResponseData>(responseJson);
                    if(data.data.Equals("Please log in"))
                    {
                        authorization = null;
                        GameEntry.UI.OpenUIForm(EnumUIForm.UILoginForm);
                    }
                    else
                    {
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_LOGED_IN));
                    }
                }
                else
                {
                    HandleErrorMessages(www);
                }
            }
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
            GameEntry.Event.Fire(this, ErrorMessagePopPUpEventArgs.Create());
        }
        [System.Serializable]
        private class RankData
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
        public class LoginData
        {
            public string username;
            public string password;
        }
        [System.Serializable]
        public class ErrorResponseData
        {
            public bool success;
            public int errorCode;
            public string errorMessage;
            public string data;
        }

        [System.Serializable]
        public class LoginResponseData
        {
            public bool success;
            public int errorCode;
            public string errorMessage;
            public DataWrapper data;
        }
        [System.Serializable]
        public class DataWrapper
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
            public List<int> roleIds;
            public List<int> permissionIds;
        }


    }
}
