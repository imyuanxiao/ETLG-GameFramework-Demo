using ETLG.Data;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UINPCRewardForm : UGuiFormEx
    {
        public Button CloseButton;
        public Button OKButton;
        public Button BattleButton;
        public TextMeshProUGUI Score;
        public TextMeshProUGUI TopContent;
        public TextMeshProUGUI AwardHint;
        public Canvas AwardList;
        public Image DownArrow;
        public TextMeshProUGUI BattleHint;
        public Canvas AttributeList;

        private DataQuiz dataQuizReport;
        private DataNPC dataNPC;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CloseButton.onClick.AddListener(OnCloseButtonClick);
            OKButton.onClick.AddListener(OnOKButtonClick);
            BattleButton.onClick.AddListener(OnBattleButtonClick);
            dataNPC = GameEntry.Data.GetData<DataNPC>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dataQuizReport = GameEntry.Data.GetData<DataQuiz>();
            Score.text = dataQuizReport.getAccuracyText();
            setAward();
            DisplayBoostInfo();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            DisableAllBoostInfo();
        }

        private void setAward()
        {
            if (dataQuizReport.pass)
            {
                Score.color = UIHexColor.HexToColor("1BA784");
                AwardHint.text = "Here are your award!\n" + "Take them now!";
                AwardList.gameObject.SetActive(true);
                ShowRewards();
                DownArrow.gameObject.SetActive(false);
                OKButton.GetComponentInChildren<Text>().text = "GET AWARDS";
            }
            else
            {
                Score.color = UIHexColor.HexToColor("FF6B6B");
                AwardHint.text = "You have NOT passed the test.\n" + "Please TRY AGAIN!";
                AwardList.gameObject.SetActive(false);
                DownArrow.gameObject.SetActive(true);
                OKButton.GetComponentInChildren<Text>().text = "TRY AGAIN";
            }
        }

        private void ShowRewards()
        {
            Transform AwardListTransfrom = AwardList.GetComponentInChildren<Transform>();
            NPCData npcData = dataNPC.GetCurrentNPCData();

            if (npcData.RewardArtifacts.Length > 1)
            {
                int[] rewardArtifacts = npcData.RewardArtifacts;
                for (int i = 0; i < rewardArtifacts.Length; i += 2)
                {
                    int id = rewardArtifacts[i];
                    int num = rewardArtifacts[i + 1];
                    ShowItem<ItemRewardPreview>(EnumItem.ItemRewardPreview, (item) =>
                    {
                        item.transform.SetParent(AwardListTransfrom, false);
                        item.transform.localScale = Vector3.one;
                        item.transform.eulerAngles = Vector3.zero;
                        item.transform.localPosition = Vector3.zero;
                        item.GetComponent<ItemRewardPreview>().SetRewardData(id, num, Constant.Type.REWARD_TYPE_ARTIFACT);
                    });
                }
            }

            if (npcData.RewardSkill != 0)
            {
                int id = npcData.RewardSkill;
                ShowItem<ItemRewardPreview>(EnumItem.ItemRewardPreview, (item) =>
                {
                    item.transform.SetParent(AwardListTransfrom, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero;
                    item.GetComponent<ItemRewardPreview>().SetRewardData(id, 1, Constant.Type.REWARD_TYPE_SKILL);
                });
            }
        }

            private void OnCloseButtonClick()
        {
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizRewardForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizRewardForm));
            }
        }

        private void OnOKButtonClick()
        {
            if (OKButton.GetComponentInChildren<Text>().text == "GET AWARDS")
            {
                if (!dataQuizReport.award)
                {
                    AwardHint.text = "Collect awards succesfully!";
                    dataQuizReport.clickGetButton = true;
                    dataQuizReport.award = true;
                }
                else
                {
                    AwardHint.text = "You have collected my awards before!\n" + "You can NOT collect one more.";
                }
                OKButton.GetComponentInChildren<Text>().text = "TRY AGAIN";
            }
            //try again
            else
            {
                dataQuizReport.again = true;
                if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizRewardForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizRewardForm));
                }
            }
        }

        private void OnBattleButtonClick()
        {
            int accuracy = int.Parse(dataQuizReport.accuracyText);
            Debug.Log(accuracy);
            
            string planetType = "";
            switch (dataQuizReport.domain)
            {
                case Constant.Type.DOMAIN_CLOUD_COMPUTING:
                    planetType = "CloudComputing";
                    break;
                case Constant.Type.DOMAIN_ARTIFICIAL_INTELLIGENCE:
                    planetType = "AI";
                    break;
                case Constant.Type.DOMAIN_CYBERSECURITY:
                    planetType = "CyberSecurity";
                    break;
                case Constant.Type.DOMAIN_DATA_SCIENCE:
                    planetType = "DataScience";
                    break;
                case Constant.Type.DOMAIN_BLOCKCHAIN:
                    planetType = "Blockchain";
                    break;
                case Constant.Type.DOMAIN_IoT:
                    planetType = "IoT";
                    break;
            }
            Debug.Log("Is Boss Fight ? " + dataQuizReport.boss);
            if (dataQuizReport.boss)
            {
                GameEntry.Event.Fire(this, EnterBattleEventArgs.Create("IntermidateBattle", planetType, accuracy));
            }
            else
            {
                GameEntry.Event.Fire(this, EnterBattleEventArgs.Create("BasicBattle", planetType, accuracy));
            }
        }

        private void DisplayBoostInfo()
        {
            int accuracy = int.Parse(dataQuizReport.accuracyText);
            UIAttributeList attrList = this.AttributeList.GetComponent<UIAttributeList>();

            switch (dataQuizReport.domain)
            {
                case Constant.Type.DOMAIN_CLOUD_COMPUTING:
                    attrList.DisplayCloudComputingBoost(accuracy);
                    break;
                case Constant.Type.DOMAIN_ARTIFICIAL_INTELLIGENCE:
                    attrList.DisplayAIBoost(accuracy);
                    break;
                case Constant.Type.DOMAIN_CYBERSECURITY:
                    attrList.DisplayCybersecurityBoost(accuracy);
                    break;
                case Constant.Type.DOMAIN_DATA_SCIENCE:
                    attrList.DisplayDataScienceBoost(accuracy);
                    break;
                case Constant.Type.DOMAIN_BLOCKCHAIN:
                    attrList.DisplayBlockchainBoost(accuracy);
                    break;
                case Constant.Type.DOMAIN_IoT:
                    attrList.DisplayIoTBoost(accuracy);
                    break;
            }
        }

        private void DisableAllBoostInfo()
        {
            UIAttributeList attrList = this.AttributeList.GetComponent<UIAttributeList>();
            attrList.firePower.SetActive(false);
            attrList.energy.SetActive(false);
            attrList.shield.SetActive(false);
            attrList.durability.SetActive(false);
            attrList.agility.SetActive(false);
            attrList.fireRate.SetActive(false);
        }
    }
}