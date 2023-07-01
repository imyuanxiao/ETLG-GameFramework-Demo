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
    public class UISkillInfoForm : UGuiFormEx
    {

        public TextMeshProUGUI skillName = null;

        public TextMeshProUGUI domainValue = null;


        private SkillData skillData;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("已打开SkillInfoForm");

            skillData = GameEntry.Data.GetData<DataSkill>().GetCurrentShowSkillData();
            skillName.text = skillData.Name;
            domainValue.text = skillData.Domain;

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            skillData = null;
            Log.Debug("已关闭SkillInfoForm");

            base.OnClose(isShutdown, userData);


        }


    }
}


