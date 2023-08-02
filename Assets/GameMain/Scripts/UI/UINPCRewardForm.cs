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
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CloseButton.onClick.AddListener(OnCloseButtonClick);
            OKButton.onClick.AddListener(OnOKButtonClick);
            BattleButton.onClick.AddListener(OnBattleButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dataQuizReport = GameEntry.Data.GetData<DataQuiz>();
            Score.text = dataQuizReport.getAccuracyText();
            //看有没有获得奖励，然后展示不同的
            setAward();

        }

        private void setAward()
        {
            if (dataQuizReport.pass)
            {
                Score.color = UIHexColor.HexToColor("1BA784");
                AwardHint.text = "Here are your award!\n" + "Take them now!";
                AwardList.gameObject.SetActive(true);
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

        private void OnCloseButtonClick()
        {
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCRewardForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCRewardForm));
            }
        }

        private void OnOKButtonClick()
        {
            Debug.Log(OKButton.GetComponentInChildren<Text>().text);
            if (OKButton.GetComponentInChildren<Text>().text == "GET AWARDS")
            {
                //显示奖励东西
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
            else
            {
                dataQuizReport.again = true;
                if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCRewardForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCRewardForm));
                }
            }
        }

        private void OnBattleButtonClick()
        {
            //准确率，0-100的数值
            int accuracy = int.Parse(dataQuizReport.accuracyText);
            Debug.Log(accuracy);
            //领域,获取npc是什么领域的方法，还没有想好怎么写，现在是直接在DataQuiz.cs的脚本中，把dataQuizReport.domain写死了，如果你要换着测试，直接去DataQuiz的domain改值
            switch (dataQuizReport.domain)
            {
                case Constant.Type.DOMAIN_CLOUD_COMPUTING:
                    break;
                case Constant.Type.DOMAIN_ARTIFICIAL_INTELLIGENCE:
                    break;
                case Constant.Type.DOMAIN_CYBERSECURITY:
                    break;
                case Constant.Type.DOMAIN_DATA_SCIENCE:
                    break;
                case Constant.Type.DOMAIN_BLOCKCHAIN:
                    break;
                case Constant.Type.DOMAIN_IoT:
                    break;
            }
            //是否boss战
            Debug.Log(dataQuizReport.boss);
        }
    }
}