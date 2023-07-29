using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using ETLG.Data;
using System.Xml.Linq;

namespace ETLG
{
    public class UIBattleWin : UGuiFormEx
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI info1 = null;
        // [SerializeField] private TextMeshProUGUI info2 = null;
        [SerializeField] private TextMeshProUGUI[] rewardsInfo;  // 0: coin, 1: skill, 2: fraction
        public Transform rewardContainer;
        private List<int> rewardsId;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            continueButton.onClick.AddListener(OnContinueBtnClick);
        }

        private void OnContinueBtnClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            this.rewardsId = new List<int>();

            if (GameEntry.Procedure.CurrentProcedure is ProcedureBasicBattle)
            {
                Dictionary<string, int> data = (Dictionary<string, int>) userData;
                info1.text = "Enemy Killed: " + data["Killed"] + " | Enemy Passed: " + data["Passed"];
                // info2.text = "You got " + data["Killed"] * 100 + " coins!";
                this.rewardsId.Add((int) EnumArtifact.Money);
                this.rewardsId.Add(data["Killed"] * 100);
                // rewardsInfo[0].text = "You got " + data["Killed"] * 100 + " coins!";
                // rewardsInfo[0].gameObject.transform.parent.gameObject.SetActive(true);
                // rewardsInfo[0].transform.parent.gameObject.GetComponent<UITipTrigger>().tipTitle = GameEntry.Localization.GetString("Artifact_Money");
                // rewardsInfo[0].transform.parent.gameObject.GetComponent<UITipTrigger>().tipContent = GameEntry.Localization.GetString("Tip_Money");
            }
            else if (GameEntry.Procedure.CurrentProcedure is ProcedureIntermidateBattle)
            {
                info1.text = "You defeated the boss in " + GetDomain() + " domain";
                GetUnlockedSkill();
                GetKnowledgeFraction();
                // info2.text = "You got a knowledge fraction!";
                // rewardsInfo[1].text = "Unlock Skill " + GetUnlockedSkill();
                // rewardsInfo[2].text = "You got a knowledge fraction in " + GetDomain() + " domain";
                // rewardsInfo[1].gameObject.transform.parent.gameObject.SetActive(true);
                // rewardsInfo[2].gameObject.transform.parent.gameObject.SetActive(true);

                // rewardsInfo[1].transform.parent.gameObject.GetComponent<UITipTrigger>().tipTitle = GetUnlockedSkill();
                // rewardsInfo[1].transform.parent.gameObject.GetComponent<UITipTrigger>().tipContent = GetUnlockedSkillDesc();
                // rewardsInfo[2].transform.parent.gameObject.GetComponent<UITipTrigger>().tipTitle = GetKnowledgeFraction()[0];
                // rewardsInfo[2].transform.parent.gameObject.GetComponent<UITipTrigger>().tipContent = GetKnowledgeFraction()[1];
            }
            else if (GameEntry.Procedure.CurrentProcedure is ProcedureFinalBattle)
            {
                info1.text = "You defeated final Boss";
                // rewardsInfo[2].text = "You got the final fraction!";
                // rewardsInfo[2].gameObject.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                info1.text = "Reward";
                // info2.text = "Reward";
            }

            DisplayRewardInfo();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            DisableRewardContainer();
            this.rewardsId.Clear();
        }

        private void DisableRewardContainer()
        {
            foreach (var item in rewardsInfo)
            {
                item.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }

        private string GetDomain()
        {
            switch (BattleManager.Instance.bossType)
            {
                case "CloudComputing":
                    return "Cloud Computing";
                case "CyberSecurity":
                    return "Cybersecurity";
                case "AI":
                    return "Artificial Intelligence";
                case "DataScience":
                    return "Data Science";
                case "Blockchain":
                    return "Blockchain";
                case "IoT":
                    return "Internet of Things";
                default:
                    return "";
            }
        }

        private string GetUnlockedSkill()
        {
            switch (BattleManager.Instance.bossType)
            {
                case "CloudComputing":
                    this.rewardsId.Add((int) EnumSkill.EdgeComputing);
                    this.rewardsId.Add(1);
                    return GameEntry.Localization.GetString("Skill_EdgeComputing"); //+ " : " + GameEntry.Localization.GetString("Skill_EdgeComputing_Level_1");
                case "CyberSecurity":
                    this.rewardsId.Add((int) EnumSkill.ElectronicWarfare);
                    this.rewardsId.Add(1);
                    return GameEntry.Localization.GetString("Skill_ElectronicWarfare"); // + " : " + GameEntry.Localization.GetString("Skill_ElectronicWarfare_Level_1");
                case "AI":
                    this.rewardsId.Add((int) EnumSkill.AdaptiveIntelligentDefense);
                    this.rewardsId.Add(1);
                    return GameEntry.Localization.GetString("Skill_AdaptiveIntelligentDefense"); // + " : " + GameEntry.Localization.GetString("Skill_AdaptiveIntelligentDefense_Level_1");
                case "DataScience":
                    this.rewardsId.Add((int) EnumSkill.EnergyBoost);
                    this.rewardsId.Add(1);
                    return GameEntry.Localization.GetString("Skill_EnergyBoost"); // + " : " + GameEntry.Localization.GetString("Skill_EnergyBoost_Level_1");
                case "Blockchain":
                    this.rewardsId.Add((int) EnumSkill.BlockchainResurgence);
                    this.rewardsId.Add(1);
                    return GameEntry.Localization.GetString("Skill_BlockchainResurgence"); // + " : " + GameEntry.Localization.GetString("Skill_BlockchainResurgence_Level_1");
                case "IoT":
                    this.rewardsId.Add((int) EnumSkill.MedicalSupport);
                    this.rewardsId.Add(1);
                    return GameEntry.Localization.GetString("Skill_MedicalSupport"); // + " : " + GameEntry.Localization.GetString("Skill_MedicalSupport_Level_1");
                default:
                    return "";
            }
        }

        private string GetUnlockedSkillDesc()
        {
            switch (BattleManager.Instance.bossType)
            {
                case "CloudComputing":
                    return GameEntry.Localization.GetString("Skill_EdgeComputing_Level_1");
                case "CyberSecurity":
                    return GameEntry.Localization.GetString("Skill_ElectronicWarfare_Level_1");
                case "AI":
                    return GameEntry.Localization.GetString("Skill_AdaptiveIntelligentDefense_Level_1");
                case "DataScience":
                    return GameEntry.Localization.GetString("Skill_EnergyBoost_Level_1");
                case "Blockchain":
                    return GameEntry.Localization.GetString("Skill_BlockchainResurgence_Level_1");
                case "IoT":
                    return GameEntry.Localization.GetString("Skill_MedicalSupport_Level_1");
                default:
                    return "";
            }
        }

        private List<string> GetKnowledgeFraction()
        {
            switch (BattleManager.Instance.bossType)
            {
                case "CloudComputing":
                    this.rewardsId.Add((int) EnumArtifact.KnowledgeFragments_CloudComputing);
                    this.rewardsId.Add(1);
                    return GetTipResult("Artifact_KnowledgeFragments_CloudComputing", "Tip_Knowledge Fragments - Cloud Computing");
                case "CyberSecurity":
                    this.rewardsId.Add((int) EnumArtifact.KnowledgeFragments_Cybersecurity);
                    this.rewardsId.Add(1);
                    return GetTipResult("Artifact_KnowledgeFragments_Cybersecurity", "Tip_Knowledge Fragments - Cybersecurity");
                case "AI":
                    this.rewardsId.Add((int) EnumArtifact.KnowledgeFragments_AI);
                    this.rewardsId.Add(1);
                    return GetTipResult("Artifact_KnowledgeFragments_AI", "Tip_Knowledge Fragments - AI");
                case "DataScience":
                    this.rewardsId.Add((int) EnumArtifact.KnowledgeFragments_DataScience);
                    this.rewardsId.Add(1);
                    return GetTipResult("Artifact_KnowledgeFragments_DataScience", "Tip_Knowledge Fragments - Data Science");
                case "Blockchain":
                    this.rewardsId.Add((int) EnumArtifact.KnowledgeFragments_Blockchain);
                    this.rewardsId.Add(1);
                    return GetTipResult("Artifact_KnowledgeFragments_Blockchain", "Tip_Knowledge Fragments - Blockchain");
                case "IoT":
                    this.rewardsId.Add((int) EnumArtifact.KnowledgeFragments_IoT);
                    this.rewardsId.Add(1);
                    return GetTipResult("Artifact_KnowledgeFragments_IoT", "Tip_Knowledge Fragments - IoT");
                default:
                    return null;
            }
        }

        private List<string> GetTipResult(string title, string tip)
        {
            List<string> result = new List<string>();
            string key = GameEntry.Localization.GetString(title);
            string value = GameEntry.Localization.GetString(tip);
            result.Add(key);
            result.Add(value);
            return result;
        }

        private void DisplayRewardInfo()
        {
            // Reward ID: [id, num, id, num, ...]
            // int »ò List¶¼ÐÐ
            int[] rewardArtifactId = new int[1];

            for (int i = 0; i < rewardArtifactId.Length; i += 2) {
                int id = rewardArtifactId[i];
                int num = rewardArtifactId[i + 1];
                ShowItem<ItemRewardPreview>(EnumItem.ItemRewardPreview, (item) =>
                {
                    item.transform.SetParent(rewardContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero;
                    item.GetComponent<ItemRewardPreview>().SetRewardData(id, num, Constant.Type.REWARD_TYPE_ARTIFACT);
                });
            }
            
            // reward skill
            int skillId = 0;

            ShowItem<ItemRewardPreview>(EnumItem.ItemRewardPreview, (item) =>
            {
                item.transform.SetParent(rewardContainer, false);
                item.transform.localScale = Vector3.one;
                item.transform.eulerAngles = Vector3.zero;
                item.transform.localPosition = Vector3.zero;
                item.GetComponent<ItemRewardPreview>().SetRewardData(skillId, 1, Constant.Type.REWARD_TYPE_SKILL);
            });

        }
    }
}
