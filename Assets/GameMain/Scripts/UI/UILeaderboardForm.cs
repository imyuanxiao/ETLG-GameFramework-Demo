using ETLG.Data;
using GameFramework.Event;
using System;
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
        public Button bossButton_AI;
        public Button bossButton_CloudComputing;
        public Button bossButton_Blockchain;
        public Button bossButton_Cybersecurity;
        public Button bossButton_DataScience;
        public Button bossButton_IoT;
        public Transform container = null;
        public GameObject panel;
        private bool isPanelVisible = false;
        //current player
        private DataPlayer dataPlayer;
        //mock players
        private DataPlayer[] mockPlayers;
        private DataSpaceship dataSpaceship;
        private SpaceshipData spaceshipData;
        private List<LeaderboardData> leaderboardData;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            // 获取玩家数据管理器
            achievementButton.onClick.AddListener(OnachievementButtonClick);
            spaceshipButton.onClick.AddListener(OnspaceshipButtonClick);
            bossButton.onClick.AddListener(OnBossButtonClick);
            bossButton_AI.onClick.AddListener(OnBossButtonAIClick);
            bossButton_CloudComputing.onClick.AddListener(OnBossButtonCloudComputingClick);
            bossButton_Blockchain.onClick.AddListener(OnBossButtonBlockchainClick);
            bossButton_Cybersecurity.onClick.AddListener(OnBossButtonCybersecurityClick);
            bossButton_DataScience.onClick.AddListener(OnBossButtonDataScienceClick);
            bossButton_IoT.onClick.AddListener(OnBossButtonIoTClick);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataSpaceship = GameEntry.Data.GetData<DataSpaceship>();
            leaderboardData = new List<LeaderboardData>();

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("Open Leaderboard");
            // open navigationform UI
            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);
            AddMockPlayers();
            //AddOtherPlayerData();
            AddCurrentPlayerData();
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
            leaderboardData.Sort((a, b) => b.AchievementScore.CompareTo(a.AchievementScore));
            showLeaderBoardInfo(Constant.Type.LB_ACHIVEMENT);
        }
        private void OnspaceshipButtonClick()
        {
            s_name.text = "Spaceship Leaderboard";
            leaderboardData.Sort((a, b) => b.SpaceshipScore.CompareTo(a.SpaceshipScore));
            showLeaderBoardInfo(Constant.Type.LB_SPACESHIP);
        }
        private void OnBossButtonClick()
        {
            isPanelVisible = !isPanelVisible;
            panel.SetActive(isPanelVisible);
        }
        private void OnBossButtonAIClick()
        {
            s_name.text = "Boss AI Leaderboard";
            leaderboardData.Sort((a, b) => a.Boss_AI.CompareTo(b.Boss_AI));
            showLeaderBoardInfo(Constant.Type.LB_BOSS_AI);
        }
        private void OnBossButtonCloudComputingClick()
        {
            s_name.text = "Boss Cloud Computing Leaderboard";
            leaderboardData.Sort((a, b) => a.Boss_CloudComputing.CompareTo(b.Boss_CloudComputing));
            showLeaderBoardInfo(Constant.Type.LB_BOSS_CLOUDCOMPUTING);
        }
        private void OnBossButtonBlockchainClick()
        {
            s_name.text = "Boss Blockchain Leaderboard";
            leaderboardData.Sort((a, b) => a.Boss_Blockchain.CompareTo(b.Boss_Blockchain));
            showLeaderBoardInfo(Constant.Type.LB_BOSS_BLOCKCHAIN);
        }
        private void OnBossButtonCybersecurityClick()
        {
            s_name.text = "Boss Cybersecurity Leaderboard";
            leaderboardData.Sort((a, b) => a.Boss_Cybersecurity.CompareTo(b.Boss_Cybersecurity));
            showLeaderBoardInfo(Constant.Type.LB_BOSS_CYBERSECURITY);
        }
        private void OnBossButtonDataScienceClick()
        {
            s_name.text = "Boss Data Science Leaderboard";
            leaderboardData.Sort((a, b) => a.Boss_DataScience.CompareTo(b.Boss_DataScience));
            showLeaderBoardInfo(Constant.Type.LB_BOSS_DATASCIENCE);
        }
        private void OnBossButtonIoTClick()
        {
            s_name.text = "Boss IoT Leaderboard";
            leaderboardData.Sort((a, b) => a.Boss_IoT.CompareTo(b.Boss_IoT));
            showLeaderBoardInfo(Constant.Type.LB_BOSS_IOT);
        }
        private void AddMockPlayers()
        {

            leaderboardData.Add(new LeaderboardData("Player1", 1, 1000, 150, 10.5f, 8.2f, 7.5f, 6.9f, 12.3f, 9.8f));
            leaderboardData.Add(new LeaderboardData("Player2", 2, 800, 30, 8.0f, 7.3f, 6.5f, 5.9f, 10.2f, 8.5f));
            leaderboardData.Add(new LeaderboardData("Player3", 3, 1200, 45, 15.2f, 11.8f, 10.5f, 9.6f, 14.0f, 11.2f));
            leaderboardData.Add(new LeaderboardData("Player4", 4, 900, 10, 9.8f, 8.1f, 7.2f, 6.6f, 11.1f, 9.2f));
            leaderboardData.Add(new LeaderboardData("Player5", 5, 1100, 75, 13.5f, 10.3f, 9.5f, 8.9f, 13.2f, 10.6f));
            leaderboardData.Add(new LeaderboardData("Player6", 6, 750, 300, 7.0f, 6.2f, 5.5f, 5.0f, 9.8f, 7.3f));
            leaderboardData.Add(new LeaderboardData("Player7", 7, 1350, 400, 16.7f, 12.5f, 11.0f, 10.2f, 15.5f, 12.8f));
            leaderboardData.Add(new LeaderboardData("Player8", 8, 950, 250, 10.3f, 8.5f, 7.8f, 7.2f, 11.8f, 9.6f));
            leaderboardData.Add(new LeaderboardData("Player9", 9, 1050, 165, 14.0f, 10.9f, 9.8f, 9.1f, 12.8f, 10.4f));
            leaderboardData.Add(new LeaderboardData("Player10", 10, 800, 55, 8.5f, 7.6f, 6.9f, 6.3f, 10.5f, 8.8f));
        }
        private void AddCurrentPlayerData()
        {
            LeaderboardData data = new LeaderboardData();
            data.Name = "current player";
            data.Id = 123456;
            data.AchievementScore = dataPlayer.GetPlayerData().GetPlayerAchievementPoints();
            data.SpaceshipScore=dataPlayer.GetPlayerData().GetPlayerScore();
            data.Boss_AI = 1000f;
            data.Boss_Blockchain = 1000f;
            data.Boss_CloudComputing = 1000f;
            data.Boss_Cybersecurity = 1000f;
            data.Boss_DataScience = 1000f;
            data.Boss_IoT = 1000f;
            leaderboardData.Add(data);
        }
        private void showLeaderBoardInfo(int Type)
        {
            addRank(Type);
            ItemLeaderboardInfo[] items = container.GetComponentsInChildren<ItemLeaderboardInfo>(true);
            if (items.Length==0)
            {
                for (int i = 0; i < 10; i++)
                {
                    LeaderboardData data = leaderboardData[i];
                    ShowItem<ItemLeaderboardInfo>(EnumItem.UILeaderboardInfo, (item) =>
                    {
                        item.transform.SetParent(container, false);
                        item.GetComponent<ItemLeaderboardInfo>().SetData(data, container, Type);
                    });
                }
            }
            else
            {
                UpdateData(Type);
            }
            
        }
        private void UpdateData(int Type)
        {
            ItemLeaderboardInfo[] items = container.GetComponentsInChildren<ItemLeaderboardInfo>(true);
            int i = 0;
            foreach (ItemLeaderboardInfo subItem in items)
            {
                subItem.UpdateData(leaderboardData[i++],Type);
            }
        }
        private void addRank(int type)
        {
            int currentRank = 1;
            LeaderboardData previousData = null;

            for (int i = 0; i < leaderboardData.Count; i++)
            {
                LeaderboardData currentData = leaderboardData[i];

                // Handle tied rankings
                if (previousData != null && CompareData(type,currentData, previousData) == 0)
                {
                    // do not update rank
                }
                else
                {
                    currentRank = i + 1;
                }

                currentData.Rank = currentRank;
                previousData = currentData;
            }
        }
        private int CompareData(int type,LeaderboardData data1, LeaderboardData data2)
        {
            switch(type)
            {
                case Constant.Type.LB_ACHIVEMENT:
                    return data2.AchievementScore.CompareTo(data1.AchievementScore);
                case Constant.Type.LB_SPACESHIP:
                    return data2.SpaceshipScore.CompareTo(data1.SpaceshipScore);
                case Constant.Type.LB_BOSS_AI:
                    return data1.Boss_AI.CompareTo(data2.Boss_AI);
                case Constant.Type.LB_BOSS_CLOUDCOMPUTING:
                    return data1.Boss_CloudComputing.CompareTo(data2.Boss_CloudComputing);
                case Constant.Type.LB_BOSS_BLOCKCHAIN:
                    return data1.Boss_Blockchain.CompareTo(data2.Boss_Blockchain);
                case Constant.Type.LB_BOSS_CYBERSECURITY:
                    return data1.Boss_Cybersecurity.CompareTo(data2.Boss_Cybersecurity);
                case Constant.Type.LB_BOSS_DATASCIENCE:
                    return data1.Boss_DataScience.CompareTo(data2.Boss_DataScience);
                case Constant.Type.LB_BOSS_IOT:
                    return data1.Boss_IoT.CompareTo(data2.Boss_IoT);
                default:
                    // Handle the default case or throw an exception if necessary
                    throw new ArgumentException("Invalid leaderboard type.");
            }
        }
    }

}