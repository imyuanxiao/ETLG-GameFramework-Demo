using ETLG.Data;
using GameFramework.Event;
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
    public class UIArtifactInfoForm : UGuiFormEx
    {

        public DataArtifact dataArtifact;
        public DataPlayer dataPlayer;

        private ArtifactDataBase artifactDataBase;


        public Transform UIContainer;

        public TextMeshProUGUI ArtifactName = null;
        public TextMeshProUGUI ArtifactType = null;
        public TextMeshProUGUI ArtifactTradeable = null;
        public TextMeshProUGUI ArtifactValue = null;
        public TextMeshProUGUI ArtifactNumber = null;
        public TextMeshProUGUI ArtifactDescription = null;

        private object userData;



        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            this.userData = userData;

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

        }

        protected override void OnOpen(object userData)
        {

            artifactDataBase = dataArtifact.GetCurrentShowArtifactData();

            UIContainer.position = dataArtifact.artifactInfoPosition;

            ArtifactName.text = artifactDataBase.NameID;
            ArtifactType.text = GameEntry.Localization.GetString(Constant.Type.ARTIFACT_TYPE + artifactDataBase.Type);
            ArtifactTradeable.text = artifactDataBase.Tradeable ? "Tradeable" : "Untradeable";
            ArtifactValue.text = artifactDataBase.Value.ToString();

            ArtifactNumber.text = dataPlayer.GetPlayerData().GetArtifactNumById(artifactDataBase.Id).ToString();
            ArtifactDescription.text = artifactDataBase.Description;


            base.OnOpen(userData);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            artifactDataBase = null;
            base.OnClose(isShutdown, userData);

        }
    }
}


