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
        public Button CloseButton;
        public Button UpgradeButton;
        public bool refresh;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataSkill = GameEntry.Data.GetData<DataSkill>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            CancelButton.onClick.AddListener(OnCancelButtonClick);
            CloseButton.onClick.AddListener(OnCancelButtonClick);
            UpgradeButton.onClick.AddListener(OnUpgradeButtonClick);


        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(SkillUpgradedEventArgs.EventId, OnSkillUpgraded);
            GameEntry.Event.Subscribe(SkillUpgradedLackCostEventArgs.EventId, OnSkillUpgradedLackCost);

            refresh = true;

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(SkillUpgradedEventArgs.EventId, OnSkillUpgraded);
            GameEntry.Event.Unsubscribe(SkillUpgradedLackCostEventArgs.EventId, OnSkillUpgradedLackCost);
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
            UIContainer.position = dataSkill.skillUpgradeInfoPosition + new Vector3(0f, 50f,0f);

            bool isMaxLevel = dataPlayer.GetPlayerData().GetSkillLevelById(dataSkill.currentSkillId) - 1 >= dataSkill.GetCurrentSkillData().GetMaxLevelIndex();
            
            UpgradeButton.interactable = !isMaxLevel && dataSkill.CanUpgradeCurrentSkill;

            string title = dataSkill.CanUpgradeCurrentSkill ? "Upgrade skill" : "Lack of upgrade materials.";

            Title.text = isMaxLevel ? "Cannot be upgraded any more." : title;

            if(Constant.Type.SKILL_LOCKED == dataPlayer.GetPlayerData().GetSkillLevelById(dataSkill.currentSkillId))
            {
                Title.text = "Unlock this skill by gettting Landing Point Reward";
                UpgradeButton.interactable = false;
            }


        }

        public void OnCancelButtonClick()
        {
            dataSkill.lockCurrentSkillID = false;
            GameEntry.Event.Fire(this, SkillUpgradeInfoUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }

        public void OnUpgradeButtonClick()
        {
            dataPlayer.GetPlayerData().UpgradeCurrentSkill();
        }

        public void OnSkillUpgraded(object sender, GameEventArgs e)
        {
            SkillUpgradedEventArgs ne = (SkillUpgradedEventArgs)e;
            if (ne == null)
                return;
            refresh = true;
        }
         public void OnSkillUpgradedLackCost(object sender, GameEventArgs e)
        {
            SkillUpgradedLackCostEventArgs ne = (SkillUpgradedLackCostEventArgs)e;
            if (ne == null)
                return;
            refresh = true;
        }

    }
}


