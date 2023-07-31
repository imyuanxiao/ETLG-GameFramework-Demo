using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;


namespace ETLG
{
    public class UINpcAlertForm : UGuiFormEx
    {
        public TextMeshProUGUI AlertContent;
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
            OKButton.onClick.AddListener(OnCloseButtonClick);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void OnCloseButtonClick()
        {
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCAlertForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCAlertForm));
            }
        }



    }
}