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
    public class UIAlertForm : UGuiFormEx
    {
        public Button CloseButton;
        public Button OKButton;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            CloseButton.onClick.AddListener(OnCloseButtonClick);
            OKButton.onClick.AddListener(OnOKButtonClick);
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