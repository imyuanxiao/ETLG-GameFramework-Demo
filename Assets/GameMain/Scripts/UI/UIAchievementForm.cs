using ETLG.Data;
using GameFramework.Event;
using System.Collections;
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
        public TextMeshProUGUI s_name_3 = null;
        public TextMeshProUGUI s_name_4 = null;
        public TextMeshProUGUI s_name_5 = null;
        public TextMeshProUGUI s_name_6 = null;
        public TextMeshProUGUI s_name_7 = null;
        public TextMeshProUGUI s_name_8 = null;
        public TextMeshProUGUI s_name_9 = null;
        public TextMeshProUGUI s_name_10 = null;

        public TextMeshProUGUI s_unlockedNumber = null;

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
   
        private Dictionary<int, List<PlayerAchievementData>> playerAchievementData;
        private DataPlayer dataPlayer;
        DataAchievement dataAchievement = GameEntry.Data.GetData<DataAchievement>();
        // 实体加载器
        private EntityLoader entityLoader;

        public bool refresh;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            // 获取玩家数据管理器
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            entityLoader = EntityLoader.Create(this);
            //获取成就数据
            playerAchievementData = dataPlayer.GetPlayerData().getPlayerAchievements();
            showContent();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            showContent();
            // open navigationform UI
            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
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

            int unlockedCount = dataPlayer.GetPlayerData().getUnlockedAchievementCount();
            s_unlockedNumber.text = unlockedCount.ToString() + " / " + dataAchievement.GetAchievementCount();
            foreach (KeyValuePair<int, List<PlayerAchievementData>> pair in playerAchievementData)
            {
                if (pair.Key == Constant.Type.ACHV_QUIZ)
                {
                    showAchievements(content_1, pair.Value);
                }
                else if (pair.Key == Constant.Type.ACHV_RESOURCE)
                {
                    showAchievements(content_2, pair.Value);
                }
                else if (pair.Key == Constant.Type.ACHV_KNOWLEDGE_BASE)
                {
                    showAchievements(content_3, pair.Value);
                }
                else if (pair.Key == Constant.Type.ACHV_INTERSTELLAR)
                {
                    showAchievements(content_4, pair.Value);
                }
                else if (pair.Key == Constant.Type.ACHV_BATTLE)
                {
                    showAchievements(content_5, pair.Value);
                }
                else if (pair.Key == Constant.Type.ACHV_SPACESHIP)
                {
                    showAchievements(content_6, pair.Value);
                }
                else if (pair.Key == Constant.Type.ACHV_LOGIN)
                {
                    showAchievements(content_7, pair.Value);
                }
                else if (pair.Key == Constant.Type.ACHV_LEADERSHIP)
                {
                    showAchievements(content_8, pair.Value);
                }
                else if (pair.Key == Constant.Type.ACHV_ACHIEVEMENT)
                {
                    showAchievements(content_9, pair.Value);
                }
                else if (pair.Key == Constant.Type.ACHV_HIDDEN)
                {
                    showAchievements(content_10, pair.Value);
                }
            }
        }
        private void showAchievements(Transform container, List<PlayerAchievementData> playerAchievementData)
        {
            for (int i = 0; i < playerAchievementData.Count; i++)
            {
                PlayerAchievementData playerAchievement = playerAchievementData[i];
                //不显示没有解锁的隐藏成就
                if(playerAchievementData[i].TypeId==Constant.Type.ACHV_HIDDEN && !playerAchievementData[i].IsUnlocked)
                {
                    continue;
                }
                ShowItem<ItemAchievementIcon>(EnumItem.AchievementIcon, (item) =>
                {
                    item.transform.SetParent(container, false);
                    item.GetComponent<ItemAchievementIcon>().SetAchievementData(playerAchievement, container);
                });
            }
        }
    }







}