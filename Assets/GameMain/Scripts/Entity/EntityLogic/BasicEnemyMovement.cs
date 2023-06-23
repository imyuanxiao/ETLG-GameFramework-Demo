using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class BasicEnemyMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Rigidbody rb;

        private void Awake() 
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start() 
        {
            rb.velocity = -Vector3.forward * speed;
        }

        private void Update() {
            
        }
    }
}
