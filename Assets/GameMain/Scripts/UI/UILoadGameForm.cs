using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameFramework.Localization;

namespace ETLG
{
    public class UILoadGameForm : UGuiFormEx
    {
       
        public Button cancelButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            cancelButton.onClick.AddListener(OnCancelButtonClick);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

       
        }

        private void OnCancelButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            Close();
            GameEntry.UI.OpenUIForm(EnumUIForm.UIMainMenuForm);
        }

    }
}


