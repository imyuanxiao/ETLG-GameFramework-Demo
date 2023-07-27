using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using ETLG.Data;

namespace ETLG
{
    public class UIBattleWin : UGuiFormEx
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI info1 = null;
        // [SerializeField] private TextMeshProUGUI info2 = null;
        [SerializeField] private TextMeshProUGUI[] rewardsInfo;  // 0: coin, 1: skill, 2: fraction

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

            if (GameEntry.Procedure.CurrentProcedure is ProcedureBasicBattle)
            {
                Dictionary<string, int> data = (Dictionary<string, int>) userData;
                info1.text = "Enemy Killed: " + data["Killed"] + " | Enemy Passed: " + data["Passed"];
                // info2.text = "You got " + data["Killed"] * 100 + " coins!";
                rewardsInfo[0].text = "You got " + data["Killed"] * 100 + " coins!";
                rewardsInfo[0].gameObject.transform.parent.gameObject.SetActive(true);
            }
            else if (GameEntry.Procedure.CurrentProcedure is ProcedureIntermidateBattle)
            {
                info1.text = "You defeated the boss in " + GetDomain() + " domain";
                // info2.text = "You got a knowledge fraction!";
                rewardsInfo[1].text = "Unlock Skill " + GetUnlockedSkill();
                rewardsInfo[2].text = "You got a knowledge fraction in " + GetDomain() + " domain";
                rewardsInfo[1].gameObject.transform.parent.gameObject.SetActive(true);
                rewardsInfo[2].gameObject.transform.parent.gameObject.SetActive(true);
            }
            else if (GameEntry.Procedure.CurrentProcedure is ProcedureFinalBattle)
            {
                info1.text = "You defeated final Boss";
                rewardsInfo[2].text = "You got the final fraction!";
                rewardsInfo[2].gameObject.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                info1.text = "Reward";
                // info2.text = "Reward";
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            DisableRewardContainer();
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
                    return GameEntry.Localization.GetString("Skill_EdgeComputing"); //+ " : " + GameEntry.Localization.GetString("Skill_EdgeComputing_Level_1");
                case "CyberSecurity":
                    return GameEntry.Localization.GetString("Skill_ElectronicWarfare"); // + " : " + GameEntry.Localization.GetString("Skill_ElectronicWarfare_Level_1");
                case "AI":
                    return GameEntry.Localization.GetString("Skill_AdaptiveIntelligentDefense"); // + " : " + GameEntry.Localization.GetString("Skill_AdaptiveIntelligentDefense_Level_1");
                case "DataScience":
                    return GameEntry.Localization.GetString("Skill_EnergyBoost"); // + " : " + GameEntry.Localization.GetString("Skill_EnergyBoost_Level_1");
                case "Blockchain":
                    return GameEntry.Localization.GetString("Skill_BlockchainResurgence"); // + " : " + GameEntry.Localization.GetString("Skill_BlockchainResurgence_Level_1");
                case "IoT":
                    return GameEntry.Localization.GetString("Skill_MedicalSupport"); // + " : " + GameEntry.Localization.GetString("Skill_MedicalSupport_Level_1");
                default:
                    return "";
            }
        }
    }
}
