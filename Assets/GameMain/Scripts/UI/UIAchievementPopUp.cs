using ETLG.Data;
using GameFramework.Event;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIAchievementPopUp : UGuiFormEx
    {
        private DataAchievement dataAchievement;
        public TextMeshProUGUI acheivementName = null;
        public Button closeButton;
        private DataPlayer dataPlayer;
        private bool isRefresh;
        public Vector3 UIPosition { get; set; }
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();
            dataPlayer= GameEntry.Data.GetData<DataPlayer>();
            closeButton.onClick.AddListener(OncloseButtonClick);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if(isRefresh)
            {
                acheivementName.text = dataAchievement.GetDataById(dataAchievement.cuurrentPopUpId).Name.ToString();
                isRefresh = !isRefresh;
            }
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            acheivementName.text = dataAchievement.GetDataById(dataAchievement.cuurrentPopUpId).Name.ToString();
            GameEntry.Event.Subscribe(AchievementMultiplesPopUpEventArgs.EventId, OnAchievementMultiplesPopUpEventArgs);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(AchievementMultiplesPopUpEventArgs.EventId, OnAchievementMultiplesPopUpEventArgs);

        }
        private void OncloseButtonClick()
        {
            GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(Constant.Type.UI_CLOSE));
        }
        private void OnAchievementMultiplesPopUpEventArgs(object sender, GameEventArgs e)
        {
            AchievementMultiplesPopUpEventArgs ne = (AchievementMultiplesPopUpEventArgs)e;
            if (ne == null)
                return;
            isRefresh = true;
        }
    }
}
