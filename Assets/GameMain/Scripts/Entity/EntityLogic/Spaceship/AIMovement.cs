using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class AIMovement : MonoBehaviour
    {
        private Vector3 originPos;
        private Vector3 leftBoundary;
        private Vector3 rightBoundary;
        private Vector3 targetPos;
        private float moveSpeed = 20f;
        private float idleTime;
        private float timeElapsed;

        private void OnEnable() 
        {
            this.originPos = transform.position;
            this.leftBoundary = new Vector3(originPos.x - 50, originPos.y, originPos.z);
            this.rightBoundary = new Vector3(originPos.x + 50, originPos.y, originPos.z);
            InitTargetPos();
            this.idleTime = Random.Range(0.5f, 2.0f);
            this.timeElapsed = 0f;
        }

        private void Update() 
        {
            if (timeElapsed < idleTime)
            {
                timeElapsed += Time.deltaTime;
                return;
            }
            if (Vector3.Distance(transform.position, targetPos) > 0.1)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = targetPos;
                SwitchTargetPos();
            }
        }

        private void InitTargetPos()
        {
            int idx = Random.Range(0, 2);
            if (idx == 0)
            {
                this.targetPos = this.leftBoundary;
            }
            else
            {
                this.targetPos = this.rightBoundary;
            }
        }

        private void SwitchTargetPos()
        {
            if (this.targetPos == this.leftBoundary)
            {
                this.targetPos = this.rightBoundary;
            }
            else
            {
                this.targetPos = this.leftBoundary;
            }
        }

        private void OnDisable() 
        {
            this.timeElapsed = 0f;
        }
    }
}
