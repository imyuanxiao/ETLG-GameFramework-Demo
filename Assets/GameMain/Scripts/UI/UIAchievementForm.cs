using ETLG.Data;
using GameFramework.Event;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
namespace ETLG
{
    public class UIAchievementForm : UGuiFormEx
    {
        // Type name
        public TextMeshProUGUI s_name_1 = null;
        public TextMeshProUGUI s_name_2 = null;
        public TextMeshProUGUI s_name_4 = null;
        public TextMeshProUGUI s_name_5 = null;
        public TextMeshProUGUI s_name_6 = null;
        public TextMeshProUGUI s_points = null;
        public TextMeshProUGUI s_unlockedNumber = null;
        public TextMeshProUGUI s_total = null;
        public Transform content_1 = null;
        public Transform content_2 = null;
        public Transform content_4 = null;
        public Transform content_5 = null;
        public Transform content_6 = null;
        public Button returnButton;
        // initial attrs

        private DataPlayer dataPlayer;
        private DataAchievement dataAchievement;

        public bool refresh;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            // 获取玩家数据管理器
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            returnButton.onClick.AddListener(OnReturnButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);
            Log.Debug("Open Achievement");
            // open navigationform UI
            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);
            
            showContent();
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (refresh)
            {
                Log.Debug("Refresh Achievement");
                showContent();
                refresh = false;
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);

        }
        private void showContent()
        {
            //load achievement name
            s_name_1.text = "Learning";
            s_name_2.text = "Resource";
            s_name_4.text = "Interstellar";
            s_name_5.text = "Battle";
            s_name_6.text = "Spaceship";

            s_unlockedNumber.text = dataPlayer.GetPlayerData().GetUnlockedAchievementCount().ToString();
            s_total.text=dataAchievement.GetAchievementCount().ToString();
            AchievementData[] achievementDatas = dataAchievement.GetAllNewData();
            Dictionary<int, int> playerAchievement = dataPlayer.GetPlayerData().GetPlayerAchievement();
            s_points.text = dataPlayer.GetPlayerData().GetPlayerAchievementPoints().ToString();
            if(refresh)
            {
                Transform[] containers = new Transform[] { content_1, content_2, content_4, content_5, content_6 };
                foreach (Transform container in containers)
                {
                    ItemAchievementIcon[] items = container.GetComponentsInChildren<ItemAchievementIcon>(true);
                    foreach (ItemAchievementIcon item in items)
                    {
                        item.UpdateData();
                    }
                }
            }
            else
            {
                //show locked achievements
                foreach (AchievementData data in achievementDatas)
                {
                    if (playerAchievement.ContainsKey(data.Id) && dataAchievement.isMaxLevel(data.Id, playerAchievement[data.Id]))
                    {
                        continue;
                    }
                    ShowAchievementByType(data);
                }
                //show unlocked achievements
                foreach (KeyValuePair<int, int> achievement in playerAchievement)
                {
                    if (!dataAchievement.isMaxLevel(achievement.Key, achievement.Value))
                    {
                        continue;
                    }
                    ShowAchievementByType(dataAchievement.GetDataById(achievement.Key));
                }
            }
        }
        private void ShowAchievementByType(AchievementData data)
        {
            switch (data.TypeId)
            {
                case Constant.Type.ACHV_LEARN:
                    ShowAchievements(content_1, data.Id);
                    break;
                case Constant.Type.ACHV_RESOURCE:
                    ShowAchievements(content_2, data.Id);
                    break;
                case Constant.Type.ACHV_INTERSTELLAR:
                    ShowAchievements(content_4, data.Id);
                    break;
                case Constant.Type.ACHV_BATTLE:
                    ShowAchievements(content_5, data.Id);
                    break;
                case Constant.Type.ACHV_SPACESHIP:
                    ShowAchievements(content_6, data.Id);
                    break;
                default:
                    break;
            }
        }
        private void ShowAchievements(Transform container,int id)
        {
            if (!refresh)
            {
                ShowItem<ItemAchievementIcon>(EnumItem.AchievementIcon, (item) =>
              {
                  item.transform.SetParent(container, false);

                  item.GetComponent<ItemAchievementIcon>().SetAchievementData(id, container);
              });
              }
        }

        private void OnAchievementPoPUp(object sender, GameEventArgs e)
        {
            AchievementPopUpEventArgs ne = (AchievementPopUpEventArgs)e;
            if (ne == null)
                return;
            this.refresh = true;
        }
        private void OnReturnButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }
    }
}