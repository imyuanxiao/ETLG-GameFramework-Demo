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
        public int artifactNum;
        private DataPlayer dataPlayer;
        private DataTrade dataTrade;

        public int Type { get; private set; }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataTrade = GameEntry.Data.GetData<DataTrade>();

            iconButton.onClick.AddListener(OnIconButtonClick);
        }

        private void OnIconButtonClick()
        {
            if (Type != Constant.Type.ARTIFACT_ICON_DEFAULT)
            {
                dataTrade.setArtifactData(CurrentArtifactID, artifactNum, Type);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            iconButton.enabled = true;
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);

            Vector3 offset;
            if (Type == Constant.Type.ARTIFACT_ICON_DEFAULT)
            {
                offset = new Vector3(0f, -110f, 0f);
            }
            else
            {
                offset = new Vector3(420f, -110f, 0f);
            }

            Vector3 newPosition = itemPosition + offset;

            dataArtifact.CurrentArtifactID = CurrentArtifactID;
            dataArtifact.artifactInfoPosition = newPosition;

            if (Type == Constant.Type.ARTIFACT_ICON_DEFAULT)
            {
                GameEntry.Event.Fire(this, ArtifactInfoUIChangeEventArgs.Create(Constant.Type.UI_OPEN));
            }
            else
            {
                if (!dataTrade.clickItemIcon)
                {
                    GameEntry.Event.Fire(this, ArtifactInfoTradeUIChangeEventArgs.Create(Constant.Type.UI_OPEN));
                }

            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Type == Constant.Type.ARTIFACT_ICON_DEFAULT)
            {
                GameEntry.Event.Fire(this, ArtifactInfoUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
            }
            else
            {
                if (!dataTrade.clickItemIcon)
                {
                    GameEntry.Event.Fire(this, ArtifactInfoTradeUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
                }
            }
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetArtifactData(int ArtifactID, int Num, int Type)
        {
            this.CurrentArtifactID = ArtifactID;

            this.Type = Type;

            this.artifactNumber.text = Num.ToString();
            this.artifactNum = Num;

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
            HideAllItem();

        }

    }
}


