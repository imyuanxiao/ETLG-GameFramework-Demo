using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    // public enum PLANET_TYPE {NULL, AI, CLOUD_COMPUTING, DATA_SCIENCE, IOT, CYBERSECURITY, BLOCKCHAIN}

    public class PlanetBase : MonoBehaviour
    {
        public bool isFocused;
        // public PLANET_TYPE planetType;
        public int PlanetId;
        public Transform focusPoint;

        private void OnEnable() 
        {
            isFocused = false;    
        }

        private void OnDisable() 
        {
            isFocused = false;    
        }
    }
}
