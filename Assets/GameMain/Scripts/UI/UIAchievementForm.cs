using ETLG.Data;
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

        // buttons
        public Button returnButton;

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
        public TextMeshProUGUI s_unlockedNumber = null;
        private Dictionary<int, List<PlayerAchievementData>> playerAchievementData;
        private DataPlayer dataPlayer;

        // 实体加载器
        private EntityLoader entityLoader;


        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 绑定按钮点击事件
            returnButton.onClick.AddListener(OnReturnButtonClick);


            // 获取玩家数据管理器
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            entityLoader = EntityLoader.Create(this);
            //获取成就数据
            playerAchievementData = dataPlayer.GetPlayerData().getPlayerAchievements();
            //加载成就名称，先加载
            foreach (KeyValuePair<int, List<PlayerAchievementData>> pair in playerAchievementData)
            {
                if (pair.Key == Constant.Type.ACHV_QUIZ)
                {
                    s_name_1.text = "Quiz";
                    showAchievements(content_1, pair.Value);
                }
                else if (pair.Key == Constant.Type.ACHV_RESOURCE)
                {
                    s_name_2.text = "Resource";
                    showAchievements(content_2, pair.Value);
                }
                //...
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnReturnButtonClick()
        {
            Log.Debug("Return to Player Menu");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
          //  GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.PlayerMenu")));

        }

        private void showAchievements(Transform container, List<PlayerAchievementData> playerAchievementData)
        {
            for (int i = 0; i < playerAchievementData.Count; i++)
            {
                // 计算当前元素所在的行数,一行摆两个
                int row = i / 2; 

                Vector3 offset = new Vector3((i % 2) * 325f, row * -150f, 0f) + new Vector3(175f, -80f, 0f);

                PlayerAchievementData playerAchievement = playerAchievementData[i];

                ShowItem<ItemAchievementIcon>(EnumItem.AchievementIcon, (item) =>
                {
                    item.transform.SetParent(container, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + offset;
                    item.GetComponent<ItemAchievementIcon>().SetAchievementData(playerAchievement, container);
                });
            }


        }
    }







}