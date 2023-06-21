using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace ETLG
{
    public class SpaceshipController : MonoBehaviour
    {
        private float horizontalInput;
        private float verticalInput;
        private bool fireInput;

        public float HorizontalInput
        {
            get
            {
                return horizontalInput;
            }
        }

        public float VerticalInput
        {
            get
            {
                return verticalInput;
            }
        }

        public bool FireInput 
        {
            get
            {
                return fireInput;
            }
        }

        private void Update() 
        {
            // Only enable Spaceship movement and attack component in Procedure Battle
            if (GameEntry.Procedure.CurrentProcedure is ProcedureBattle)
            {
                GetComponent<SpaceshipMovement>().enabled = true;
                GetComponent<SpaceshipAttack>().enabled = true;
            }
            else
            {
                GetComponent<SpaceshipMovement>().enabled = false;
                GetComponent<SpaceshipAttack>().enabled = false;
            }
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            fireInput = Input.GetMouseButtonDown(0);
        }
    }
}

