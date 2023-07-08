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
        public TextMeshProUGUI s_firepower = null;
        public TextMeshProUGUI s_energy = null;
        public TextMeshProUGUI s_agility = null;
        public TextMeshProUGUI s_speed = null;
        public TextMeshProUGUI s_detection = null;
        public TextMeshProUGUI s_capacity = null;
        public TextMeshProUGUI s_fireRate = null;
        public TextMeshProUGUI s_dogde = null;

        private readonly float valueBarMaxWidth = 180;

        public GameObject s_durability_valueBar = null;
        public GameObject s_shields_valueBar = null;
        public GameObject s_energy_valueBar = null;
        public GameObject s_firepower_valueBar = null;
        public GameObject s_agility_valueBar = null;
        public GameObject s_speed_valueBar = null;
        public GameObject s_detection_valueBar = null;
        public GameObject s_capacity_valueBar = null;

        public TextMeshProUGUI playerKnowledgePoints = null;
        public TextMeshProUGUI playerScore = null;

        private DataPlayer dataPlayer;
        
        private PlayerCalculatedSpaceshipData currentSpaceshipData = null;

        private bool refreshLeftUI;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            returnButton.onClick.AddListener(OnReturnButtonClick);
            resetButton.onClick.AddListener(OnResetButtonClick);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(SkillUpgradedEventArgs.EventId, OnSkillUpgraded);

            GameEntry.UI.OpenUIForm(EnumUIForm.UISkillTreeMap);
            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);

            currentSpaceshipData = dataPlayer.GetPlayerData().playerCalculatedSpaceshipData;

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
            Log.Debug("Reset skill data");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // reset skill data，此处等实现clone方法后再完善

            dataPlayer.GetPlayerData().ResetSkills();
        }

        public void ShowContent()
        {

            if (currentSpaceshipData == null)
            {
                Log.Error("Can not get spaceship data by from player data.");
                return;
            }

            playerKnowledgePoints.text = dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.KnowledgePoint).ToString();

            playerScore.text = dataPlayer.GetPlayerData().GetPlayerScore().ToString();

            s_durability.text = currentSpaceshipData.Durability.ToString();
            s_shields.text = currentSpaceshipData.Shields.ToString();
            s_firepower.text = currentSpaceshipData.Firepower.ToString();
            s_energy.text = currentSpaceshipData.Energy.ToString();
            s_fireRate.text = currentSpaceshipData.FireRate.ToString();
            s_agility.text = currentSpaceshipData.Agility.ToString();
            s_speed.text = currentSpaceshipData.Speed.ToString();
            s_detection.text = currentSpaceshipData.Detection.ToString();
            s_capacity.text = currentSpaceshipData.Capacity.ToString();
            s_dogde.text = currentSpaceshipData.Dogde.ToString();

            SetWidth(s_energy_valueBar, currentSpaceshipData.Energy);
            SetWidth(s_durability_valueBar, currentSpaceshipData.Durability);
            SetWidth(s_shields_valueBar, currentSpaceshipData.Shields);
            SetWidth(s_firepower_valueBar, currentSpaceshipData.Firepower);
            SetWidth(s_agility_valueBar, currentSpaceshipData.Agility);
            SetWidth(s_speed_valueBar, currentSpaceshipData.Speed);
            SetWidth(s_detection_valueBar, currentSpaceshipData.Detection);
            SetWidth(s_capacity_valueBar, currentSpaceshipData.Capacity);


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


