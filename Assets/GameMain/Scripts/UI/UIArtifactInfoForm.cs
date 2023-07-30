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

        public RectTransform UIContainer;

        public TextMeshProUGUI ArtifactName = null;
        public TextMeshProUGUI ArtifactType = null;
        public TextMeshProUGUI ArtifactTradeable = null;
        public TextMeshProUGUI ArtifactValue = null;
        public TextMeshProUGUI ArtifactNumber = null;
        public TextMeshProUGUI ArtifactDescription = null;

        private bool refresh;


        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (refresh)
            {
                showContent();
                LayoutRebuilder.ForceRebuildLayoutImmediate(UIContainer);
                refresh = false;
            }
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            refresh = true;

        }

        private void showContent()
        {
            ArtifactDataBase artifactDataBase = dataArtifact.GetCurrentShowArtifactData();
            UIContainer.position = dataArtifact.artifactInfoPosition;
            ArtifactName.text = artifactDataBase.Name;
            ArtifactType.text = GameEntry.Localization.GetString(Constant.Type.ARTIFACT_TYPE + artifactDataBase.Type);
            ArtifactTradeable.text = artifactDataBase.Tradeable ? "Tradeable" : "Untradeable";
            ArtifactValue.text = artifactDataBase.Value.ToString();
            ArtifactNumber.text = dataPlayer.GetPlayerData().GetArtifactNumById(artifactDataBase.Id).ToString();
            ArtifactDescription.text = artifactDataBase.Description;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            refresh = false;
            base.OnClose(isShutdown, userData);

        }
    }
}


