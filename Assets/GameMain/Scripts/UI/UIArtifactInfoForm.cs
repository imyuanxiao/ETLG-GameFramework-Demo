using ETLG.Data;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIArtifactInfoForm : UGuiFormEx
    {

        public DataArtifact dataArtifact;
        private ArtifactDataBase artifactDataBase;

        public PlayerArtifactData playerArtifactData;

        public Transform UIContainer;

        //public RawImage ArtifactIcon;
        public TextMeshProUGUI ArtifactName = null;
        public TextMeshProUGUI ArtifactType = null;
        public TextMeshProUGUI ArtifactTradeable = null;
        public TextMeshProUGUI ArtifactValue = null;
        public TextMeshProUGUI ArtifactNumber = null;
        public TextMeshProUGUI ArtifactDescription = null;



        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();

        }

        protected override void OnOpen(object userData)
        {

            artifactDataBase = dataArtifact.GetCurrentShowArtifactData();

            playerArtifactData = dataArtifact.currentPlayerArtifactData;

            UIContainer.position = dataArtifact.artifactInfoPosition;

            ArtifactName.text = artifactDataBase.Name;
            ArtifactType.text = GameEntry.Localization.GetString(Constant.Type.ARTIFACT_TYPE + artifactDataBase.Type);
            ArtifactTradeable.text = artifactDataBase.Tradeable ? "Tradeable" : "Untradeable";
            ArtifactValue.text = artifactDataBase.Value.ToString();

            ArtifactNumber.text = playerArtifactData.Number.ToString();
            ArtifactDescription.text = artifactDataBase.Name + "To be added";

            /*          Texture texture = Resources.Load<Texture>(AssetUtility.GetArtifactIcon(artifactDataBase.Id.ToString()));
                      if (texture != null)
                      {
                          ArtifactIcon.texture = texture;
                      }*/

            base.OnOpen(userData);


        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            artifactDataBase = null;
            playerArtifactData = null;
            base.OnClose(isShutdown, userData);

        }
 

    }
}


