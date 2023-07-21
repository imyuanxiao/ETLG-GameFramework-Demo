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
            int conditionId = achievementData.ConditionId;
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
                    this.progress.text = "0";
                    break;
                case Constant.Type.ACHV_RESOURCE:
                    //money
                    if(conditionId==5001)
                    {
                        this.progress.text= dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.Money).ToString();
                    }
                    //spend money
                   //Knowledge Fragments 
                    else if (conditionId == 5004)
                    {
                        int count = 0;
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
                            count += dataPlayer.GetPlayerData().GetArtifactNumById((int)artifactType);
                        }

                        this.progress.text = count.ToString();
                    }
                    //Knowledge point
                    else if(conditionId==5005)
                    {
                        this.progress.text = dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.KnowledgePoint).ToString();
                    }
                    //mineral
                    else if (conditionId == 5006)
                    {
                        int count = dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.HighPurityMetal) + dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.RareOre);
                        this.progress.text = count.ToString();
                    }
                    //Fule
                    else if (conditionId == 5007)
                    {
                        int count = dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.LiquidMethane) + dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.PlasmaFuel);
                        this.progress.text = count.ToString();
                    }
                    else
                    {
                        this.progress.text = "0";
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
    }
}
