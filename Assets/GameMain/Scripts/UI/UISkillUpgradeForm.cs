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
    public class UISkillUpgradeForm : UGuiFormEx
    {

        public DataSkill dataSkill;
        public DataPlayer dataPlayer;

        public Transform UIContainer;
        public TextMeshProUGUI Title = null;
        public Button CancelButton;
        public Button UpgradeButton;
        public bool refresh;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataSkill = GameEntry.Data.GetData<DataSkill>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            CancelButton.onClick.AddListener(OnCancelButtonClick);
            UpgradeButton.onClick.AddListener(OnUpgradeButtonClick);

        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            refresh = true;

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (refresh)
            {
                showContent();
                refresh = false;
            }
        }

        public void showContent()
        {
            UIContainer.position = dataSkill.skillUpgradeInfoPosition;

            bool isMaxLevel = dataPlayer.GetPlayerData().getSkillById(dataSkill.currentSkillID).Level - 1 >= dataSkill.GetCurrentShowSkillData().GetMaxLevelIndex();
            UpgradeButton.interactable = !isMaxLevel && dataSkill.CanUpgradeCurrentSkill;

            string title = dataSkill.CanUpgradeCurrentSkill ? "Upgrade skill" : "Lack of upgrade materials.";

            Title.text = isMaxLevel ? "Cannot be upgraded any more." : title;

        }

        public void OnCancelButtonClick()
        {
            GameEntry.Event.Fire(this, SkillUpgradeInfoUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }

        public void OnUpgradeButtonClick()
        {
            Log.Debug("click upgrade");
        }

    }
}


