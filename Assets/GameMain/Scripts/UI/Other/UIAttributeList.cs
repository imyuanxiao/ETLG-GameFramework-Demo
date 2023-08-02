using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ETLG
{
    public class UIAttributeList : MonoBehaviour
    {
        public GameObject firePower;
        public GameObject fireRate;
        public GameObject durability;
        public GameObject shield;
        public GameObject agility;
        public GameObject energy;

        private float GetBoostPercent(int accuracy)
        {
            return Mathf.Max(0, (float)(accuracy - 50));
        }

        public void DisplayCloudComputingBoost(int accuracy)
        {
            this.firePower.SetActive(true);
            this.energy.SetActive(true);

            this.firePower.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Fire Power +" + GetBoostPercent(accuracy) + "%";
            this.energy.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Energy +" + GetBoostPercent(accuracy) + "%";
        }

        public void DisplayCybersecurityBoost(int accuracy)
        {
            this.shield.SetActive(true);
            this.durability.SetActive(true);

            this.shield.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Shield +" + GetBoostPercent(accuracy) + "%";
            this.durability.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Durability +" + GetBoostPercent(accuracy) + "%";
        }

        public void DisplayAIBoost(int accuracy)
        {
            this.firePower.SetActive(true);
            this.energy.SetActive(true);

            this.firePower.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Fire Power +" + GetBoostPercent(accuracy) + "%";
            this.energy.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Energy +" + GetBoostPercent(accuracy) + "%";
        }

        public void DisplayIoTBoost(int accuracy)
        {
            this.agility.SetActive(true);
            this.durability.SetActive(true);

            this.agility.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Agility +" + GetBoostPercent(accuracy) + "%";
            this.durability.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Durability +" + GetBoostPercent(accuracy) + "%";
        }

        public void DisplayDataScienceBoost(int accuracy)
        {
            this.firePower.SetActive(true);
            this.agility.SetActive(true);

            this.firePower.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Fire Power +" + GetBoostPercent(accuracy) + "%";
            this.agility.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Agility +" + GetBoostPercent(accuracy) + "%";
        }

        public void DisplayBlockchainBoost(int accuracy)
        {
            this.durability.SetActive(true);
            this.shield.SetActive(true);

            this.durability.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Durability +" + GetBoostPercent(accuracy) + "%";
            this.shield.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Shield +" + GetBoostPercent(accuracy) + "%";
        }
    }
}
