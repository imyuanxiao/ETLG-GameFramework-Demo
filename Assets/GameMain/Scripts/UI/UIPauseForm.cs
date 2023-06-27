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

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            returnToMapButton.onClick.AddListener(OnReturnToMapClick);
        }

        private void OnReturnToMapClick()
        {
            
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
