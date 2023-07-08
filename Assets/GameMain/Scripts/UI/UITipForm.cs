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
    public class UITipForm : UGuiFormEx
    {
        private DataPlayer dataPlayer;

        public Transform UIContainer;

        public TextMeshProUGUI TipTitle = null;
        public TextMeshProUGUI TipContent = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
         

            UIContainer.position = dataPlayer.tipUiPosition;
            TipTitle.text = dataPlayer.tipTitle;
            TipContent.text = GameEntry.Localization.GetString(Constant.Key.PRE_TIP + dataPlayer.tipTitle); 

        }

        protected override void OnClose(bool isShutdown, object userData)
        {

            base.OnClose(isShutdown, userData);

        }
 

    }
}


