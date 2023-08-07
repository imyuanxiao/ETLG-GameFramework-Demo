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
        public Button bossButton_Final;
        public Transform container = null;
        public GameObject panel;
        public Image arrow;
        private bool isPanelVisible = false;
        private bool isRefresh;
        private int fetchedType;
        private int type;
        public Button returnButton;

        private List<LeaderboardData> rankList;

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
            bossButton_Final.onClick.AddListener(OnBossButtonFinalClick);
            returnButton.onClick.AddListener(OnReturnButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(BackendFetchedEventArgs.EventId, OnBackendFetchedEventArgs);
            Log.Debug("Open Leaderboard");
            // open navigationform UI
            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);
            panel.SetActive(false);
            OnachievementButtonClick();
   
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            if(isRefresh)
            {
                //error
                if (fetchedType == Constant.Type.BACK_RANK_FAILED)
                {
                    
                    
                }

                if (fetchedType == Constant.Type.BACK_RANK_SUCCESS)
                {
                    this.rankList = GameEntry.Data.GetData<DataBackend>().rankList;
                    SetItemsStatus(true);
                    showLeaderBoardInfo();
                }
                if(fetchedType == Constant.Type.BACK_PROFILE_SUCCESS)
                {
                    GameEntry.Event.Fire(this, PlayerZoneUIChangeEventArgs.Create(Constant.Type.UI_OPEN));
                }
                isRefresh = false;
            }
            
            
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(BackendFetchedEventArgs.EventId, OnBackendFetchedEventArgs);
        }
        private void OnachievementButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            s_name.text = "Achievement Leaderboard";
            type = Constant.Type.LB_ACHIVEMENT;
           
            BackendDataManager.Instance.GetRankData(Constant.Type.LB_ACHIVEMENT, 1,10);
            
        }
        private void OnspaceshipButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            s_name.text = "Spaceship Leaderboard";
            type = Constant.Type.LB_SPACESHIP;
            BackendDataManager.Instance.GetRankData(Constant.Type.LB_SPACESHIP,1,10);
        }
        private void OnBossButtonClick()
        {
            if (!isPanelVisible)
            {
                GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            }
            else
            {
                GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            }
            isPanelVisible = !isPanelVisible;
            //箭头垂直翻转180度
            
            Vector3 currentRotation = arrow.transform.rotation.eulerAngles;
            currentRotation.x += 180f;
            arrow.transform.rotation = Quaternion.Euler(currentRotation);
            panel.SetActive(isPanelVisible);
        }
        private void OnBossButtonAIClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            s_name.text = "AI Boss Leaderboard";
            type = Constant.Type.LB_BOSS_AI;
            
            BackendDataManager.Instance.GetRankData(Constant.Type.LB_BOSS_AI,1, 10);
        }
        private void OnBossButtonCloudComputingClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            s_name.text = "Cloud Computing Boss Leaderboard";
            type = Constant.Type.LB_BOSS_CLOUDCOMPUTING;
            
            BackendDataManager.Instance.GetRankData(Constant.Type.LB_BOSS_CLOUDCOMPUTING,1, 10);
        }
        private void OnBossButtonBlockchainClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            s_name.text = "Blockchain Boss Leaderboard";
            type = Constant.Type.LB_BOSS_BLOCKCHAIN;


            BackendDataManager.Instance.GetRankData(Constant.Type.LB_BOSS_BLOCKCHAIN, 1, 10);
            
        }
        private void OnBossButtonCybersecurityClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            s_name.text = "Cybersecurity Boss Leaderboard";
            type = Constant.Type.LB_BOSS_CYBERSECURITY;
            
            
         BackendDataManager.Instance.GetRankData( Constant.Type.LB_BOSS_CYBERSECURITY, 1, 10);
        }
        private void OnBossButtonDataScienceClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            s_name.text = "Data Science Boss Leaderboard";
            type = Constant.Type.LB_BOSS_DATASCIENCE;
            
           
        BackendDataManager.Instance.GetRankData( Constant.Type.LB_BOSS_DATASCIENCE, 1, 10);

        }
        private void OnBossButtonIoTClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            s_name.text = "IoT Boss Leaderboard";
            type = Constant.Type.LB_BOSS_IOT;
            
            BackendDataManager.Instance.GetRankData(Constant.Type.LB_BOSS_IOT, 1, 10);

        }
        private void OnBossButtonFinalClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            s_name.text = "Final Boss Leaderboard";
            type = Constant.Type.LB_BOSS_FINAL;
            
            
          BackendDataManager.Instance.GetRankData( Constant.Type.LB_BOSS_FINAL, 1, 10);

        }

        private void showLeaderBoardInfo()
        {
            addRank();
            
            ItemLeaderboardInfo[] items = container.GetComponentsInChildren<ItemLeaderboardInfo>(true);
            if (items.Length==0)
            {
                //先排前10名
                for (int i = 0; i < rankList.Count; i++)
                {
                    LeaderboardData data = rankList[i];
                    ShowItem<ItemLeaderboardInfo>(EnumItem.UILeaderboardInfo, (item) =>
                    {
                        item.transform.SetParent(container, false);
                        item.GetComponent<ItemLeaderboardInfo>().SetData(data, container, type);
                    });
                }
            }
            else
            {
                UpdateData();
                
            }
            
        }
        private void UpdateData()
        {
            
            ItemLeaderboardInfo[] items = container.GetComponentsInChildren<ItemLeaderboardInfo>(true);
            int currentItemCount = items.Length;
            int targetItemCount = rankList.Count;
            if (targetItemCount > currentItemCount)
            {
                for (int i = currentItemCount; i < targetItemCount; i++)
                {
                    LeaderboardData data = rankList[i];
                    ShowItem<ItemLeaderboardInfo>(EnumItem.UILeaderboardInfo, (item) =>
                    {
                        item.transform.SetParent(container, false);
                        item.GetComponent<ItemLeaderboardInfo>().SetData(data, container, type);
                    });
                }
            }

            // 如果要隐藏多余的
            else if (targetItemCount < currentItemCount)
            {
                for (int i = targetItemCount; i < currentItemCount; i++)
                {
                    items[i].gameObject.SetActive(false);
                }
            }

            // 更新已显示的数据
            int dataIndex = 0;
            addRank();
            foreach (ItemLeaderboardInfo subItem in items)
            {
                if (dataIndex < targetItemCount)
                {
                    subItem.UpdateData(rankList[dataIndex], type);
                    subItem.gameObject.SetActive(true);
                    dataIndex++;
                }
                else
                {
                    // 隐藏多余的
                    subItem.gameObject.SetActive(false);
                }
            }
        }
        private void addRank()
        {
            int currentRank = 1;
            LeaderboardData previousData = null;

            for (int i = 0; i < rankList.Count; i++)
            {
                LeaderboardData currentData = rankList[i];

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
                    return data2.data.CompareTo(data1.data);
                case Constant.Type.LB_SPACESHIP:
                    return data2.data.CompareTo(data1.data);
                case Constant.Type.LB_BOSS_AI:
                    return data1.data.CompareTo(data2.data);
                case Constant.Type.LB_BOSS_CLOUDCOMPUTING:
                    return data1.data.CompareTo(data2.data);
                case Constant.Type.LB_BOSS_BLOCKCHAIN:
                    return data1.data.CompareTo(data2.data);
                case Constant.Type.LB_BOSS_CYBERSECURITY:
                    return data1.data.CompareTo(data2.data);
                case Constant.Type.LB_BOSS_DATASCIENCE:
                    return data1.data.CompareTo(data2.data);
                case Constant.Type.LB_BOSS_IOT:
                    return data1.data.CompareTo(data2.data);
                case Constant.Type.LB_BOSS_FINAL:
                    return data1.data.CompareTo(data2.data);
                default:
                    // Handle the default case or throw an exception if necessary
                    throw new ArgumentException("Invalid leaderboard type.");
            }
        }
       public void SetItemsStatus(bool status)
        {
            ItemLeaderboardInfo[] items = container.GetComponentsInChildren<ItemLeaderboardInfo>(!status);
            foreach(ItemLeaderboardInfo subItem in items)
            {
                subItem.gameObject.SetActive(status);
            }
        }
        private void OnReturnButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }
        public void OnBackendFetchedEventArgs(object sender, GameEventArgs e)
        {
            BackendFetchedEventArgs ne = (BackendFetchedEventArgs)e;
            if (ne == null)
                return;
            isRefresh = true;
            fetchedType = ne.Type;

        }
    }

}