using ETLG.Data;
using GameFramework.Event;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UISpaceshipCheckForm : UGuiFormEx
    {

        public Button returnButton;

        // packages buttons

        public Button packageAllButton;
        public Button packageTradeButton;
        public Button packageSpecialButton;
        public Button packageOthersButton;

        // module buttons
        public Button moduleAllButton;
        public Button moduleWeaponButton;
        public Button moduleAttackButton;
        public Button moduleDefenseButton;
        public Button modulePowerdriveButton;
        public Button moduleSupportButton;

        // name and description
        public TextMeshProUGUI s_name = null;

        // type and size
        public TextMeshProUGUI s_type = null;
        public TextMeshProUGUI s_size = null;

        // initial attrs
        public TextMeshProUGUI s_durability = null;
        public TextMeshProUGUI s_shields = null;
        public TextMeshProUGUI s_agility = null;
        public TextMeshProUGUI s_energy = null;
        public TextMeshProUGUI s_firepower = null;
        public TextMeshProUGUI s_fireRate = null;

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

        private readonly float valueBarMaxWidth = 180;

        public TextMeshProUGUI playerMoney = null;
        public TextMeshProUGUI playerScore = null;

        public Transform artifactContainer = null;

        public Transform moduleContainer = null;

        public Transform moduleWeapon = null;
        public Transform moduleAttack = null;
        public Transform moduleDefense = null;
        public Transform modulePowerDrive = null;
        public Transform moduleSupport1 = null;
        public Transform moduleSupport2 = null;

        //public Transform equippedModuleContainer = null;

        private DataPlayer dataPlayer;

        private EntitySpaceshipSelect showSpaceshipEntity = null;

        // 实体加载器
        private EntityLoader entityLoader;

        private int selectedArtifactType;
        private int selectedModuleType;

        private bool refreshAll;
        private bool refreshUI;

        private Button currentSelectedButton;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            currentSelectedButton = null;

            // 绑定按钮点击事件
            returnButton.onClick.AddListener(OnReturnButtonClick);

            packageAllButton.onClick.AddListener(OnPackageAllButtonClick);
            packageTradeButton.onClick.AddListener(OnPackageTradeButtonClick);
            packageSpecialButton.onClick.AddListener(OnPackageSpecialButtonClick);
            packageOthersButton.onClick.AddListener(OnPackageOthersButtonClick);

            moduleAllButton.onClick.AddListener(OnModuleAllButtonClick);
            moduleWeaponButton.onClick.AddListener(OnModuleWeaponButtonClick);
            moduleAttackButton.onClick.AddListener(OnModuleAttackButtonClick);
            moduleDefenseButton.onClick.AddListener(OnModuleDefenseButtonClick);
            modulePowerdriveButton.onClick.AddListener(OnModulePowerdriveButtonClick);
            moduleSupportButton.onClick.AddListener(OnModuleSupportButtonClick);

            // 获取玩家数据管理器
            dataPlayer = GameEntry.Data.GetData < DataPlayer>();

            entityLoader = EntityLoader.Create(this);


        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(EquippedModuleChangesEventArgs.EventId, OnEquippedModuleChanges);

            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);

            refreshAll = true;

            selectedArtifactType = Constant.Type.ARTIFACT_ALL;
            selectedModuleType = Constant.Type.ARTIFACT_ALL;

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (refreshAll)
            {
                ShowSpaceship();
            }

            if (refreshAll || refreshUI)
            {
                ShowContent();
                refreshAll = false;
                refreshUI = false;
            }

            ResetAllButtons();

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(EquippedModuleChangesEventArgs.EventId, OnEquippedModuleChanges);

            HideSpaceship();
        }

        private void OnReturnButtonClick()
        {
            Log.Debug("Return to Map");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }


        private void ShowArtifactIcons(Transform container, int Type)
        {

            Dictionary<int, int> playerArtifacts = dataPlayer.GetPlayerData().GetArtifactsByType(Type);
            int i = 0;
            foreach (KeyValuePair<int, int> kvp in playerArtifacts)
            {
                int ArtifactID = kvp.Key;
                int Num = kvp.Value;

                Vector3 offset = new Vector3((i % 4) * 90f + 15f, (i / 4) * (-110f) - 10f, 0f);
                i++;
                if (ArtifactID == (int)EnumArtifact.Money || ArtifactID == (int)EnumArtifact.KnowledgePoint)
                {
                    i--;
                    continue;
                }

                ShowItem<ItemArtifactIcon>(EnumItem.ItemArtifactIcon, (item) =>
                {
                    item.transform.SetParent(container, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + offset;
                    item.GetComponent<ItemArtifactIcon>().SetArtifactData(ArtifactID, Num, Constant.Type.ARTIFACT_ICON_DEFAULT);
                });

            }
        }

        private void ShowModuleIcons(Transform container, int Type)
        {
            List<int> playerModuleIDs = dataPlayer.GetPlayerData().GetModulesByType(Type);
            for(int i = 0; i <  playerModuleIDs.Count; i++)
            {
                int ModuleID = playerModuleIDs[i];
                Vector3 offset = new Vector3((i % 6) * 90f + 10f, (i / 6) * -90f - 10f, 0f);
                ShowItem<ItemModuleIcon>(EnumItem.ItemModuleIcon, (item) =>
                {
                    item.transform.SetParent(container, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + offset;
                    item.GetComponent<ItemModuleIcon>().SetModuleData(ModuleID, false);
                });
            }

        }

        private void ShowEquipeedModuleIcons()
        {
            int[] playerEquippedModuleIDs = dataPlayer.GetPlayerData().GetEquippedModuleIdS();

            if (playerEquippedModuleIDs[0] != 0) ShowIcon(moduleWeapon, playerEquippedModuleIDs[0]);
            if (playerEquippedModuleIDs[1] != 0) ShowIcon(moduleAttack, playerEquippedModuleIDs[1]);
            if (playerEquippedModuleIDs[2] != 0) ShowIcon(moduleDefense, playerEquippedModuleIDs[2]);
            if (playerEquippedModuleIDs[3] != 0) ShowIcon(modulePowerDrive, playerEquippedModuleIDs[3]);
            if (playerEquippedModuleIDs[4] != 0) ShowIcon(moduleSupport1, playerEquippedModuleIDs[4]);
            if (playerEquippedModuleIDs[5] != 0) ShowIcon(moduleSupport2, playerEquippedModuleIDs[5]);

        }

        private void ShowIcon(Transform container, int ModuleID)
        {
            ShowItem<ItemModuleIcon>(EnumItem.ItemModuleIcon, (item) =>
            {
                item.transform.SetParent(container, false);
                item.transform.localScale = Vector3.one;
                item.transform.eulerAngles = Vector3.zero;
                item.transform.localPosition = Vector3.zero;
                item.GetComponent<ItemModuleIcon>().SetModuleData(ModuleID, true);
            });
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

            s_name.text = dataPlayer.GetPlayerData().initialSpaceship.NameId;
            s_type.text = dataPlayer.GetPlayerData().initialSpaceship.SType;
            s_size.text = dataPlayer.GetPlayerData().initialSpaceship.SSize;

            playerMoney.text = dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.Money).ToString();
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

            HideAllItem();
            ShowArtifactIcons(artifactContainer, selectedArtifactType);
            ShowModuleIcons(moduleContainer, selectedModuleType);
            ShowEquipeedModuleIcons();

        }

        public void SetWidth(GameObject targetObject, float newWidth)
        {
            newWidth = newWidth * valueBarMaxWidth / Constant.Type.ATTR_MAX_VALUE;

            // 获取目标对象的 RectTransform 组件
            RectTransform rectTransform = targetObject.GetComponent<RectTransform>();

            // 修改 sizeDelta 属性的 x 值来改变宽度
            Vector2 newSizeDelta = rectTransform.sizeDelta;
            newSizeDelta.x = newWidth;
            rectTransform.sizeDelta = newSizeDelta;
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
            currentSelectedButton = packageAllButton;

        }

        public void OnPackageTradeButtonClick()
        {


            refreshUI = true;
            selectedArtifactType = Constant.Type.ARTIFACT_TRADE;

            currentSelectedButton = packageTradeButton;
        }

        public void OnPackageSpecialButtonClick()
        {
            refreshUI = true;
            selectedArtifactType = Constant.Type.ARTIFACT_SPECIAL;
            currentSelectedButton = packageSpecialButton;
        }


        public void OnPackageOthersButtonClick()
        {
            refreshUI = true;
            selectedArtifactType = Constant.Type.ARTIFACT_OTHERS;
            currentSelectedButton = packageOthersButton;
        }

        public void ResetAllButtons()
        {
            packageAllButton.interactable = true;
            packageTradeButton.interactable = true;
            packageSpecialButton.interactable = true;
            packageOthersButton.interactable = true;
            moduleAllButton.interactable = true;
            moduleWeaponButton.interactable = true;
            moduleAttackButton.interactable = true;
            moduleDefenseButton.interactable = true;
            modulePowerdriveButton.interactable = true;
            moduleSupportButton.interactable = true;

            if(currentSelectedButton != null){
                currentSelectedButton.Select();
            }
        }

        public void OnModuleAllButtonClick()
        {
            refreshUI = true;
            selectedModuleType = Constant.Type.MODULE_TYPE_ALL;
            currentSelectedButton = moduleAllButton;
        }
        public void OnModuleWeaponButtonClick()
        {
            refreshUI = true;
            selectedModuleType = Constant.Type.MODULE_TYPE_WEAPON;
            currentSelectedButton = moduleWeaponButton;
        }


        public void OnModuleAttackButtonClick()
        {
            refreshUI = true;
            selectedModuleType = Constant.Type.MODULE_TYPE_ATTACK;
            currentSelectedButton = moduleAttackButton;
        }

        public void OnModuleDefenseButtonClick()
        {
            refreshUI = true;
            selectedModuleType = Constant.Type.MODULE_TYPE_DEFENSE;
            currentSelectedButton = moduleDefenseButton;
        }

        public void OnModulePowerdriveButtonClick()
        {
            refreshUI = true;
            selectedModuleType = Constant.Type.MODULE_TYPE_POWERDRIVE;
            currentSelectedButton = modulePowerdriveButton;
        }
        public void OnModuleSupportButtonClick()
        {
            refreshUI = true;
            selectedModuleType = Constant.Type.MODULE_TYPE_SUPPORT;
            currentSelectedButton = moduleSupportButton;
        }

        public void OnEquippedModuleChanges(object sender, GameEventArgs e)
        {
            EquippedModuleChangesEventArgs ne = (EquippedModuleChangesEventArgs)e;
            if (ne == null)
                return;

            refreshUI = true;
        }


    }
}


