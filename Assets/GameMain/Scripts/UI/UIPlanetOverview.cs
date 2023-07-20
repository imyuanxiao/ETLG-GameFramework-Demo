using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ETLG.Data;
using UnityGameFramework.Runtime;
using System;

namespace ETLG
{
    public class UIPlanetOverview : UGuiFormEx
    {
        public Button challengeButton;
        public Button exploreButton;
        public Button cancelButton;

        public TextMeshProUGUI planetName;
        public TextMeshProUGUI planetType;

        public TextMeshProUGUI Desription;

        public Image progressBar;

        private PlanetBase currentPlanet = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            exploreButton.onClick.AddListener(OnExploreButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
            challengeButton.onClick.AddListener(OnChallengeButtonClicked);
        }

        private void OnChallengeButtonClicked()
        {
            string planetType = GameEntry.Data.GetData<DataPlanet>().GetPlanetData(currentPlanet.PlanetId).TypeStr;
            Debug.Log(planetType);
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
            GameEntry.Event.Fire(this, EnterBattleEventArgs.Create("IntermidateBattle", planetType));
        }

        private void OnExploreButtonClicked()
        {
            // int enterBasicBattleProbablity = 3;
            // determine if entering basic battle
            // int r = UnityEngine.Random.Range(0, 10);
            // if (r < enterBasicBattleProbablity)
            // {
            //     // Enter Basic Battle
            //     GameEntry.Event.Fire(this, EnterBattleEventArgs.Create("BasicBattle", ""));
            //     this.Close();
            //     return;
            // }

            MapManager.Instance.focusedPlanet = currentPlanet.gameObject;
            GameEntry.Data.GetData<DataPlanet>().currentPlanetID = currentPlanet.PlanetId;
            currentPlanet.isFocused = true;
            GameEntry.Event.Fire(this, FocusOnPlanetEventArgs.Create(currentPlanet));
            this.Close();
        }

        private void OnCancelButtonClicked()
        {
            this.Close();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            currentPlanet = (PlanetBase) userData;

            if (currentPlanet == null)
            {
                Log.Error("Invalid PlanetBase");
            }
            
            PlanetData data = GameEntry.Data.GetData<DataPlanet>().GetPlanetData(currentPlanet.PlanetId);

            planetName.text = data.Name;
            planetType.text = data.TypeStr;
            Desription.text = data.Description;
            progressBar.fillAmount = 0.4f;  // change this to the progress of the planet
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
