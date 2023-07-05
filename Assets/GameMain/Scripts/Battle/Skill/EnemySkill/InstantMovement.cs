using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class InstantMovement : MonoBehaviour
    {
        public float coolDown = 3f;
        private float timeElapsed = 0f;

        private void OnEnable() 
        {
            timeElapsed = 0f;
        }

        private void Update() 
        {
            if (timeElapsed < coolDown)
            {
                timeElapsed += Time.deltaTime;
            }
            else 
            {
                GameEntry.Event.Fire(this, EnemyInstantMoveEventArgs.Create());
                timeElapsed = 0;
            }
        }

        private void OnDisable() 
        {
            timeElapsed = 0f;
        }
    }
}
