using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETLG
{
    public class UIPauseForm : UGuiFormEx
    {
        [SerializeField] private Button returnToMapButton;
        [SerializeField] private Button continueButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            returnToMapButton.onClick.AddListener(OnReturnToMapClick);
            continueButton.onClick.AddListener(OnContinueClicked);
        }

        private void OnContinueClicked()
        {
            this.Close();
        }

        private void OnReturnToMapClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            GameEntry.Event.Fire(this, PlayerDeadEventArgs.Create());
            this.Close();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
