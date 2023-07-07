using ETLG.Data;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UISkillInfoForm : UGuiFormEx
    {

        public DataSkill dataSkill;
        private SkillData skillData;
        public PlayerSkillData playerSkillData;

        public Transform UIContainer;

        public RawImage skillIcon;
        public TextMeshProUGUI SkillName = null;
        public TextMeshProUGUI Domain = null;
        public TextMeshProUGUI IsActiveSkill;
        public TextMeshProUGUI IsCombatSkill;
        public TextMeshProUGUI SkillDescription;

        public TextMeshProUGUI CurrentLevel;
        public TextMeshProUGUI CurrentLevelDescription;
        public TextMeshProUGUI NextLevelDescription;

        public GameObject bottomPart;

        public Transform costsContainer;

        public bool hideBottomPart { get; set; }

        public bool refresh;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataSkill = GameEntry.Data.GetData<DataSkill>();

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

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            refresh = true;
        }

        public void showContent()
        {
            skillData = dataSkill.GetCurrentShowSkillData();
            playerSkillData = dataSkill.currentPlayerSkillData;

            UIContainer.position = dataSkill.skillInfoPosition;
            hideBottomPart = dataSkill.hideSkillInfoBottomPart;

            SkillName.text = skillData.Name;
            Domain.text = skillData.Domain;
            IsActiveSkill.text = skillData.IsActiveSkill ? "Active" : "Passive";
            IsCombatSkill.text = skillData.IsCombatSkill ? "Combat" : "Explore";
            SkillDescription.text = skillData.GetSkillDescription();

            int currentLevel = playerSkillData.Level;

            bool isMaxLevel = currentLevel - 1 >= skillData.GetMaxLevelIndex();
            string max = isMaxLevel ? " (Max)" : "";

            CurrentLevel.text = playerSkillData.Level.ToString() + max;
            CurrentLevelDescription.text = skillData.GetLevelEffectByLevel(currentLevel);


            // set skill icon            
            string texturePath = AssetUtility.GetSkillIcon(playerSkillData.Id.ToString());
            Texture texture = Resources.Load<Texture>(texturePath);
            if (texture == null)
            {
                texturePath = AssetUtility.GetIconMissing();
                texture = Resources.Load<Texture>(texturePath);
            }
            skillIcon.texture = texture;

            if (hideBottomPart || isMaxLevel)
            {
                bottomPart.SetActive(false);
            }
            else
            {
                bottomPart.SetActive(true);
                NextLevelDescription.text = skillData.GetLevelEffectByLevel(currentLevel + 1);
                ShowCosts(costsContainer, skillData.GetLevelCosts(currentLevel + 1));
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            skillData = null;
            playerSkillData = null;
            SkillDescription.text = null;
            CurrentLevelDescription.text = null;
            NextLevelDescription.text = null;

            base.OnClose(isShutdown, userData);

        }

        private void ShowCosts(Transform container, int[] costs)
        {

            // 展示内容需要 玩家有该道具数，需要道具数，

            for(int i = 0; i < costs.Length; i += 2)
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


