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
        public TextMeshProUGUI overview;
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
            string planetType = GameEntry.Data.GetData<DataPlanet>().GetPlanetData(currentPlanet.PlanetId).Type;
            GameEntry.Event.Fire(this, EnterBattleEventArgs.Create("IntermidateBattle", planetType));
        }

        private void OnExploreButtonClicked()
        {
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
            planetType.text = data.Type;
            overview.text = data.Overview;
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
