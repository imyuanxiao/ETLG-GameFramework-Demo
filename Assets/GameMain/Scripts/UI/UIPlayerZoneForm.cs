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
        public TextMeshProUGUI s_planetNum = null;
        public TextMeshProUGUI s_name = null;
        public TextMeshProUGUI s_ID = null;
        public LeaderboardData leaderboardData;
        public Button closeButton;
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
            showContent();
        }
        private void showContent()
        {
            leaderboardData = LeaderboardManager.Instance.leaderboardData;
            planetNum = LeaderboardManager.Instance.planetNum;
            s_achievement.text = leaderboardData.AchievementScore.ToString();
            s_spaceship.text = leaderboardData.SpaceshipScore.ToString();
            s_planetNum.text = planetNum.ToString();
            s_name.text = leaderboardData.Name;
            s_ID.text = leaderboardData.Id.ToString();
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

