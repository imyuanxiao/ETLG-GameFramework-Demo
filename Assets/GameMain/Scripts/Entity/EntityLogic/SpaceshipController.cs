using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Event;
using System;

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

        private void Awake() 
        {
            GameEntry.Event.Subscribe(ActiveBattleComponentEventArgs.EventId, OnActiveBattleComponent);
            GameEntry.Event.Subscribe(DeactiveBattleComponentEventArgs.EventId, OnDeactiveBattleComponent);
        }

        private void OnDeactiveBattleComponent(object sender, GameEventArgs e)
        {
            DeactiveBattleComponentEventArgs ne = (DeactiveBattleComponentEventArgs) e;

            GetComponent<SpaceshipMovement>().enabled = false;
            GetComponent<SpaceshipAttack>().enabled = false;
            GetComponent<PlayerHealth>().enabled = false;
        }

        private void OnActiveBattleComponent(object sender, GameEventArgs e)
        {
            ActiveBattleComponentEventArgs ne = (ActiveBattleComponentEventArgs) e;

            GetComponent<SpaceshipMovement>().enabled = true;
            GetComponent<SpaceshipAttack>().enabled = true;
            GetComponent<PlayerHealth>().enabled = true;
        }

        private void Update() 
        {            
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            fireInput = Input.GetMouseButtonDown(0);
        }
    }
}

