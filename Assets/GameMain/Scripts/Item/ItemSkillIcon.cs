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
    public class ItemSkillIcon : ItemLogicEx, IPointerEnterHandler, IPointerExitHandler
    {

        private DataSkill dataSkill;

        private PlayerSkillData playerSkillData;

        public RawImage skillIcon;

        public Button iconButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataSkill = GameEntry.Data.GetData<DataSkill>();

            iconButton.onClick.AddListener(OnIconButtonClick);

            EventTrigger trigger = iconButton.gameObject.AddComponent<EventTrigger>();

        }

        private void OnIconButtonClick()
        {
            // TODO 显示当前技能所需的升级资源和确认按钮

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 获得挂载对象的位置
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 newPosition = itemPosition + new Vector3(100f, 0f, 0f);

            dataSkill.currentPlayerSkillData = this.playerSkillData;

            dataSkill.skillInfoPosition = newPosition;

            // 显示skill info ui 的事件，传入UI应该显示的位置
            GameEntry.Event.Fire(this, SkillInfoOpenEventArgs.Create());

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEntry.Event.Fire(this, SkillInfoCloseEventArgs.Create());
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetSkillData(PlayerSkillData playerSkillData)
        {
            this.playerSkillData = playerSkillData;

            string texturePath = AssetUtility.GetSkillIcon(playerSkillData.Id.ToString(), playerSkillData.ActiveState.ToString());
            Texture texture = Resources.Load<Texture>(texturePath);
            if (texture != null)
            {
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


