using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemRewardPreview : ItemLogicEx
    {
        private DataArtifact dataArtifact;
        private DataSkill dataSkill;

        public RawImage icon;

        public TextMeshProUGUI ShowName;
        public TextMeshProUGUI ShowNum;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataSkill = GameEntry.Data.GetData<DataSkill>();

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetRewardData(int Id, int Num, int Type)
        {
            string ShowNameText = "";
            string texturePath = "";

            if (Type == Constant.Type.REWARD_TYPE_ARTIFACT)
            {
                ShowNameText = dataArtifact.GetArtifactData(Id).Name;
                texturePath = AssetUtility.GetArtifactIcon(Id.ToString());
            }
            if (Type == Constant.Type.REWARD_TYPE_SKILL)
            {
                ShowNameText = dataSkill.GetSkillData(Id).Name;
                texturePath = AssetUtility.GetSkillIcon(Id.ToString());
            }
            texturePath = AssetUtility.GetArtifactIcon(Id.ToString());


            ShowName.text = ShowNameText;
            ShowNum.text = Num.ToString();

            Texture texture = Resources.Load<Texture>(texturePath);

            if (texture == null)
            {
                texturePath = AssetUtility.GetIconMissing();
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


