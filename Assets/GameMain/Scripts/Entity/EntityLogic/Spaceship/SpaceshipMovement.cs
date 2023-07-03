using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;


namespace ETLG
{
    public class SpaceshipMovement : MonoBehaviour
    {
        private SpaceshipController controller;
        private Rigidbody rb;
        private EntitySpaceshipSelect entitySpaceshipSelect;
        private EntitySpaceship entitySpaceship;
        private float speed;

        private void Awake() 
        {
            controller = GetComponent<SpaceshipController>();
            rb = GetComponent<Rigidbody>();
            entitySpaceshipSelect = GetComponent<EntitySpaceshipSelect>();
            entitySpaceship = GetComponent<EntitySpaceship>();
        }

        private void FixedUpdate() 
        {
            // float speed = entitySpaceship.EntityDataSpaceship.SpaceshipData.Agility;
            float speed = entitySpaceship.EntityDataSpaceship.PlayerData.playerCalculatedSpaceshipData.Agility;
            rb.velocity = new Vector3(controller.HorizontalInput, 0, controller.VerticalInput).normalized * speed;
        }
    }
}

