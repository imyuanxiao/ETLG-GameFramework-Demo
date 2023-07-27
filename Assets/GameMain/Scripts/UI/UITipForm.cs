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
        private DataAchievement dataAchievement;
        public Transform UIContainer;

        public TextMeshProUGUI TipTitle = null;
        public TextMeshProUGUI TipContent = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            UIContainer.position = dataPlayer.tipUiPosition;
            TipTitle.text = dataPlayer.tipTitle;
            if(dataAchievement.descriptionLevel==0)
            {
                //TipContent.text = GameEntry.Localization.GetString(Constant.Key.PRE_TIP + dataPlayer.tipTitle);
            /*    if(GameEntry.Data.GetData<DataPlayer>().tipContent != )
                {*/
                    TipContent.text = GameEntry.Data.GetData<DataPlayer>().tipContent;
                //}
        /*        else
                {
                    TipContent.text = GameEntry.Localization.GetString(Constant.Key.PRE_TIP + dataPlayer.tipTitle);
                }*/
            }
            else
            {
                TipContent.text = GameEntry.Localization.GetString(Constant.Key. PRE_ACHIEVE+ dataAchievement.descriptionId.ToString()+ Constant.Key.POST_ACHIEVE_LEVEL+ dataAchievement.descriptionLevel.ToString());
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            dataAchievement.descriptionLevel = 0;
        }

    }
}


