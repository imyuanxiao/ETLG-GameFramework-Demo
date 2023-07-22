using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class SkillFX : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed;
        private void Update() 
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}
