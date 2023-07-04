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

            UIContainer.position = dataSkill.skillInfoPosition;

            SkillName.text = skillData.Name;
            Domain.text = skillData.Domain;
            IsActiveSkill.text = skillData.IsActiveSkill ? "Active" : "Passive";
            IsCombatSkill.text = skillData.IsCombatSkill ? "Combat" : "Explore";

            SkillDescription.text = skillData.Name + "To be added";

            // set skill icon            
            Texture texture = Resources.Load<Texture>(AssetUtility.GetSkillIcon(skillData.Id.ToString(), "2"));
            if (texture != null)
            {
                skillIcon.texture = texture;
            }

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            skillData = null;
            base.OnClose(isShutdown, userData);

        }
 

    }
}


