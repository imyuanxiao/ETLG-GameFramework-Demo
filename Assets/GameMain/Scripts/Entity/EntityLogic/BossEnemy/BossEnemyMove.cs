using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class BossEnemyMove : MonoBehaviour
    {
        private Vector3 originPos;
        private float leftBoundary;
        private float rightBoundary;
        private float targetPos;
        private float moveSpeed = 10f;
        private float idleTime;
        private float timeElapsed;

        private void OnEnable() 
        {
            this.originPos = transform.position;
            this.leftBoundary = originPos.x - 10;
            this.rightBoundary = originPos.x + 10;
            this.targetPos = leftBoundary;
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
            if (Mathf.Abs(transform.position.x - targetPos) > 0.1)
            {
                Vector3 target = new Vector3(targetPos, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(targetPos, transform.position.y, transform.position.z);
                SwitchTargetPos();
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
