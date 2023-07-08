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
        private SkillData skillData;
        public PlayerSkillData playerSkillData;

        public Transform UIContainer;
        public Transform CostsContainer;

        public RawImage skillIcon;
        public TextMeshProUGUI SkillName = null;
        public TextMeshProUGUI Domain = null;
        public TextMeshProUGUI IsActiveSkill;
        public TextMeshProUGUI IsCombatSkill;
        public TextMeshProUGUI SkillDescription;

        public Button CancelButton;
        public Button OkButton;


        public bool refresh;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataSkill = GameEntry.Data.GetData<DataSkill>();

        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);


            if (refresh)
            {
                showContent();
                refresh = false;
            }


        }

        public void showContent()
        {
            skillData = dataSkill.GetCurrentShowSkillData();
            playerSkillData = dataSkill.currentPlayerSkillData;

            UIContainer.position = dataSkill.skillInfoPosition;

            SkillName.text = skillData.Name;
            Domain.text = skillData.Domain;
            IsActiveSkill.text = skillData.IsActiveSkill ? "Active" : "Passive";
            IsCombatSkill.text = skillData.IsCombatSkill ? "Combat" : "Explore";

            SkillDescription.text = skillData.GetSkillDescription();

            // set skill icon            
            Texture texture = Resources.Load<Texture>(AssetUtility.GetSkillIcon(skillData.Id.ToString(), "2"));
            if (texture != null)
            {
                skillIcon.texture = texture;
            }

            ShowCosts(CostsContainer, skillData.GetLevelCosts(playerSkillData.Level + 1));

        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void ShowCosts(Transform container, int[] costs)
        {

            // 展示内容需要 玩家有该道具数，需要道具数，

            for (int i = 0; i < costs.Length; i += 2)
            {
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


    }
}


