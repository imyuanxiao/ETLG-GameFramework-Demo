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
        public Button combatButton;

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
            combatButton.onClick.AddListener(OnCombatButtonClick);



        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            exploreButton.onClick.RemoveAllListeners();
        }

        public void OnCombatButtonClick()
        {

        }

        public void OnExploreButtonClick()
        {

            dataLandingPoint.currentLandingPointID = landingPointID;

            if(Type == Constant.Type.LP_IN_MAP)
            {
                // change false to true for final product
                if(EnterRandomBattle(false)) { return; }

                // move to planet
                MoveToPlanet(landingPointID);
            }
            else if (Type == Constant.Type.LP_IN_PLANET)
            {
                GameEntry.Event.Fire(this, PlanetLandingPointEventArgs.Create());
            }

        }

        private bool EnterRandomBattle(bool isActive)
        {
            if (!isActive) { return false; }

            // calculate if enter random battle     
            int enterBasicBattleProbablity = 3;
            int r = UnityEngine.Random.Range(0, 10);
            if (r < enterBasicBattleProbablity)
            {
                // Enter Basic Battle
                GameEntry.Event.Fire(this, EnterBattleEventArgs.Create("BasicBattle", ""));
                return true;
            }
            return false;
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


