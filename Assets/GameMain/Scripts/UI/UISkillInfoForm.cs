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

        public Transform UIContainer;

        private SkillData skillData;

        public RawImage skillIcon;
        public TextMeshProUGUI SkillName = null;
        public TextMeshProUGUI Domain = null;
        public TextMeshProUGUI IsActiveSkill;
        public TextMeshProUGUI IsCombatSkill;
        public TextMeshProUGUI SkillDescription;

        public TextMeshProUGUI CurrentLevel;
        public TextMeshProUGUI CurrentLevelDescription;
        public TextMeshProUGUI NextLevelDescription;



        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            skillData = GameEntry.Data.GetData<DataSkill>().GetCurrentShowSkillData();
            UIContainer.position = GameEntry.Data.GetData<DataSkill>().skillInfoPosition;

            SkillName.text = skillData.Name;
            Domain.text = skillData.Domain;
            IsActiveSkill.text = skillData.IsActiveSkill ? "Active" : "Passive";
            IsCombatSkill.text = skillData.IsCombatSkill ? "Combat" : "Explore";
            CurrentLevel.text = skillData.CurrentLevel.ToString();
            SkillDescription.text = skillData.Name + "To be added";
            CurrentLevelDescription.text = skillData.Name + "To be added. Reduce active skill energy consumption by <color=#FF00FF>3 </color> %, increase energy cap by <color=#FF00FF> 3 </color> %.";
            NextLevelDescription.text = skillData.Name + "To be added. Reduce active skill energy consumption by <color=#FF00FF>5 </color> %, increase energy cap by <color=#FF00FF> 5 </color> %.";


        // set skill icon            
        Texture texture = Resources.Load<Texture>(AssetUtility.GetSkillIcon(skillData.Id.ToString(), "1"));
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


