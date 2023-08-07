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
        private PlayerData playerData;

        private void Awake() 
        {
            planetBase = transform.parent.GetComponent<PlanetBase>();    
        }

        private void OnEnable() 
        {
            playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();
            transform.forward = mainCamera.transform.forward;
            transform.rotation = Quaternion.Euler(90, 180, 0);
            planetTitle.text = GameEntry.Data.GetData<DataPlanet>().GetPlanetData(planetBase.PlanetId).Name;
            planetType.text = GameEntry.Data.GetData<DataPlanet>().GetPlanetData(planetBase.PlanetId).TypeStr;
            progressBar.fillAmount = playerData.DomiansSaveData[planetBase.PlanetId];
        }

        private void Update() 
        {
            progressBar.fillAmount = playerData.DomiansSaveData[planetBase.PlanetId];
        }

        private void OnDisable() 
        {
            
        }
    }
}
