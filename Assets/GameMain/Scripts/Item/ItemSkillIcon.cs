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

        private int sceneID;


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
            // TODO 显示当前技能所需的升级资源和确认按钮

        }

        private void OnIconPointerEnter()
        {

            // 获得挂载对象的位置
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 newPosition = itemPosition + new Vector3(100f, 0f, 0f);


            GameEntry.Data.GetData<DataSkill>().currentSkillID = this.skillData.Id;
            GameEntry.Data.GetData<DataSkill>().skillInfoPosition = newPosition;

            // 显示skill info ui 的事件，传入UI应该显示的位置
            GameEntry.Event.Fire(this, SkillInfoOpenEventArgs.Create());

        }

        private void OnIconPointerExit()
        {
            // 关闭技能信息UI
            GameEntry.Event.Fire(this, SkillInfoCloseEventArgs.Create());

        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetSkillData(SkillData skillData, int sceneID)
        {
            this.skillData = skillData;

            string texturePath = "";

            // 根据场景ID，展示不同icon
            this.sceneID = sceneID;
            // 3 == 新建游戏
            if (sceneID == 3)
            {
                texturePath = AssetUtility.GetSkillIcon(skillData.Id.ToString(), "2");
            }
            // 5 == 玩家菜单
            if (sceneID == 5)
            {
                texturePath = AssetUtility.GetSkillIcon(skillData.Id.ToString(), skillData.ActiveState.ToString());
            }

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
            //inSelectScene = false;

            base.OnHide(isShutdown, userData);

        }

    }
}


