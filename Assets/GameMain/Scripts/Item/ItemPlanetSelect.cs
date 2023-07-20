using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemPlanetSelect : ItemLogicEx
    {

        public Button expandButton;
        public RawImage expandButtonIcon;

        public TextMeshProUGUI planet_name = null;
        public TextMeshProUGUI planet_domain = null;

        public TextMeshProUGUI planet_percentage = null;

        public GameObject planet_valueBar = null;
        
        public RectTransform LandingPointsContainer;

        private DataPlanet dataPlanet;

        private int PlanetID;

        private bool Expanded;

        private readonly float valueBarMaxWidth = 440f;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataPlanet = GameEntry.Data.GetData<DataPlanet>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetData(int PlanetID)
        {
            this.PlanetID = PlanetID;

            expandButton.onClick.AddListener(OnExpandButtonClick);

            planet_name.text = dataPlanet.GetPlanetData(PlanetID).Name;
            planet_domain.text = dataPlanet.GetPlanetData(PlanetID).TypeStr;

            // need to get study progress
            float percentage = 33.45f;

            planet_percentage.text = (int)percentage + "%";

            SetWidth(planet_valueBar, percentage/100);

            SetArrowIcon(Constant.Type.ARROW_RIGHT);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            expandButton.onClick.RemoveAllListeners();
        }

        public void OnExpandButtonClick()
        {
            if (!Expanded)
            {
                GameEntry.Data.GetData<DataPlanet>().currentPlanetID = PlanetID;
                SetArrowIcon(Constant.Type.ARROW_DOWN);
                ShowLandingPoints();
            }
            else
            {
                SetArrowIcon(Constant.Type.ARROW_RIGHT);
                HideAllItem();
            }
            Expanded = !Expanded;
            GameEntry.Event.Fire(this, PlanetExpandedEventArgs.Create());

        }

        private void SetArrowIcon(int Type)
        {
            string texturePath = AssetUtility.GetArrowImg(Type);
            Texture texture = Resources.Load<Texture>(texturePath);
            expandButtonIcon.texture = texture;
        }

        private void ShowLandingPoints()
        {

            int[] LandingPoints = GameEntry.Data.GetData<DataPlanet>().GetCurrentPlanetData().LandingPoints;

            foreach (var LandingPoint in LandingPoints)
            {
                ShowItem<ItemLandingPointSelect>(EnumItem.ItemLandingPointSelect, (item) =>
                {
                    item.transform.SetParent(LandingPointsContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.GetComponent<ItemLandingPointSelect>().SetData(LandingPoint, Constant.Type.LP_IN_MAP);
                });
            }
        }

        public void SetWidth(GameObject targetObject, float percentage)
        {
            float newWidth = percentage * valueBarMaxWidth;

            RectTransform rectTransform = targetObject.GetComponent<RectTransform>();

            Vector2 newSizeDelta = rectTransform.sizeDelta;
            newSizeDelta.x = newWidth;
            rectTransform.sizeDelta = newSizeDelta;
        }

    }
}


