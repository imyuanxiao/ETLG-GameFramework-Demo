using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class MapManager : Singleton<MapManager>
    {
        [HideInInspector] public GameObject focusedPlanet;
        [HideInInspector] public int currentLandingPointID = -1;  // -1 means no landing point is being clicked

        public GameObject[] planetsObj;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable() 
        {
            this.focusedPlanet = null;
        }

        public PlanetBase GetPlanetBaseById(int id) 
        {
            foreach (GameObject planet in this.planetsObj)
            {
                if (planet.GetComponent<PlanetBase>().PlanetId == id)
                {
                    return planet.GetComponent<PlanetBase>();
                }
            }
            return null;
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
