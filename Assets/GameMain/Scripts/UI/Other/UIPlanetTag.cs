using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ETLG.Data;

namespace ETLG
{
    public class UIPlanetTag : MonoBehaviour
    {
        public Camera mainCamera;
        public TextMeshProUGUI planetTitle;
        public TextMeshProUGUI planetType;
        public Image progressBar;
        private PlanetBase planetBase;

        private void Awake() 
        {
            planetBase = transform.parent.GetComponent<PlanetBase>();    
        }

        private void OnEnable() 
        {
            transform.forward = mainCamera.transform.forward;
            transform.rotation = Quaternion.Euler(90, 180, 0);
            planetTitle.text = GameEntry.Data.GetData<DataPlanet>().GetPlanetData(planetBase.PlanetId).Name;
            planetType.text = GameEntry.Data.GetData<DataPlanet>().GetPlanetData(planetBase.PlanetId).TypeStr;
            progressBar.fillAmount = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().DomiansSaveData[planetBase.PlanetId];  // change this to the progress of the planet
            Debug.Log("Progress Bar: " + planetBase.PlanetId + " | " + progressBar.fillAmount);
        }

        private void Update() 
        {
            
        }

        private void OnDisable() 
        {
            
        }
    }
}
