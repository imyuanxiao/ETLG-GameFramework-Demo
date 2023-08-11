using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using ETLG.Data;
using UnityEngine.Networking;
using ETLG;
using Newtonsoft.Json;
namespace ETLG
{
    public class BackendDataManager : Singleton<BackendDataManager>
    {
        //用于回调函数
        public delegate void IsLoginDetectionCallback(bool isLoggedIn);

        public int connectType;

        private Dictionary<string, string> uploadJsonStrDic;
        private void OnEnable()
        {
            connectType = Constant.Type.BACK_LOCAL;
            //connectType = Constant.Type.BACK_REOMTE;
        }
        public void GetRankData(int type, int current, int pageSize)
        {
            StartCoroutine(GetRankDataRoutine(type, current, pageSize));
        }
       
        private IEnumerator GetRankDataRoutine(int type, int current, int pageSize)
        {
            RankRequest requestData = new RankRequest
            {
                type= type,
                current = current,
                pageSize = pageSize
            };

            string jsonData = JsonUtility.ToJson(requestData);
            using (UnityWebRequest www = UnityWebRequest.Post(GameEntry.Data.GetData<DataBackend>().GetLeaderboardUrl(), jsonData))
                {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                www.uploadHandler.Dispose();
                www.downloadHandler.Dispose();
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.Success)
                {
                    string responseJson = www.downloadHandler.text;

                    ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseJson);

                    if (responseData.success)
                    {
                        RankResponse rankResponse = JsonUtility.FromJson<RankResponse>(responseJson);
                        List<LeaderboardData> list = new List<LeaderboardData>();
                        foreach (RankRecord record in rankResponse.data.records)
                        {
                            LeaderboardData data = new LeaderboardData(record.userId,record.nickName,record.data);
                            list.Add(data);
                        }
                        GameEntry.Data.GetData<DataBackend>().rankList = list;
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_RANK_SUCCESS));
                    }
                    else
                    {
                        GameEntry.Data.GetData<DataBackend>().message = responseData.data;
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_RANK_FAILED));
                    }
                }
                else
                {
                    HandleErrorMessages(www);
                    GameEntry.Event.Fire(this, ErrorMessagePopPUpEventArgs.Create());
                }
                www.Dispose();

            } 

        }
        private void GetLogIn(string userName, string password)
        {

            StartCoroutine(PostLogInUserRoutine(userName, password));

        }
        private IEnumerator PostLogInUserRoutine(string userName, string password)
        {
            LoginData loginData = new LoginData
            {
                username = userName,
                password = password
            };

            string jsonData = JsonUtility.ToJson(loginData);

            using (UnityWebRequest request = UnityWebRequest.Post(GameEntry.Data.GetData<DataBackend>().GetLoginUrl(), jsonData))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler.Dispose();
                request.downloadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                var asyncOperation = request.SendWebRequest();

                yield return asyncOperation;

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string responseJson = request.downloadHandler.text;
                    LoginResponseData responseData = JsonUtility.FromJson<LoginResponseData>(responseJson);
                    if (responseData.success)
                    {
                        GameEntry.Data.GetData<DataBackend>().currentUser.avatar = responseData.data.userVO.avatar;
                        GameEntry.Data.GetData<DataBackend>().currentUser.id = responseData.data.userVO.id;
                        GameEntry.Data.GetData<DataBackend>().currentUser.nickName = responseData.data.userVO.nickName;
                        GameEntry.Data.GetData<DataBackend>().currentUser.username = responseData.data.userVO.username;
                        GameEntry.Data.GetData<DataBackend>().authorization = responseData.data.token;

                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_LOGIN_SUCCESS));
                        OnLoginSuccess();
                    }
                    else
                    {
                        ResponseData erroResponseData = JsonUtility.FromJson<ResponseData>(responseJson);
                        GameEntry.Data.GetData<DataBackend>().message = erroResponseData.data;
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_LOGIN_FAILED));
                    }

                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                    HandleErrorMessages(request);
                }
                request.disposeDownloadHandlerOnDispose = true;
                request.disposeUploadHandlerOnDispose = true;
                request.Dispose();
            }
        }
        private void OnLoginSuccess()
        {
            switch (GameEntry.Data.GetData<DataBackend>().loginType)
            {
                case Constant.Type.BACK_PROFILE:
                    GetUserProfileByUserId(GameEntry.Data.GetData<DataBackend>().currentUser.id);
                    break;
                case Constant.Type.BACK_SAVE_DOWNLOAD:
                    StartCoroutine(GetSaveDownloadRoutine());
                    break;
                case Constant.Type.BACK_SAVE_UPLOAD:
                    StartCoroutine(PostSaveUpLoadRoutine()) ;
                    break;
            }
        }
        public void HandleProfileUpdate(int avatar, string nickName)
        {
            IsLoginDetection((isLoggedIn) =>
            {
                if (isLoggedIn)
                {
                    StartCoroutine(PutProfileUpdateRoutine(avatar, nickName));
                }
                else
                {
                    if (GameEntry.UI.HasUIForm(EnumUIForm.UILoginForm))
                        GameEntry.UI.CloseUIForm((int)EnumUIForm.UILoginForm);
                    GameEntry.UI.OpenUIForm(EnumUIForm.UILoginForm);
                }
            });
            
        }
        private IEnumerator PutProfileUpdateRoutine(int avatar, string nickName)
        {
            UserInfo updateData = new UserInfo
            {
                nickName = nickName,
                avatar = avatar
            };
            string jsonData = JsonUtility.ToJson(updateData);
            using (UnityWebRequest request = UnityWebRequest.Put(GameEntry.Data.GetData<DataBackend>().GetProfileUpdateUrl(), jsonData))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler.Dispose();
                request.downloadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", GameEntry.Data.GetData<DataBackend>().authorization);

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string responseJson = request.downloadHandler.text;
                    // Process the response data
                    ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseJson);

                    if (responseData.success)
                    {
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_PROFILE_UPDATE_SUCCESS));
                    }
                    else
                    {
                        GameEntry.Data.GetData<DataBackend>().message = responseData.data;
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_PROFILE_UPDATE_FAILED));
                    }

                    Debug.Log("PUT Request Successful: " + responseJson);
                }
                else
                {
                    // HandleErrorMessages(request);
                    Debug.LogError("PUT Request Failed: " + request.error);
                }
                
                request.disposeDownloadHandlerOnDispose = true;
                request.disposeUploadHandlerOnDispose = true;
            }
        }
        public void HandleProfilePassword(string oldPassword, string newPassword)
        {
            IsLoginDetection((isLoggedIn) =>
            {
                if (isLoggedIn)
                {
                    StartCoroutine(PutProfilePasswordRoutine(oldPassword, newPassword));
                }
                else
                {
                    if (GameEntry.UI.HasUIForm(EnumUIForm.UILoginForm))
                    {
                        GameEntry.UI.CloseUIForm((int)EnumUIForm.UILoginForm);
                        GameEntry.UI.OpenUIForm(EnumUIForm.UILoginForm);
                    }
                    else
                    {
                        GameEntry.UI.OpenUIForm(EnumUIForm.UILoginForm);
                    }
                }
            });
        }
        private IEnumerator PutProfilePasswordRoutine(string oldPassword, string newPassword)
        {
            ProfilePassword updatePwd = new ProfilePassword
            {
                oldPassword = oldPassword,
                newPassword = newPassword
            };

            string jsonData = JsonUtility.ToJson(updatePwd);

            using (UnityWebRequest request = UnityWebRequest.Put(GameEntry.Data.GetData<DataBackend>().GeProfilePasswordUrl(), jsonData))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                //
                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler.Dispose();
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", GameEntry.Data.GetData<DataBackend>().authorization);
                yield return request.SendWebRequest(); ;
                if (request.result == UnityWebRequest.Result.Success)
                {
                    string responseJson = request.downloadHandler.text;
                    ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseJson);

                    if (responseData.success)
                    {
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_PROFILE_PASSWORD_SUCCESS));
                    }
                    else
                    {
                        GameEntry.Data.GetData<DataBackend>().message = responseData.data;
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_PROFILE_PASSWORD_FAILED));
                    }
                }
                else
                {
                    HandleErrorMessages(request);
                }
                request.disposeDownloadHandlerOnDispose = true;
                request.disposeUploadHandlerOnDispose = true;
            }
        }
        public void HandleSaveUpLoad(Dictionary<string, string> jsonStrDic)
        {
            this.uploadJsonStrDic = jsonStrDic;
            GameEntry.Data.GetData<DataBackend>().loginType = Constant.Type.BACK_SAVE_UPLOAD;
            IsLoginDetection((isLoggedIn) =>
            {
                if (isLoggedIn)
                {
                    StartCoroutine(PostSaveUpLoadRoutine());
                }
                else
                {
                    if (GameEntry.UI.HasUIForm(EnumUIForm.UILoginForm))
                        GameEntry.UI.CloseUIForm((int)EnumUIForm.UILoginForm);
                    GameEntry.UI.OpenUIForm(EnumUIForm.UILoginForm);
                }
            });
            jsonStrDic = null;
        }
        private IEnumerator PostSaveUpLoadRoutine()
        {

            string jsonData = JsonConvert.SerializeObject(uploadJsonStrDic);

            using (UnityWebRequest request = UnityWebRequest.Post(GameEntry.Data.GetData<DataBackend>().GetSaveUploadUrl(), jsonData))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler.Dispose();
                request.downloadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", GameEntry.Data.GetData<DataBackend>().authorization);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string responseJson = request.downloadHandler.text;
                    ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseJson);
                    if (responseData.success)
                    {
                        Debug.Log("upload success");
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_SAVE_UPLOAD_SUCCESS));
                    }
                    else
                    {
                        Debug.Log(responseData.data);
                        GameEntry.Data.GetData<DataBackend>().message = responseData.data;
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_SAVE_UPLOAD_FAILED));
                    }

                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                    HandleErrorMessages(request);
                }
                request.disposeDownloadHandlerOnDispose = true;
                request.disposeUploadHandlerOnDispose = true;
            }
        }
        public void HandleLoad()
        {
            //先判断是否登录
            //如果没登陆就开登录UI
            GameEntry.Data.GetData<DataBackend>().loginType = Constant.Type.BACK_SAVE_DOWNLOAD;
            IsLoginDetection((isLoggedIn) =>
            {
                if (isLoggedIn)
                {
                    StartCoroutine(GetSaveDownloadRoutine());
                }
                else
                {
                    if (GameEntry.UI.HasUIForm(EnumUIForm.UILoginForm))
                        GameEntry.UI.CloseUIForm((int)EnumUIForm.UILoginForm);
                    GameEntry.UI.OpenUIForm(EnumUIForm.UILoginForm);
                }
            });
        }
        public void HandleProfile()
        {
            GameEntry.Data.GetData<DataBackend>().loginType = Constant.Type.BACK_PROFILE;
            //登录之后再查userprofile
            IsLoginDetection((isLoggedIn) =>
            {
                if (isLoggedIn)
                {
                    GetUserProfileByUserId(GameEntry.Data.GetData<DataBackend>().currentUser.id);
                }
                else
                {
                    if (GameEntry.UI.HasUIForm(EnumUIForm.UILoginForm))
                        GameEntry.UI.CloseUIForm((int)EnumUIForm.UILoginForm);
                    GameEntry.UI.OpenUIForm(EnumUIForm.UILoginForm);
                }
            });


        }
        public void HandleLogIn(string userName, string password)
        {
            GetLogIn(userName, password);

        }
        public void HandleRegister(string userName, string password)
        {
            GetRegister(userName, password);
        }
        private void GetRegister(string userName, string password)
        {
            GameEntry.Data.GetData<DataBackend>().message = null;

            StartCoroutine(PostRegisterRoutine(userName, password));
        }
        private IEnumerator PostRegisterRoutine(string userName, string password)
        {
            LoginData loginData = new LoginData
            {
                username = userName,
                password = password
            };

            string jsonData = JsonUtility.ToJson(loginData);

            using (UnityWebRequest request = UnityWebRequest.Post(GameEntry.Data.GetData<DataBackend>().GetRegisterUrl(),jsonData))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler.Dispose();
                request.downloadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                var asyncOperation = request.SendWebRequest();

                yield return asyncOperation;

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string responseJson = request.downloadHandler.text;
                    ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseJson);
                    if (responseData.success)
                    {
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_REGISTER_SUCCESS));
                    }
                    else
                    {
                        GameEntry.Data.GetData<DataBackend>().message = responseData.data;
                        GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_REGISTER_FAILED));
                    }

                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                    HandleErrorMessages(request);
                }
                request.disposeDownloadHandlerOnDispose = true;
                request.disposeUploadHandlerOnDispose = true;
            }

        }
        public void IsLoginDetection(IsLoginDetectionCallback callback)
        {
            if (GameEntry.Data.GetData<DataBackend>().authorization == null)
            {
                GameEntry.Data.GetData<DataBackend>().authorization = null;
                callback(false);
            }
            else
            {
                GetCurrentUser(callback);
            }
            
        }
        private IEnumerator GetSaveDownloadRoutine()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(GameEntry.Data.GetData<DataBackend>().GetSaveDownloadUrl()))
            {
                www.SetRequestHeader("Authorization", GameEntry.Data.GetData<DataBackend>().authorization);
                yield return www.SendWebRequest();
                Debug.Log(www.result);
                if (www.result == UnityWebRequest.Result.Success)
                {
                    string responseJson = www.downloadHandler.text;
                    ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseJson);
                    
                    if (responseData.success)
                    {
                        if(!string.IsNullOrEmpty(responseData.data))
                        {
                           
                            GameEntry.Data.GetData<DataBackend>().downLoadjsonStrDic = JsonConvert.DeserializeObject<Dictionary<string,string>>(responseData.data);
                            GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_SAVE_DOWNLOAD_SUCCESS));
                        }
                        else
                        {
                            //if it is null , tell player to upload data
                            GameEntry.Data.GetData<DataAlert>().AlertType = Constant.Type.ERROR_DATA;
                            GameEntry.Data.GetData<DataAlert>().isFromBackend = true;
                            GameEntry.Event.Fire(this, ErrorMessagePopPUpEventArgs.Create());
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
                www.Dispose();
            }
        }
        public void GetUserProfileByUserId(int id)
        {
            StartCoroutine(GetUserProfileByUserIdRoutine(id));
        }
        private IEnumerator GetUserProfileByUserIdRoutine(int id)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(GameEntry.Data.GetData<DataBackend>().GetGetProfileByIdUrl() + id))
            {
                //www.SetRequestHeader("Authorization", GameEntry.Data.GetData<DataBackend>().authorization);
                yield return www.SendWebRequest();
                Debug.Log(www.result);
                if (www.result == UnityWebRequest.Result.Success)
                {
                    string responseJson = www.downloadHandler.text;
                    UserProfileResponseData responseData = JsonUtility.FromJson<UserProfileResponseData>(responseJson);
                    if (responseData.success)
                    {
                        if (responseData.data!=null && !string.IsNullOrEmpty( responseData.data.achievement))
                        {
                            GameEntry.Data.GetData<DataBackend>().userProfile.achievement = responseData.data.achievement;
                            GameEntry.Data.GetData<DataBackend>().userProfile.learningProgress = responseData.data.learningProgress;
                            GameEntry.Data.GetData<DataBackend>().userProfile.nickName = responseData.data.nickName;
                            GameEntry.Data.GetData<DataBackend>().userProfile.playerScore = responseData.data.playerScore;
                            GameEntry.Data.GetData<DataBackend>().userProfile.avatar = responseData.data.avatar;
                            GameEntry.Data.GetData<DataBackend>().userProfile.boss1 = responseData.data.boss1;
                            GameEntry.Data.GetData<DataBackend>().userProfile.boss2 = responseData.data.boss2;
                            GameEntry.Data.GetData<DataBackend>().userProfile.boss3 = responseData.data.boss3;
                            GameEntry.Data.GetData<DataBackend>().userProfile.boss4 = responseData.data.boss4;
                            GameEntry.Data.GetData<DataBackend>().userProfile.boss5 = responseData.data.boss5;
                            GameEntry.Data.GetData<DataBackend>().userProfile.boss6 = responseData.data.boss6;
                            GameEntry.Data.GetData<DataBackend>().userProfile.boss7 = responseData.data.boss7;
                            GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_PROFILE_SUCCESS));
                        }
                        else
                        {
                            GameEntry.Event.Fire(this, BackendFetchedEventArgs.Create(Constant.Type.BACK_PROFILE_FAILED));
                        }
                    }
                    else
                    {
                        //if it is null , tell player to upload data
                        GameEntry.Data.GetData<DataAlert>().AlertType = Constant.Type.ERROR_DATA;
                        GameEntry.Data.GetData<DataAlert>().isFromBackend = true;
                        GameEntry.Event.Fire(this, ErrorMessagePopPUpEventArgs.Create());
                    }
                }
                else
                {
                    HandleErrorMessages(www);
                }
                www.Dispose();
            }
        }
        private void GetCurrentUser(IsLoginDetectionCallback callback)
        {
            StartCoroutine(GetCurrentUserRoutine(callback));
        }
        private IEnumerator GetCurrentUserRoutine(IsLoginDetectionCallback callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(GameEntry.Data.GetData<DataBackend>().GetCurrentUserUrl()))
            {
                www.SetRequestHeader("Authorization", GameEntry.Data.GetData<DataBackend>().authorization);
                yield return www.SendWebRequest();
                Debug.Log(www.result);
                if (www.result == UnityWebRequest.Result.Success)
                {
                    // 获取API响应数据
                    string responseJson = www.downloadHandler.text;
                    ResponseData data = JsonUtility.FromJson<ResponseData>(responseJson);
                    if(data.data.Equals("Please log in"))
                    {
                        GameEntry.Data.GetData<DataBackend>().authorization = null;
                        callback(false);
                    }
                    else
                    {
                        CurrentUserResponseData responseData = JsonUtility.FromJson<CurrentUserResponseData>(responseJson);
                        GameEntry.Data.GetData<DataBackend>().currentUser.avatar = responseData.data.avatar;
                        GameEntry.Data.GetData<DataBackend>().currentUser.id = responseData.data.id;
                        GameEntry.Data.GetData<DataBackend>().currentUser.nickName = responseData.data.nickName;
                        GameEntry.Data.GetData<DataBackend>().currentUser.username = responseData.data.username;
                        callback(true);
                    }
                }
                else
                {
                    HandleErrorMessages(www);
                    callback(false);
                }
            }
        }
        private void HandleErrorMessages(UnityWebRequest www)
        {
            Debug.LogError("Error: " + www.error);
            if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                GameEntry.Data.GetData<DataAlert>().AlertType = Constant.Type.ERROR_NETWORK;
                
            }
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                GameEntry.Data.GetData<DataAlert>().AlertType = Constant.Type.ERROR_SERVER;
            }
            if (www.result == UnityWebRequest.Result.DataProcessingError)
            {
                GameEntry.Data.GetData<DataAlert>().AlertType = Constant.Type.ERROR_DATA;
            }
            GameEntry.Data.GetData<DataAlert>().isFromBackend = true;
            GameEntry.Event.Fire(this, ErrorMessagePopPUpEventArgs.Create());
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
        [System.Serializable]
        public class RankRecord
        {
            public int userId;
            public string nickName;
            public float data;
        }
        [System.Serializable]
        public class RankDataWrapper
        {
            public List<RankRecord> records;
        }
        [System.Serializable]
        public class RankRequest
        {
            public int type;
            public int current;
            public int pageSize;
        }
        [System.Serializable]
        public class RankResponse
        {
            public bool success;
            public int errorCode;
            public string errorMessage;
            public RankDataWrapper data;
        }
        [System.Serializable]
        public class KeyValuePair
        {
            string key;
            string value;
            public KeyValuePair(string key,string value)
            {
                this.key = key;
                this.value = value;
            }
        }

    }
}
