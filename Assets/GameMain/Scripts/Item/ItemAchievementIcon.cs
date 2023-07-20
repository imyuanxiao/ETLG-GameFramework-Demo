using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemAchievementIcon : ItemLogicEx
    {
        private DataPlayer dataPlayer;
        private DataAchievement dataAchievement;
        private AchievementData achievementData;
        public Image image;
        public Button achievementButton;
        public TextMeshProUGUI acheivementName = null;
        public TextMeshProUGUI progress = null;
        public TextMeshProUGUI next_level = null;
        public Transform container;
        public string tipTitle;
        public int position = 0;
        public bool refresh;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();

        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (refresh)
            {
                SetData();
                refresh = false;
            }
        }
        public void UpdateData()
        {
            refresh = true;
        }
        public void SetAchievementData(int id, Transform container)
        {
            this.container = container;
            this.achievementData = dataAchievement.GetDataById(id);
            refresh = false;
            SetData();
        }
        private void SetData()
        {
            int type = achievementData.TypeId;
            int id = achievementData.Id;
            if (dataPlayer.GetPlayerData().GetPlayerAchievement().ContainsKey(id) && dataAchievement.isMaxLevel(id, dataPlayer.GetPlayerData().GetPlayerAchievement()[id]))
            {
                Sprite sprite = Resources.Load<Sprite>(AssetUtility.GetUnLockAchievementIcon());
                this.image.sprite = sprite;
            }
            this.acheivementName.text = achievementData.Name;
            switch (type)
            {
                case Constant.Type.ACHV_QUIZ:
                    //player data
                    break;
                case Constant.Type.ACHV_RESOURCE:
                    //player data
                    break;
                case Constant.Type.ACHV_KNOWLEDGE_BASE:
                    //level
                    if (dataPlayer.GetPlayerData().GetPlayerAchievement().ContainsKey(id))
                    {
                        this.progress.text = dataPlayer.GetPlayerData().GetPlayerAchievement()[id].ToString();
                    }
                    else
                    {
                        this.progress.text = "0";
                    }
                    break;
                case Constant.Type.ACHV_INTERSTELLAR:
                    //conditionId=3001,player data
                    //others, level
                    break;
                case Constant.Type.ACHV_BATTLE:
                    //player data
                    break;
                case Constant.Type.ACHV_SPACESHIP:
                    //level
                    if (dataPlayer.GetPlayerData().GetPlayerAchievement().ContainsKey(id))
                    {
                        this.progress.text = dataPlayer.GetPlayerData().GetPlayerAchievement()[id].ToString();
                    }
                    else
                    {
                        this.progress.text = "0";
                    }
                    break;
                case Constant.Type.ACHV_LOGIN:
                    //player data
                    break;
                case Constant.Type.ACHV_LEADERSHIP:
                    //player data
                    break;
                case Constant.Type.ACHV_ACHIEVEMENT:
                    //player data
                    this.progress.text = dataPlayer.GetPlayerData().GetPlayerAchievementPoints().ToString();
                    break;
                case Constant.Type.ACHV_HIDDEN:
                    break;
            }

            //先暂时都设为0
            this.progress.text = "0";
            this.next_level.text = GetNextLevel();
        }
        private string GetNextLevel()
        {
            PlayerData playerData = dataPlayer.GetPlayerData();
            Dictionary<int, int> playerAchievement = playerData.GetPlayerAchievement();

            if (!playerAchievement.ContainsKey(achievementData.Id))
            {
                return achievementData.Count[0].ToString();
            }

            int level = playerAchievement[achievementData.Id];
            int countIndex = (level < achievementData.Count.Length) ? level : level - 1;

            return achievementData.Count[countIndex].ToString();
        }
        public int GetCurrentAchievementID()
        {
            return this.achievementData.Id;
        }
    }
}
