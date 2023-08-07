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
        //public Button combatButton;

        public TextMeshProUGUI planet_percentage = null;

        private DataLandingPoint dataLandingPoint;

        private int landingPointID;

        private int Type;
        private DataPlayer dataPlayer;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
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
            // combatButton.onClick.AddListener(OnCombatButtonClick);

            // need to get study progress
            float percentage = dataPlayer.GetPlayerData().CoursesSaveData[landingPointID];

            planet_percentage.text = UIFloatString.FloatToString(percentage);


        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            exploreButton.onClick.RemoveAllListeners();
            //combatButton.onClick.RemoveAllListeners();
        }

        public void OnCombatButtonClick()
        {
            int PlanetID = 100 + (int) landingPointID / 100;
            string planetType = GameEntry.Data.GetData<DataPlanet>().GetPlanetData(PlanetID).TypeStr;
            switch (planetType)
            {
                case "Cloud Computing":
                    planetType = "CloudComputing";
                    break;
                case "Artificial Intelligence":
                    planetType = "AI";
                    break;
                case "CyberSecurity":
                    planetType = "CyberSecurity";
                    break;
                case "Data Science":
                    planetType = "DataScience";
                    break;
                case "Blockchain":
                    planetType = "Blockchain";
                    break;
                case "Internet of Things":
                    planetType = "IoT";
                    break;
                default:
                    break;
            }
            Debug.Log("Basic Battle Planet Type: " + planetType);
            GameEntry.Event.Fire(this, EnterBattleEventArgs.Create("BasicBattle", planetType));
        }

        public void OnExploreButtonClick()
        {

            dataLandingPoint.currentLandingPointID = landingPointID;

            if(Type == Constant.Type.LP_IN_MAP)
            {
                // move to planet
                MoveToPlanet(landingPointID);
            }
            else if (Type == Constant.Type.LP_IN_PLANET)
            {
                GameEntry.Event.Fire(this, PlanetLandingPointEventArgs.Create());
            }

        }


        private void MoveToPlanet(int landingPointID)
        {
            int planetId = 100 + (int) landingPointID / 100;

            PlanetBase currentPlanet = MapManager.Instance.GetPlanetBaseById(planetId);

            MapManager.Instance.focusedPlanet = currentPlanet.gameObject;
            MapManager.Instance.currentLandingPointID = landingPointID;
            GameEntry.Data.GetData<DataLandingPoint>().currentLandingPointID = landingPointID;
            GameEntry.Data.GetData<DataPlanet>().currentPlanetID = planetId;
            currentPlanet.isFocused = true;
            GameEntry.Event.Fire(this, FocusOnPlanetEventArgs.Create(currentPlanet));
        }
    }
}


