using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIBasicBattleLost : UGuiFormEx
    {
        public Button retryButton;
        public Button leaveButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            retryButton.onClick.AddListener(OnRetryButtonClick);
            leaveButton.onClick.AddListener(OnLeaveButtonClick);
        }

        private void OnLeaveButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));
        }

        private void OnRetryButtonClick()
        {
            
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}
