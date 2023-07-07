using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class FinalBossSkill : MonoBehaviour
    {
        [HideInInspector] public int stage = 1;
        private bool isStag1Active = false;
        private bool isStag2Active = false;

        private void OnEnable() 
        {
            this.stage = 1; 
            this.isStag1Active = false;
            this.isStag2Active = false;

            SetSecondStageSkill(false);
            SetFirstStageSkill(true);
        }

        private void SetFirstStageSkill(bool state)
        {
            GetComponent<ShieldProtecting>().enabled = state;
            GetComponent<CriticalAttack>().enabled = state;  
            this.isStag1Active = state; 
        }

        private void SetSecondStageSkill(bool state) 
        {
            GetComponent<AIAssist>().enabled = state;
            GetComponent<CriticalAttack>().enabled = state;
            GetComponent<InstantMovement>().enabled = state;
            GetComponent<ShieldProtecting>().enabled = !state;
            this.isStag2Active = state;
        }

        private void Update() 
        {
            if (stage == 2 && !isStag2Active)
            {
                SetSecondStageSkill(true);
            }
        }

        private void OnDisable() 
        {
            this.stage = 1; 
            SetFirstStageSkill(false);
            SetSecondStageSkill(false);   
        }
    }
}
