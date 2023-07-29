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
    public class ItemModuleIcon : ItemLogicEx, IPointerEnterHandler, IPointerExitHandler
    {

        public DataArtifact dataArtifact;

        public int CurrentModuleID;

        public RawImage artifactIcon;

        public Button iconButton;

        private bool equipped;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();

            iconButton.onClick.AddListener(OnIconButtonClick);
            equipped = false;
        }

        private void OnIconButtonClick()
        {
            if (equipped) return;
            dataArtifact.lockCurrentModuleID = true;
            GameEntry.Event.Fire(this, ModuleEquipUIchangeEventArgs.Create(Constant.Type.UI_OPEN));

        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 offset = new Vector3(-10f, 0f, 0f);
            Vector3 newPosition = itemPosition + offset;

            dataArtifact.SetCurrentModuleID(CurrentModuleID);
            dataArtifact.artifactInfoPosition = newPosition;

            GameEntry.Event.Fire(this, ModuleInfoUIChangeEventArgs.Create(Constant.Type.UI_OPEN));

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEntry.Event.Fire(this, ModuleInfoUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetModuleData(int ModuleID, bool equipped)
        {
            this.CurrentModuleID = ModuleID;
            this.equipped = equipped;

            string texturePath = AssetUtility.GetArtifactIcon(ModuleID.ToString());

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


