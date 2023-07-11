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
       // PlayerAchievementData[] playerAchievementDatas;
       // private Dictionary<int, List<PlayerAchievementData>> playerAchievementData;
        private DataPlayer dataPlayer;
        DataAchievement dataAchievement;
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
            //获取成就数据
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("Open Achievement");
            showContent();
            // open navigationform UI
            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);
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

            s_unlockedNumber.text = dataPlayer.GetPlayerData().getUnlockedAchievementCount().ToString();
            s_total.text=dataAchievement.GetAchievementCount().ToString();
            AchievementData[] achievementDatas = dataAchievement.GetAllNewData();
            foreach (AchievementData data in achievementDatas)
            {
                if (data.TypeId == Constant.Type.ACHV_QUIZ)
                {
                    showAchievements(content_1, data);
                }
                else if (data.TypeId == Constant.Type.ACHV_RESOURCE)
                {
                    showAchievements(content_2, data);
                }
                else if (data.TypeId == Constant.Type.ACHV_KNOWLEDGE_BASE)
                {
                    showAchievements(content_3, data);
                }
                else if (data.TypeId == Constant.Type.ACHV_INTERSTELLAR)
                {
                    showAchievements(content_4, data);
                }
                else if (data.TypeId == Constant.Type.ACHV_BATTLE)
                {
                    showAchievements(content_5, data);
                }
                else if (data.TypeId == Constant.Type.ACHV_SPACESHIP)
                {
                    showAchievements(content_6, data);
                }
                else if (data.TypeId == Constant.Type.ACHV_LOGIN)
                {
                    showAchievements(content_7, data);
                }
                else if (data.TypeId == Constant.Type.ACHV_LEADERSHIP)
                {
                    showAchievements(content_8, data);
                }
                else if (data.TypeId == Constant.Type.ACHV_ACHIEVEMENT)
                {
                    showAchievements(content_9, data);
                }
                else if (data.TypeId == Constant.Type.ACHV_HIDDEN)
                {
                    showAchievements(content_10, data);
                }
            }
        }
        private void showAchievements(Transform container, AchievementData achievementData)
        {
            //不显示没有解锁的隐藏成就
            if (achievementData.TypeId != Constant.Type.ACHV_HIDDEN && !dataPlayer.GetPlayerData().GetPlayerAchievement().ContainsKey(achievementData.Id))
            {
                ShowItem<ItemAchievementIcon>(EnumItem.AchievementIcon, (item) =>
                {
                    item.transform.SetParent(container, false);
                    item.GetComponent<ItemAchievementIcon>().SetAchievementData(achievementData, container);
                });
            }
        }
        public void updateAchievement()
        {
           
        }
       
    }







}