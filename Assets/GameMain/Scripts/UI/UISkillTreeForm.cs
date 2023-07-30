using ETLG.Data;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UISkillTreeForm : UGuiFormEx
    {

        // buttons
        public Button resetButton;
        public Button returnButton;


        // initial attrs
        public TextMeshProUGUI s_durability = null;
        public TextMeshProUGUI s_shields = null;
        public TextMeshProUGUI s_agility = null;
        public TextMeshProUGUI s_energy = null;
        public TextMeshProUGUI s_fireRate = null;
        public TextMeshProUGUI s_firepower = null;

        private readonly float valueBarMaxWidth = 180;

        public GameObject s_durability_initialVal = null;
        public GameObject s_shields_initialVal = null;
        public GameObject s_agility_initialVal = null;
        public GameObject s_energy_initialVal = null;
        public GameObject s_firepower_initialVal = null;
        public GameObject s_fireRate_initialVal = null;

        public GameObject s_durability_increVal = null;
        public GameObject s_shields_increVal = null;
        public GameObject s_agility_increVal = null;
        public GameObject s_energy_increVal = null;
        public GameObject s_firepower_increVal = null;
        public GameObject s_fireRate_increVal = null;

        public TextMeshProUGUI playerKnowledgePoints = null;
        public TextMeshProUGUI playerScore = null;

        public TextMeshProUGUI totalSkillsNum = null;
        public TextMeshProUGUI totalLevelsNum = null;
        public TextMeshProUGUI unlockedSkillsNum = null;
        public TextMeshProUGUI upgradedLevelsNum = null;


        private DataPlayer dataPlayer;
        private DataSkill dataSkill;

        private bool refreshLeftUI;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            returnButton.onClick.AddListener(OnReturnButtonClick);
            resetButton.onClick.AddListener(OnResetButtonClick);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataSkill = GameEntry.Data.GetData<DataSkill>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(SkillUpgradedEventArgs.EventId, OnSkillUpgraded);

            GameEntry.UI.OpenUIForm(EnumUIForm.UISkillTreeMap);
            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);

            refreshLeftUI = true;

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (refreshLeftUI)
            {
                ShowContent();
                refreshLeftUI = false; 
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(SkillUpgradedEventArgs.EventId, OnSkillUpgraded);


        }

        private void OnReturnButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }

        private void OnResetButtonClick()
        {
            if (dataSkill.lockCurrentSkillID)
            {
                return;
            }
            dataPlayer.GetPlayerData().ResetSkills();
        }

        public void ShowContent()
        {
            SpaceshipData initialSpaceshipData = dataPlayer.GetPlayerData().initialSpaceship;
            PlayerCalculatedSpaceshipData currentSpaceshipData = dataPlayer.GetPlayerData().playerCalculatedSpaceshipData;

            if (currentSpaceshipData == null)
            {
                Log.Error("Can not get spaceship data by from player data.");
                return;
            }

            playerKnowledgePoints.text = dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.KnowledgePoint).ToString();

            playerScore.text = dataPlayer.GetPlayerData().GetPlayerScore().ToString();


            s_energy.text = currentSpaceshipData.Energy.ToString();
            s_durability.text = currentSpaceshipData.Durability.ToString();
            s_shields.text = currentSpaceshipData.Shields.ToString();
            s_firepower.text = currentSpaceshipData.Firepower.ToString();
            s_fireRate.text = currentSpaceshipData.FireRate.ToString();
            s_agility.text = currentSpaceshipData.Agility.ToString();

            SetWidth(s_durability_initialVal, initialSpaceshipData.Durability);
            SetWidth(s_shields_initialVal, initialSpaceshipData.Shields);
            SetWidth(s_agility_initialVal, initialSpaceshipData.Agility);
            SetWidth(s_energy_initialVal, initialSpaceshipData.Energy);
            SetWidth(s_firepower_initialVal, initialSpaceshipData.Firepower);
            SetWidth(s_fireRate_initialVal, initialSpaceshipData.FireRate);

            SetWidth(s_durability_increVal, currentSpaceshipData.Durability - initialSpaceshipData.Durability);
            SetWidth(s_shields_increVal, currentSpaceshipData.Shields - initialSpaceshipData.Shields);
            SetWidth(s_agility_increVal, currentSpaceshipData.Agility - initialSpaceshipData.Agility);
            SetWidth(s_energy_increVal, currentSpaceshipData.Energy - initialSpaceshipData.Energy);
            SetWidth(s_firepower_increVal, currentSpaceshipData.Firepower - initialSpaceshipData.Firepower);
            SetWidth(s_fireRate_increVal, currentSpaceshipData.FireRate - initialSpaceshipData.FireRate);


            totalSkillsNum.text = "/" + dataSkill.skillCount.ToString();
            totalLevelsNum.text = "/" + dataSkill.levelCount.ToString();
            unlockedSkillsNum.text = dataPlayer.GetPlayerData().GetUnlockedSkillsNum().ToString();
            upgradedLevelsNum.text = dataPlayer.GetPlayerData().GetUnlockedLevelsNum().ToString();

        }

        public void SetWidth(GameObject targetObject, float newWidth)
        {
            newWidth = newWidth * valueBarMaxWidth / Constant.Type.ATTR_MAX_VALUE;

            RectTransform rectTransform = targetObject.GetComponent<RectTransform>();

            Vector2 newSizeDelta = rectTransform.sizeDelta;
            newSizeDelta.x = newWidth;
            rectTransform.sizeDelta = newSizeDelta;
        }

        public void OnSkillUpgraded(object sender, GameEventArgs e)
        {
            SkillUpgradedEventArgs ne = (SkillUpgradedEventArgs)e;
            if (ne == null)
                return;

            refreshLeftUI = true;
        }


    }
}


