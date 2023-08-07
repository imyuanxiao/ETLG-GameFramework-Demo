using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;
using UnityEngine.UI;
using ETLG.Data;

namespace ETLG
{
    public class PlanetBase : MonoBehaviour
    {
        public bool isFocused;
        public int PlanetId;
        public Transform focusPoint;
        public GameObject UIPlanetTag;
        public GameObject[] landingPoints;
        public Image progressBar;
        private DataLearningProgress dataLearningProgress;
        private DataPlayer dataPlayer;

        private void OnEnable() 
        {
            GameEntry.Event.Subscribe(FocusOnPlanetEventArgs.EventId, OnFocused);
            GameEntry.Event.Subscribe(UnFocusOnPlanetEventArgs.EventId, OnUnFocused);
            dataLearningProgress = GameEntry.Data.GetData<DataLearningProgress>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            isFocused = false;
            ActiveLandingPoint(false);
            GetComponent<DragRotate>().enabled = false;
            updateProgress();
        }

        private void OnUnFocused(object sender, GameEventArgs e)
        {
            UnFocusOnPlanetEventArgs ne = (UnFocusOnPlanetEventArgs) e;
            if (ne == null)
            {
                Log.Error("Invalid Event [UnFocusOnPlanetEventArgs]");
            }
            if (ne.PlanetBase.PlanetId == this.PlanetId)
            {
                ActiveLandingPoint(false);
            }
            if (UIPlanetTag != null)
                UIPlanetTag.SetActive(true);
        }

        private void OnFocused(object sender, GameEventArgs e)
        {
            FocusOnPlanetEventArgs ne = (FocusOnPlanetEventArgs) e;
            if (ne == null)
            {
                Log.Error("Invalid Event [FocusOnPlanetEventArgs]");
            }
            if (ne.PlanetBase.PlanetId == this.PlanetId)
            {
                ActiveLandingPoint(true);
            }
            if (UIPlanetTag != null)
                UIPlanetTag.SetActive(false);
        }

        private void ActiveLandingPoint(bool state)
        {
            foreach (var landingPoint in landingPoints)
            {
                landingPoint.SetActive(state);
            }
        }

        private void updateProgress()
        {
            float progress = dataPlayer.GetPlayerData().DomiansSaveData[PlanetId];
            string progressString = UIFloatString.FloatToString(progress);
            //RectTransform progressBarRectTransform = progressBar.GetComponentInChildren<RectTransform>();
            //float targetWidth = 5f * progress;
            //progressBarRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        }

        private void OnDisable() 
        {
            isFocused = false;
            ActiveLandingPoint(false);
            GetComponent<DragRotate>().enabled = false;
            GameEntry.Event.Unsubscribe(FocusOnPlanetEventArgs.EventId, OnFocused);
            GameEntry.Event.Unsubscribe(UnFocusOnPlanetEventArgs.EventId, OnUnFocused);
        }
    }
}
