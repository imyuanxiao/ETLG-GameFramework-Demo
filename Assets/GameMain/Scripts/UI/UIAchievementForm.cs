using ETLG.Data;
using GameFramework.Event;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;
namespace ETLG
{
    public class UIAchievementForm : UGuiFormEx
    {
        // Type name
        public TextMeshProUGUI s_name_1 = null;
        public TextMeshProUGUI s_name_2 = null;
        public TextMeshProUGUI s_name_3 = null;
        public TextMeshProUGUI s_name_4 = null;
        public TextMeshProUGUI s_name_5 = null;
        public TextMeshProUGUI s_name_6 = null;
        public TextMeshProUGUI s_name_7 = null;
        public TextMeshProUGUI s_name_8 = null;
        public TextMeshProUGUI s_name_9 = null;
        public TextMeshProUGUI s_name_10 = null;
        public TextMeshProUGUI s_points = null;
        public TextMeshProUGUI s_unlockedNumber = null;
        public TextMeshProUGUI s_total = null;
        public Transform content_1 = null;
        public Transform content_2 = null;
        public Transform content_3 = null;
        public Transform content_4 = null;
        public Transform content_5 = null;
        public Transform content_6 = null;
        public Transform content_7 = null;
        public Transform content_8 = null;
        public Transform content_9 = null;
        public Transform content_10 = null;

        // initial attrs

        private DataPlayer dataPlayer;
        private DataAchievement dataAchievement;
        // 实体加载器
        private EntityLoader entityLoader;

        public bool refresh;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            // 获取玩家数据管理器

            dataAchievement = GameEntry.Data.GetData<DataAchievement>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            entityLoader = EntityLoader.Create(this);
            
            GameEntry.Event.Subscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("Open Achievement");
            // open navigationform UI
            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);
            if (!dataPlayer.GetPlayerData().GetPlayerAchievement().ContainsKey(5001))
            {
                GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(5001, 9999));
            }
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
            //加载成就名称
            s_name_1.text = "Quiz";
            s_name_2.text = "Resource";
            s_name_3.text = "Knowledge Base";
            s_name_4.text = "Interstellar";
            s_name_5.text = "Battle";
            s_name_6.text = "Spaceship";
            s_name_7.text = "Login";
            s_name_8.text = "Leadership";
            s_name_9.text = "Achievement";
            s_name_10.text = "Hidden";

            s_unlockedNumber.text = dataPlayer.GetPlayerData().GetUnlockedAchievementCount().ToString();
            s_total.text=dataAchievement.GetAchievementCount().ToString();
            AchievementData[] achievementDatas = dataAchievement.GetAllNewData();
            Dictionary<int, int> playerAchievement = dataPlayer.GetPlayerData().GetPlayerAchievement();
            s_points.text = dataPlayer.GetPlayerData().GetPlayerAchievementPoints().ToString();
            //show locked achievements
            foreach (AchievementData data in achievementDatas)
            {
                
                if(playerAchievement.ContainsKey(data.Id) && dataAchievement.isMaxLevel(data.Id,playerAchievement[data.Id]))
                {
                    continue;
                }
                showAchievementByType(data);
            }
            //show unlocked achievements
            foreach(KeyValuePair<int,int> achievement in playerAchievement)
            {
                if(!dataAchievement.isMaxLevel(achievement.Key, achievement.Value))
                    {
                    continue;
                }
                showAchievementByType(dataAchievement.GetDataById(achievement.Key));
            }
        }
        private void showAchievementByType(AchievementData data)
        {
            switch (data.TypeId)
            {
                case Constant.Type.ACHV_QUIZ:
                    showAchievements(content_1, data.Id);
                    break;
                case Constant.Type.ACHV_RESOURCE:
                    showAchievements(content_2, data.Id);
                    break;
                case Constant.Type.ACHV_KNOWLEDGE_BASE:
                    showAchievements(content_3, data.Id);
                    break;
                case Constant.Type.ACHV_INTERSTELLAR:
                    showAchievements(content_4, data.Id);
                    break;
                case Constant.Type.ACHV_BATTLE:
                    showAchievements(content_5, data.Id);
                    break;
                case Constant.Type.ACHV_SPACESHIP:
                    showAchievements(content_6, data.Id);
                    break;
                case Constant.Type.ACHV_LOGIN:
                    showAchievements(content_7, data.Id);
                    break;
                case Constant.Type.ACHV_LEADERSHIP:
                    showAchievements(content_8, data.Id);
                    break;
                case Constant.Type.ACHV_ACHIEVEMENT:
                    showAchievements(content_9, data.Id);
                    break;
                // 不显示没有解锁的隐藏成就
                case Constant.Type.ACHV_HIDDEN when dataPlayer.GetPlayerData().GetPlayerAchievement().ContainsKey(data.Id):
                    showAchievements(content_10, data.Id);
                    break;
                default:
                    break;
            }

        }
        private void showAchievements(Transform container,int id)
        {
            ShowItem<ItemAchievementIcon>(EnumItem.AchievementIcon, (item) =>
            {
                item.transform.SetParent(container, false);
                item.GetComponent<ItemAchievementIcon>().SetAchievementData(id, container);
            });
        }
        private void OnAchievementPoPUp(object sender, GameEventArgs e)
        {
            AchievementPopUpEventArgs ne = (AchievementPopUpEventArgs)e;
            if (ne == null)
                return;
            dataAchievement.cuurrentPopUpId = ne.achievementId;
            dataPlayer.GetPlayerData().UpdatePlayerAchievementData(ne.achievementId, GetNextLevel(ne.achievementId, ne.count));
        }
        private int GetNextLevel(int Id,int count)
        {
            AchievementData achievementData = dataAchievement.GetDataById(Id);
            if(dataAchievement==null)
            {
                return 0;
            }
            int[] Count = achievementData.Count;
            for (int i=0;i< Count.Length;i++)
            {
               if(count< Count[i])
                {
                    return i;
                }
            }

            return Count.Length-1;
        }

    }
}