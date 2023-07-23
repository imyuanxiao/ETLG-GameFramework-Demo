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
    public class ItemCostResBar : ItemLogicEx
    {
        private DataArtifact dataArtifact;

        public RawImage icon;

        public TextMeshProUGUI ArtifactName;
        public TextMeshProUGUI hasNum;
        public TextMeshProUGUI needNum;

        public int ArtifactId;

        public int Type { get; private set; }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetCostResData(int ArtifactId, int hasNum, int needNum)
        {
            this.ArtifactId = ArtifactId;

            ArtifactName.text = dataArtifact.GetArtifactData(ArtifactId).Name;

            int has = hasNum;

            this.hasNum.text = has >= needNum ?
                "<color=yellow>" + has + " </color>" :
                 "<color=red>" + has + " </color>";

            this.needNum.text = needNum.ToString();

            string texturePath = AssetUtility.GetArtifactIcon(ArtifactId.ToString());

            Texture texture = Resources.Load<Texture>(texturePath);

            if (texture == null)
            {
                texturePath = AssetUtility.GetArtifactIcon(Constant.Type.ICON_LOST);
                texture = Resources.Load<Texture>(texturePath);
            }

            icon.texture = texture;

        }


        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

        }

  /*      public void OnPointerEnter(PointerEventData eventData)
        {
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 newPosition = itemPosition + new Vector3(0f, 10f, 0f);

            

            string tipTitle = dataArtifact.GetArtifactData(ArtifactId).Name;

            GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(newPosition, tipTitle, Constant.Type.UI_OPEN));

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }*/


    }
}


