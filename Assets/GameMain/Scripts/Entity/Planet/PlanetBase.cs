using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace ETLG
{
    public class PlanetBase : MonoBehaviour
    {
        public bool isFocused;
        public int PlanetId;
        public Transform focusPoint;
        public GameObject[] landingPoints;

        private void OnEnable() 
        {
            GameEntry.Event.Subscribe(FocusOnPlanetEventArgs.EventId, OnFocused);
            GameEntry.Event.Subscribe(UnFocusOnPlanetEventArgs.EventId, OnUnFocused);

            isFocused = false;
            ActiveLandingPoint(false);
            GetComponent<DragRotate>().enabled = false;
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
        }

        private void ActiveLandingPoint(bool state)
        {
            foreach (var landingPoint in landingPoints)
            {
                landingPoint.SetActive(state);
            }
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
