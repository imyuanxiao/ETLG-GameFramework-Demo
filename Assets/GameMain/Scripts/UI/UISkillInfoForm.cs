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

        public bool hideBottomPart { get; set; }

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataSkill = GameEntry.Data.GetData<DataSkill>();

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            skillData = dataSkill.GetCurrentShowSkillData();
            playerSkillData = dataSkill.currentPlayerSkillData;

            UIContainer.position = dataSkill.skillInfoPosition;
            hideBottomPart = dataSkill.hideSkillInfoBottomPart;

            SkillName.text = skillData.Name;
            Domain.text = skillData.Domain;
            IsActiveSkill.text = skillData.IsActiveSkill ? "Active" : "Passive";
            IsCombatSkill.text = skillData.IsCombatSkill ? "Combat" : "Explore";

            CurrentLevel.text = playerSkillData.Level.ToString();
            CurrentLevelDescription.text = skillData.Name + "To be added. Reduce active skill energy consumption by <color=#FF00FF>3 </color> %, increase energy cap by <color=#FF00FF> 3 </color> %.";

            SkillDescription.text = skillData.Name + "To be added";

            // set skill icon            
            Texture texture = Resources.Load<Texture>(AssetUtility.GetSkillIcon(skillData.Id.ToString(), playerSkillData.ActiveState.ToString()));
            if (texture != null)
            {
                skillIcon.texture = texture;
            }

            if (hideBottomPart)
            {
                bottomPart.SetActive(false);
            }
            else
            {
                bottomPart.SetActive(true);
                NextLevelDescription.text = skillData.Name + "To be added. Reduce active skill energy consumption by <color=#FF00FF>5 </color> %, increase energy cap by <color=#FF00FF> 5 </color> %.";

            }

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            skillData = null;
            playerSkillData = null;
            base.OnClose(isShutdown, userData);

        }
 

    }
}


