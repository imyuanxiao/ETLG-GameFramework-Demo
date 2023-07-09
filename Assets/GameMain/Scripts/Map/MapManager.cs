using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class MapManager : Singleton<MapManager>
    {
        [HideInInspector] public GameObject focusedPlanet;
        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable() 
        {
            this.focusedPlanet = null;
        }

        private void OnDisable() 
        {
            this.focusedPlanet = null;    
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
