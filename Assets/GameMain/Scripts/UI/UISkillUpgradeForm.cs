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
       // private SkillData skillData;

        public DataPlayer dataPlayer;

        //public PlayerSkillData playerSkillData;

        public Transform UIContainer;
        public Transform CostsContainer;

        public RawImage skillIcon;
        public TextMeshProUGUI SkillName = null;
        public TextMeshProUGUI Domain = null;
        public TextMeshProUGUI Activeness;
        public TextMeshProUGUI Functionality;
        public TextMeshProUGUI SkillDescription;

        public Button CancelButton;
        public Button UpgradeButton;

        public GameObject CostsContainerObj;
        public GameObject TipsContainerObj;

        public bool hideBottomPart { get; set; }

        public bool refresh;

        // 初始化菜单数据
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
            SkillData skillData = dataSkill.GetCurrentShowSkillData();
            
            UIContainer.position = dataSkill.skillInfoPosition;

            SkillName.text = skillData.Name;
            Domain.text = skillData.Domain;

            Activeness.text = skillData.Activeness;
            Functionality.text = skillData.Functionality;

            SkillDescription.text = skillData.GetSkillDescription();

            bool isMaxLevel = dataPlayer.GetPlayerData().getSkillById(dataSkill.currentSkillID).Level - 1 >= skillData.GetMaxLevelIndex();

            // set skill icon            
            Texture texture = Resources.Load<Texture>(AssetUtility.GetSkillIcon(skillData.Id.ToString(), "2"));
            if (texture != null)
            {
                skillIcon.texture = texture;
            }

            if (hideBottomPart || isMaxLevel)
            {
                UpgradeButton.interactable = false;
                CostsContainerObj.SetActive(false);
                TipsContainerObj.SetActive(true);
            }
            else
            {
                UpgradeButton.interactable = true;
                CostsContainerObj.SetActive(true);
                TipsContainerObj.SetActive(false);
                ShowCosts(CostsContainer, skillData.GetLevelCosts(
                    dataPlayer.GetPlayerData().getSkillById(dataSkill.currentSkillID).Level + 1)
                );
            }

        }


        private void ShowCosts(Transform container, int[] costs)
        {
            for (int i = 0; i < costs.Length; i += 2) {
                int artifactId = costs[i];
                int hasNum = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().getArtifactNumById(artifactId);
                int needNum = costs[i + 1];
                ShowItem<ItemCostResBar>(EnumItem.CostResBar, (item) =>
                {
                    item.transform.SetParent(container, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero;
                    item.GetComponent<ItemCostResBar>().SetCostResData(artifactId, hasNum, needNum);
                });
            }
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


