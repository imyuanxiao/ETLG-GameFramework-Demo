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
        public Button CancelButton;
        public TextMeshProUGUI Score;
        public TextMeshProUGUI TopContent;
        public TextMeshProUGUI ButtomContent;
        public Canvas AwardList;

        private DataQuiz dataQuizReport;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dataQuizReport = GameEntry.Data.GetData<DataQuiz>();
            CloseButton.onClick.AddListener(OnCloseButtonClick);
            OKButton.onClick.AddListener(OnOKButtonClick);
            Score.text = dataQuizReport.getAccuracyText();
            //看有没有获得奖励，然后展示不同的

        }

        private void OnCloseButtonClick()
        {
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCRewardForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCRewardForm));
            }
            GameEntry.UI.OpenUIForm(EnumUIForm.UINPCRewardForm);
        }

        private void OnOKButtonClick()
        {
            //try again功能
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCRewardForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCRewardForm));
            }
            GameEntry.UI.OpenUIForm(EnumUIForm.UINPCRewardForm);
        }
    }
}