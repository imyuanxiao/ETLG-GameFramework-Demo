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
        }

        private void OnCloseButtonClick()
        {
            GameEntry.Event.Fire(this, UIAlertTriggerEventArgs.Create(Constant.Type.UI_CLOSE));
        }

        private void OnOKButtonClick()
        {
            GameEntry.Event.Fire(this, UIAlertTriggerEventArgs.Create(Constant.Type.UI_CLOSE));
        }
    }
}