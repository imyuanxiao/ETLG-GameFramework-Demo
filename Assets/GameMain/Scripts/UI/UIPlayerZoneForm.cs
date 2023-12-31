using ETLG.Data;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
namespace ETLG
{
    public class UIPlayerZoneForm : UGuiFormEx
    {
        public TextMeshProUGUI s_spaceship = null;
        public TextMeshProUGUI s_achievement = null;
        public TextMeshProUGUI s_learningProgress = null;
        public TextMeshProUGUI s_name = null;
        public LeaderboardData leaderboardData;
        public Button closeButton;
        public RawImage avatar;
        public int planetNum;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            closeButton.onClick.AddListener(onCloseButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("Open Player Zone");
            BackendDataManager.Instance.GetUserProfileByUserId(GameEntry.Data.GetData<DataBackend>().selectedRank.Id) ;
            showContent();
        }
        private void showContent()
        {
            s_achievement.text = GameEntry.Data.GetData<DataBackend>().userProfile.achievement;
            s_spaceship.text = GameEntry.Data.GetData<DataBackend>().userProfile.playerScore;
            s_learningProgress.text = UIFloatString.FloatToString(GameEntry.Data.GetData<DataBackend>().userProfile.learningProgress);
            s_name.text = GameEntry.Data.GetData<DataBackend>().userProfile.nickName;
            Debug.Log(GameEntry.Data.GetData<DataBackend>().userProfile.avatar);
            if (GameEntry.Data.GetData<DataBackend>().userProfile.avatar >= 1000)
                avatar.texture = Resources.Load<Texture>(AssetUtility.GetPlayerAvatar(GameEntry.Data.GetData<DataBackend>().userProfile.avatar.ToString()));
            else
            {
                avatar.texture = Resources.Load<Texture>(AssetUtility.GetPlayerAvatar("1000"));
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
        private void onCloseButtonClick()
        {
            GameEntry.Event.Fire(this, PlayerZoneUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }
        public void updateData()
        {
            showContent();
        }
    }
}

