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

        public int CurrentArtifactID;


        public RawImage artifactIcon;

        public Button iconButton;

        public TextMeshProUGUI artifactNumber;

        public int Type { get; private set; }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();

            iconButton.onClick.AddListener(OnIconButtonClick);

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
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);

            Vector3 offset = new Vector3(-10f, 0f, 0f);

            if(Type == Constant.Type.TRADE_NPC_PLAYER)
            {
                offset = new Vector3(400f, -130f, 0f);
            }

            Vector3 newPosition = itemPosition + offset;

            dataArtifact.CurrentArtifactID = CurrentArtifactID;
            dataArtifact.artifactInfoPosition = newPosition;

            GameEntry.Event.Fire(this, ArtifactInfoUIChangeEventArgs.Create(Constant.Type.UI_OPEN));

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEntry.Event.Fire(this, ArtifactInfoUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetArtifactData(int ArtifactID, int NUm, int Type)
        {
            this.CurrentArtifactID = ArtifactID;

            this.Type = Type;

            this.artifactNumber.text = NUm.ToString();

            string texturePath = AssetUtility.GetArtifactIcon(ArtifactID.ToString());

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


