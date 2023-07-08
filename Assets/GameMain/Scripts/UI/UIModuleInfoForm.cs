using ETLG.Data;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIModuleInfoForm : UGuiFormEx
    {

        public DataArtifact dataArtifact;

        public DataPlayer dataPlayer;

        public Transform UIContainer;

        public RawImage moduleIcon;
        public TextMeshProUGUI ModuleName = null;
        public TextMeshProUGUI ModuleType = null;
        public TextMeshProUGUI ModuleDescription;

        public TextMeshProUGUI ModuleEffect;

        public bool refresh;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();


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

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            refresh = true;
        }

        public void showContent()
        {

            ArtifactModuleData moduleData = dataArtifact.GetCurrentShowModuleData();

            UIContainer.position = new Vector3(50f, 120f, 0f);

            ModuleName.text = moduleData.Name;
            ModuleType.text = moduleData.ClassificationName();
            ModuleDescription.text = moduleData.Description;

            ModuleEffect.text = moduleData.GetEffect();

            // set skill icon            
            string texturePath = AssetUtility.GetSkillIcon(moduleData.Id.ToString());
            Texture texture = Resources.Load<Texture>(texturePath);
            if (texture == null)
            {
                texturePath = AssetUtility.GetIconMissing();
                texture = Resources.Load<Texture>(texturePath);
            }
            moduleIcon.texture = texture;


        }

        protected override void OnClose(bool isShutdown, object userData)
        {

            base.OnClose(isShutdown, userData);
        }


    }
}


