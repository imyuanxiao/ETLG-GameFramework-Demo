using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UISpaceshipCheckForm : UGuiFormEx
    {

        // buttons
        public Button returnButton;

        public Button allButton;
        public Button moduleButton;
        public Button specialButton;
        public Button tradeButton;
        public Button othersButton;


        // name and description
        public TextMeshProUGUI s_name = null;

        // type and size
        public TextMeshProUGUI s_type = null;
        public TextMeshProUGUI s_size = null;

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
        private readonly float maxAttrValue = 300;

        public GameObject s_durability_valueBar = null;
        public GameObject s_shields_valueBar = null;
        public GameObject s_energy_valueBar = null;
        public GameObject s_firepower_valueBar = null;
        public GameObject s_agility_valueBar = null;
        public GameObject s_speed_valueBar = null;
        public GameObject s_detection_valueBar = null;
        public GameObject s_capacity_valueBar = null;

        public TextMeshProUGUI playerMoney = null;

        public Transform artifactContainer = null;

        private DataPlayer dataPlayer;

        private PlayerCalculatedSpaceshipData currentSpaceshipData = null;

        private EntitySpaceshipSelect showSpaceshipEntity = null;

        // 实体加载器
        private EntityLoader entityLoader;

        private int selectedArtifactType;
        private bool refreshUI;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 绑定按钮点击事件
            returnButton.onClick.AddListener(OnReturnButtonClick);

            allButton.onClick.AddListener(OnPackageAllButtonClick);
            moduleButton.onClick.AddListener(OnPackageModuleButtonClick);
            specialButton.onClick.AddListener(OnPackageSpecialButtonClick);
            tradeButton.onClick.AddListener(OnPackageTradeButtonClick);
            othersButton.onClick.AddListener(OnPackageOthersButtonClick);


            // 获取玩家数据管理器
            dataPlayer = GameEntry.Data.GetData < DataPlayer>();

            entityLoader = EntityLoader.Create(this);


        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            currentSpaceshipData = dataPlayer.GetPlayerData().playerCalculatedSpaceshipData;

            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);

            ShowSpaceshipSelect();

            refreshUI = true;
            selectedArtifactType = Constant.Type.ARTIFACT_ALL;


        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (refreshUI)
            {
                HideAllItem();
                ShowArtifactIcons(artifactContainer, selectedArtifactType);
                refreshUI = false;
            }


        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            HideSpaceship();
        }

        private void OnReturnButtonClick()
        {
            Log.Debug("Return to Map");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }


        private void ShowArtifactIcons(Transform container, int type)
        {

            List<PlayerArtifactData> playerArtifacts = dataPlayer.GetPlayerData().getArtifactsByType(type);

            for (int i = 0; i < playerArtifacts.Count; i++)
            {

                Vector3 offset = new Vector3((i % 2) * 100f, (i / 2) * -150f, 0f);


                PlayerArtifactData playerArtifact = playerArtifacts[i];

                ShowItem<ItemArtifactIcon>(EnumItem.ArtifactIcon, (item) =>
                {
                    item.transform.SetParent(container, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + offset;
                    item.GetComponent<ItemArtifactIcon>().SetArtifactData(playerArtifact, Constant.Type.ARTIFACT_ICON_DEFAULT);
                });

            }
        }

        public void ShowSpaceshipSelect()
        {

            if (currentSpaceshipData == null)
            {
                Log.Error("Can not get spaceship data by from player data.");
                return;
            }

            s_name.text = dataPlayer.GetPlayerData().initialSpaceship.NameId;
            s_type.text = dataPlayer.GetPlayerData().initialSpaceship.SType;
            s_size.text = dataPlayer.GetPlayerData().initialSpaceship.SSize;

            playerMoney.text = dataPlayer.GetPlayerData().getArtifactNumById((int)EnumArtifact.Money).ToString();

            s_energy.text = currentSpaceshipData.Energy.ToString();
            s_durability.text = currentSpaceshipData.Durability.ToString();
            s_shields.text = currentSpaceshipData.Shields.ToString();
            s_firepower.text = currentSpaceshipData.Firepower.ToString();
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

            ShowSpaceship();

        }

        public void SetWidth(GameObject targetObject, float newWidth)
        {
            // 获取目标对象的 RectTransform 组件
            RectTransform rectTransform = targetObject.GetComponent<RectTransform>();

            // 修改 sizeDelta 属性的 x 值来改变宽度
            Vector2 newSizeDelta = rectTransform.sizeDelta;
            newSizeDelta.x = newWidth;
            rectTransform.sizeDelta = newSizeDelta * valueBarMaxWidth / maxAttrValue;
        }


        public void ShowSpaceship()
        {

            // 显示模型
            entityLoader.ShowEntity(dataPlayer.GetPlayerData().initialSpaceship.EntityId, TypeUtility.GetEntityType(dataPlayer.GetPlayerData().initialSpaceship.Type),
                (entity) =>
                {
                    showSpaceshipEntity = (EntitySpaceshipSelect)entity.Logic;
                },
                EntityDataSpaceshipSelect.Create(dataPlayer.GetPlayerData().initialSpaceship, true));

        }

        // 隐藏模型，这个方法会隐藏字典里所有的模型
        private void HideSpaceship()
        {   
            if(showSpaceshipEntity != null)
            {
                entityLoader.HideEntity(showSpaceshipEntity.Entity);
            }
        }

        public void OnPackageAllButtonClick()
        {
            refreshUI = true;
            selectedArtifactType = Constant.Type.ARTIFACT_ALL;
            ResetAllButtons();
            allButton.Select();

        }

        public void OnPackageModuleButtonClick()
        {
            refreshUI = true;
            selectedArtifactType = Constant.Type.ARTIFACT_MODULE;

            ResetAllButtons();
            moduleButton.Select();
        }

        public void OnPackageSpecialButtonClick()
        {
  

            refreshUI = true;
            selectedArtifactType = Constant.Type.ARTIFACT_SPECIAL;

            ResetAllButtons();
            specialButton.Select();
        }

        public void OnPackageTradeButtonClick()
        {
       

            refreshUI = true;
            selectedArtifactType = Constant.Type.ARTIFACT_TRADE;

            ResetAllButtons();
            tradeButton.Select();
        }

        public void OnPackageOthersButtonClick()
        {


            refreshUI = true;
            selectedArtifactType = Constant.Type.ARTIFACT_OTHERS;
            ResetAllButtons();
            othersButton.Select();
        }

        public void ResetAllButtons()
        {
            allButton.interactable = true;
            moduleButton.interactable = true;
            specialButton.interactable = true;
            tradeButton.interactable = true;
            othersButton.interactable = true;
        }


    }
}


