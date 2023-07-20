using ETLG.Data;
using GameFramework.Event;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
namespace ETLG
{
    public class UILeaderboardForm : UGuiFormEx
    {
        // Type name
        public TextMeshProUGUI s_name = null;
        public Button achievementButton;
        public Button spaceshipButton;
        public Button bossButton;
        public Button bossButton_1;
        public Button bossButton_2;
        public Button bossButton_3;
        public Button bossButton_4;
        public Button bossButton_5;
        public Button bossButton_6;
        public GameObject panel;
        private bool isPanelVisible = false;
        private DataPlayer dataPlayer;
        private DataAchievement dataAchievement;


        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            // 获取玩家数据管理器
            achievementButton.onClick.AddListener(OnachievementButtonClick);
            spaceshipButton.onClick.AddListener(OnspaceshipButtonClick);
            bossButton.onClick.AddListener(OnBossButtonClick);
            bossButton_1.onClick.AddListener(OnBossButtonClick_1);
            bossButton_2.onClick.AddListener(OnBossButtonClick_2);
            bossButton_3.onClick.AddListener(OnBossButtonClick_3);
            bossButton_4.onClick.AddListener(OnBossButtonClick_4);
            bossButton_5.onClick.AddListener(OnBossButtonClick_5);
            bossButton_6.onClick.AddListener(OnBossButtonClick_6);
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("Open Leaderboard");
            // open navigationform UI
            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);
            showContent();
        }
       
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);


        }
        private void showContent()
        {
            OnachievementButtonClick();
        }
        private void OnachievementButtonClick()
        {
            s_name.text = "Achievment Leaderboard";
        }
        private void OnspaceshipButtonClick()
        {
            s_name.text = "Spaceship Leaderboard";
        }
        private void OnBossButtonClick()
        {
            isPanelVisible = !isPanelVisible;
            panel.SetActive(isPanelVisible);
        }
        private void OnBossButtonClick_1()
        {
            s_name.text = "Boss_1 Leaderboard";
        }
        private void OnBossButtonClick_2()
        {
            s_name.text = "Boss_2 Leaderboard";
        }
        private void OnBossButtonClick_3()
        {
            s_name.text = "Boss_3 Leaderboard";
        }
        private void OnBossButtonClick_4()
        {
            s_name.text = "Boss_4 Leaderboard";
        }
        private void OnBossButtonClick_5()
        {
            s_name.text = "Boss_5 Leaderboard";
        }
        private void OnBossButtonClick_6()
        {
            s_name.text = "Boss_6 Leaderboard";
        }
    }

    }