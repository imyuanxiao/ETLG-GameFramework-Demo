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
    public class ItemArtifactIcon : ItemLogicEx
    {

        public DataArtifact dataArtifact;

        private PlayerArtifactData playerArtifact;

        public RawImage artifactIcon;

        public Button iconButton;

        public TextMeshProUGUI artifactNumber;

        //public bool isEmpty;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();

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
            Vector3 newPosition = itemPosition + new Vector3(-50f, 0f, 0f);

            dataArtifact.currentPlayerArtifactData = this.playerArtifact;
            dataArtifact.artifactInfoPosition = newPosition;

            GameEntry.Event.Fire(this, ArtifactInfoOpenEventArgs.Create());

        }

        private void OnIconPointerExit()
        {

            GameEntry.Event.Fire(this, ArtifactInfoCloseEventArgs.Create());

        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetArtifactData(PlayerArtifactData playerArtifact)
        {
            this.playerArtifact = playerArtifact;

            this.artifactNumber.text = playerArtifact.Number.ToString();

            string texturePath = AssetUtility.GetArtifactIcon(playerArtifact.Id.ToString());

            Texture texture = Resources.Load<Texture>(texturePath);

            if (texture == null)
            {
                texturePath = AssetUtility.GetArtifactIcon("iconLost");
                texture = Resources.Load<Texture>(texturePath);
            }

            if (texture != null)
            {
                artifactIcon.texture = texture;
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


