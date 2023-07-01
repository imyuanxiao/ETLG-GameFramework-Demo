using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemSkillIcon : ItemLogicEx
    {

        private SkillData skillData;

        public RawImage skillIcon;

        public Button iconButton;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            iconButton.onClick.AddListener(OnIconButtonClick);

            EventTrigger trigger = iconButton.gameObject.AddComponent<EventTrigger>();

            // 添加鼠标进入事件监听器
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { OnIconPointerEnter(); });
            trigger.triggers.Add(enterEntry);

            // 添加鼠标移出事件监听器
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { OnIconPointerExit(); });
            trigger.triggers.Add(exitEntry);
        }

        private void OnIconButtonClick()
        {
          /*  GameEntry.Data.GetData<DataSkill>().currentSkillID = this.skillData.Id;

            GameEntry.Event.Fire(this, SkillInfoOpenEventArgs.Create());*/

        }

        private void OnIconPointerEnter()
        {
            Debug.Log("Mouse entered iconButton");

            GameEntry.Data.GetData<DataSkill>().currentSkillID = this.skillData.Id;

            // 显示技能信息UI
            GameEntry.Event.Fire(this, SkillInfoOpenEventArgs.Create());

        }

        private void OnIconPointerExit()
        {
            Debug.Log("Mouse exited iconButton");

            // 关闭技能信息UI
            GameEntry.Event.Fire(this, SkillInfoCloseEventArgs.Create());


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


