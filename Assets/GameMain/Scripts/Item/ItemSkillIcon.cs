using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemSkillIcon : ItemLogicEx
    {

        private SkillData skillData;

        public RawImage skillIcon;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetSkillData(SkillData skillData)
        {
            this.skillData = skillData;

            string state = skillData.ActiveState.ToString();
            string texturePath = AssetUtility.GetSkillIcon(skillData.Id.ToString(), state);

            // 根据当前技能ID 和 激活状态 获取图标
            Texture texture = Resources.Load<Texture>(texturePath);

            if (texture != null)
            {
                // 将加载的纹理赋值给Raw Image的纹理
                skillIcon.texture = texture;
            }
            else
            {
                Debug.LogError("Failed to load texture: " + texturePath);
            }

        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

        }

    }
}


