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

        private void OnEnable() 
        {
            GameEntry.Event.Subscribe(ActiveBattleComponentEventArgs.EventId, OnActiveBattleComponent);
            GameEntry.Event.Subscribe(DeactiveBattleComponentEventArgs.EventId, OnDeactiveBattleComponent);
            GameEntry.Event.Subscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Subscribe(BasicBattleWinEventArgs.EventId, OnBasicBattleWin);
        }

        private void OnBasicBattleWin(object sender, GameEventArgs e)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameEntry.Event.Fire(this, DeactiveBattleComponentEventArgs.Create());
        }

        private void OnPlayerDead(object sender, GameEventArgs e)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameEntry.Event.Fire(this, DeactiveBattleComponentEventArgs.Create());
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

        private void OnDisable() 
        {
            GameEntry.Event.Unsubscribe(ActiveBattleComponentEventArgs.EventId, OnActiveBattleComponent);
            GameEntry.Event.Unsubscribe(DeactiveBattleComponentEventArgs.EventId, OnDeactiveBattleComponent);
            GameEntry.Event.Unsubscribe(PlayerDeadEventArgs.EventId, OnPlayerDead);
            GameEntry.Event.Unsubscribe(BasicBattleWinEventArgs.EventId, OnBasicBattleWin);
        }
    }
}

