using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemLandingPointSelect : ItemLogicEx
    {

        public TextMeshProUGUI landingpoint_title = null;
        public Button exploreButton;

        private DataLandingPoint dataLandingPoint;

        private int landingPointID;

        private int Type;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataLandingPoint = GameEntry.Data.GetData<DataLandingPoint>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetData(int landingPointID, int Type)
        {
            this.landingPointID = landingPointID;

            this.Type = Type; 

            landingpoint_title.text = dataLandingPoint.GetLandingPointData(landingPointID).Title;

            exploreButton.onClick.AddListener(OnExploreButtonClick);



        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            exploreButton.onClick.RemoveAllListeners();
        }

        public void OnExploreButtonClick()
        {

            dataLandingPoint.currentLandingPointID = landingPointID;

            if(Type == Constant.Type.LP_IN_MAP)
            {
                // move to planet
            }
            else if (Type == Constant.Type.LP_IN_PLANET)
            {
                GameEntry.Event.Fire(this, PlanetLandingPointEventArgs.Create());
            }

        }


    }
}


