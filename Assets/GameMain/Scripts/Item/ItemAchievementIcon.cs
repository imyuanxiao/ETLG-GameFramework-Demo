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
        private Dictionary<int, int> playerTotalArtifact;
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
            int conditionId = achievementData.ConditionId;
            playerTotalArtifact = dataPlayer.GetPlayerData().playerTotalArtifacts;
            if (dataPlayer.GetPlayerData().GetPlayerAchievement().ContainsKey(id) && dataAchievement.isMaxLevel(id, dataPlayer.GetPlayerData().GetPlayerAchievement()[id]))
            {
                Sprite sprite = Resources.Load<Sprite>(AssetUtility.GetUnLockAchievementIcon());
                this.image.sprite = sprite;
            }
            this.acheivementName.text = GameEntry.Localization.GetString(Constant.Key.PRE_ACHIEVE + id.ToString() + Constant.Key.POST_TITLE);
            switch (type)
            {
                case Constant.Type.ACHV_QUIZ:
                    //player data
                    this.progress.text = "0";
                    break;
                case Constant.Type.ACHV_RESOURCE:
                    //money
                    switch (conditionId)
                    {
                        case 5001:
                            this.progress.text = GetAchievementProgress((int)EnumArtifact.Money).ToString();
                            break;
                        //spend money
                        case 5002:
                            this.progress.text = GetAchievementProgress(Constant.Type.ACHIV_TOTAL_SPEND_MONEY).ToString();
                            break;
                        //Knowledge Fragments 
                        case 5004:
                            int fragmentCount = 0;
                            EnumArtifact[] artifactTypes = new EnumArtifact[]
                            {
                            EnumArtifact.KnowledgeFragments_AI,
                            EnumArtifact.KnowledgeFragments_Blockchain,
                            EnumArtifact.KnowledgeFragments_CloudComputing,
                            EnumArtifact.KnowledgeFragments_Cybersecurity,
                            EnumArtifact.KnowledgeFragments_DataScience,
                            EnumArtifact.KnowledgeFragments_IoT
                            };
                            foreach (EnumArtifact artifactType in artifactTypes)
                            {
                                fragmentCount += GetAchievementProgress((int)artifactType);
                            }

                            this.progress.text = fragmentCount.ToString();
                            break;
                        //Knowledge point
                        case 5005:
                            this.progress.text = GetAchievementProgress((int)EnumArtifact.KnowledgePoint).ToString();
                            break;
                        //mineral
                        case 5006:
                            int mineralCount = GetAchievementProgress((int)EnumArtifact.RareOre);
                            this.progress.text = mineralCount.ToString();
                            break;
                        //Fule
                        case 5007:
                            int fuleCount = GetAchievementProgress((int)EnumArtifact.FuelRefillUnit);
                            this.progress.text = fuleCount.ToString();
                            break;
                        default:
                            this.progress.text = "0";
                            break;
                    }
                    break;
                case Constant.Type.ACHV_INTERSTELLAR:
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
                case Constant.Type.ACHV_BATTLE:
                    //player data
                    this.progress.text = "0";
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
            }

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
        public int GetAchievementProgress(int id)
        {
            if (!playerTotalArtifact.ContainsKey(id))
            {
                return 0;
            }
            return playerTotalArtifact[id];
        }
    }
}
