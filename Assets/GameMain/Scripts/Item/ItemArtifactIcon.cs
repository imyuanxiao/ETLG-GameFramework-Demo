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
    public class ItemArtifactIcon : ItemLogicEx, IPointerEnterHandler, IPointerExitHandler
    {

        public DataArtifact dataArtifact;

        private PlayerArtifactData playerArtifact;

        public RawImage artifactIcon;

        public Button iconButton;

        public TextMeshProUGUI artifactNumber;

        public int Type { get; private set; }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();

            iconButton.onClick.AddListener(OnIconButtonClick);

            EventTrigger trigger = iconButton.gameObject.AddComponent<EventTrigger>();

        }

        private void OnIconButtonClick()
        {

            if (Type == Constant.Type.TRADE_PLAYER_NPC)
            {
                Log.Debug("trade Player to NPC");
            }

            if (Type == Constant.Type.TRADE_NPC_PLAYER)
            {
                Log.Debug("trade NPC to Player");
            }


        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 获得挂载对象的位置
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);

            // artifactInfo new position
            Vector3 offset = new Vector3(-10f, 0f, 0f);

            if(Type == Constant.Type.TRADE_NPC_PLAYER)
            {
                offset = new Vector3(400f, -130f, 0f);
            }

            Vector3 newPosition = itemPosition + offset;

            dataArtifact.currentPlayerArtifactData = this.playerArtifact;
            dataArtifact.artifactInfoPosition = newPosition;

            GameEntry.Event.Fire(this, ArtifactInfoOpenEventArgs.Create());

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEntry.Event.Fire(this, ArtifactInfoCloseEventArgs.Create());
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetArtifactData(PlayerArtifactData playerArtifact, int Type)
        {
            this.playerArtifact = playerArtifact;

            this.Type = Type;

            this.artifactNumber.text = playerArtifact.Number.ToString();

            string texturePath = AssetUtility.GetArtifactIcon(playerArtifact.Id.ToString());

            Texture texture = Resources.Load<Texture>(texturePath);

            if (texture == null)
            {
                texturePath = AssetUtility.GetArtifactIcon(Constant.Type.ICON_LOST);
                texture = Resources.Load<Texture>(texturePath);
            }

            artifactIcon.texture = texture;

        }


        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

        }

    }
}


