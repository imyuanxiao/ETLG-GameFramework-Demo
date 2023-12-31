﻿using ETLG.Data;
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
    public class UIModuleEquipForm : UGuiFormEx
    {

        public DataArtifact dataArtifact;
        public DataPlayer dataPlayer;

        public Transform UIContainer;
        public TextMeshProUGUI Title = null;
        public Button CancelButton;
        public Button EquipButton;
        public bool refresh;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            CancelButton.onClick.AddListener(OnCancelButtonClick);
            EquipButton.onClick.AddListener(OnEquipButtonClick);


        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            refresh = true;

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);


        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (refresh)
            {
                showContent();
                refresh = false;
            }
        }

        public void showContent()
        {
            UIContainer.position = dataArtifact.artifactInfoPosition;
        }

        public void OnCancelButtonClick()
        {
            dataArtifact.lockCurrentModuleID = false;
            GameEntry.Event.Fire(this, ModuleEquipUIchangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }

        public void OnEquipButtonClick()
        {
            dataPlayer.GetPlayerData().EquipCurrentModule();
        }

    }
}


