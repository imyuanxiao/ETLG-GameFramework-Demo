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

        public RawImage icon;

        public TextMeshProUGUI resName;
        public TextMeshProUGUI hasNum;
        public TextMeshProUGUI needNum;


        public int Type { get; private set; }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetCostResData(int ArtifactId, int hasNum, int needNum)
        {

            int has = hasNum;

            this.hasNum.text = has > needNum ?
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

    }
}


