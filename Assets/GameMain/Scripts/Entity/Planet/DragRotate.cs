using UnityEngine;

namespace ETLG
{
    public class DragRotate : MonoBehaviour
    {
        private bool isDragging = false;
        private Vector3 previousMousePosition;
        public Transform model;
        private float rotationSpeed = 0f;

        // private void Awake() 
        // {
        //     model = transform.GetChild(0);
        // }
        private void OnEnable() 
        {
            this.rotationSpeed = 0.5f;
        }

        private void OnMouseDown()
        {
            isDragging = true;
            previousMousePosition = Input.mousePosition;
        }

        private void OnMouseUp()
        {
            isDragging = false;
        }

        private void OnMouseDrag()
        {
            if (isDragging)
            {
                Vector3 currentMousePosition = Input.mousePosition;
                Vector3 mouseDelta = currentMousePosition - previousMousePosition;
                previousMousePosition = currentMousePosition;

                float rotationY = mouseDelta.x * rotationSpeed;

                model.Rotate(0, -rotationY, 0);
            }
        }

        private void Update() {
            if (!GetComponent<PlanetBase>().isFocused) { return; }
            if (!GetComponent<DragRotate>().enabled) { return; }

            if (Input.GetMouseButtonDown(0)) {
                OnMouseDown();
            }
            if (Input.GetMouseButtonUp(0)) {
                OnMouseUp();
            }
            OnMouseDrag();
        }

        private void OnDisable() 
        {
            this.rotationSpeed = 0f;    
        }
    }
}

