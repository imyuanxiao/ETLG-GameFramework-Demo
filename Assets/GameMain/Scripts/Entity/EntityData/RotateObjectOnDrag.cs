using UnityEngine;

namespace ETLG
{
    public class RotateObjectOnDrag : MonoBehaviour
    {
        private bool isDragging = false;
        private float dragSpeed = 10f;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // 鼠标左键按下时进行射线检测
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                }
            }

            if (Input.GetMouseButtonUp(0)) // 鼠标左键松开停止拖拽
            {
                isDragging = false;
            }
        }

        private void OnMouseDrag()
        {
            if (isDragging) // 当拖拽标志为真时进行旋转
            {
                float rotationX = Input.GetAxis("Mouse X") * dragSpeed;
                //float rotationY = Input.GetAxis("Mouse Y") * dragSpeed;

                transform.Rotate(Vector3.up, -rotationX, Space.World);
                //transform.Rotate(Vector3.right, rotationY, Space.World);
            }
        }
    }
}
