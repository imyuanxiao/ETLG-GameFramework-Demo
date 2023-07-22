using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class BasicEnemyMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        private float horizontalDirection;
        private Rigidbody rb;
        private float changeDirectionTime;
        private float elapsedTime;

        private void Awake() 
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start() 
        {
            horizontalDirection = 1;
            // rb.velocity = -Vector3.forward * speed;
            rb.velocity = new Vector3(horizontalDirection * 0.5f, -Vector3.forward.y, -Vector3.forward.z) * speed;
        }

        private void OnEnable() 
        {
            elapsedTime = Random.Range(0f, 0.9f);
            changeDirectionTime = 2.5f;
        }

        private void Update() 
        {
            if (elapsedTime < changeDirectionTime)
            {
                elapsedTime += Time.deltaTime;
            }
            else 
            {
                horizontalDirection = -horizontalDirection;
                rb.velocity = new Vector3(horizontalDirection * 0.5f, -Vector3.forward.y, -Vector3.forward.z) * speed;
                elapsedTime = 0f;
            }
        }
    }
}
