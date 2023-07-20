using UnityEngine;
// using Cinemachine;
using System.Collections;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace ETLG
{
    public class CameraController : MonoBehaviour
    {
        public float moveSpeed;
        public Transform cameraBounds;

        private float leftBoundary;
        private float rightBoundary;
        private float upBoundary;
        private float bottomBoundary;

        private Vector3 panoramicPosition;
        private Quaternion panoramicRotation;
        private Transform focusedPlanet;
        public bool isFocused;


        private void Start()
        {
            // Calculate the left, right, up and bottom movement boundaries of the camera
            leftBoundary = cameraBounds.position.x - cameraBounds.localScale.x / 2f - 2;
            rightBoundary = cameraBounds.position.x + cameraBounds.localScale.x / 2f + 10;
            upBoundary = cameraBounds.position.z + cameraBounds.localScale.z / 2f - 4;
            bottomBoundary = cameraBounds.position.z - cameraBounds.localScale.z / 2f + 7;
        }

        private void OnEnable() 
        {
            GameEntry.Event.Subscribe(FocusOnPlanetEventArgs.EventId, OnFocusOnPlanet);
            GameEntry.Event.Subscribe(UnFocusOnPlanetEventArgs.EventId, OnResetCamera);
        }

        private void OnResetCamera(object sender, GameEventArgs e)
        {
            UnFocusOnPlanetEventArgs ne = (UnFocusOnPlanetEventArgs) e;
            if (ne == null)
            {
                Log.Error("Invalid event UnFocusOnPlanetEventArgs");
            }
            ResetCamera();
        }

        private void OnFocusOnPlanet(object sender, GameEventArgs e)
        {
            FocusOnPlanetEventArgs ne = (FocusOnPlanetEventArgs) e;
            if (ne == null)
            {
                Log.Error("Invalid event FocusOnPlanetEventArgs");
            }
            Transform focusPoint = ne.PlanetBase.focusPoint;
            FocusOnPlanet(focusPoint);
        }

        private void Update()
        {
            MoveCamera();
        }

        private void MoveCamera() {
            if (isFocused) { return; }
            // Check if the mouse is at the right boundary
            if (Input.mousePosition.x <= 30) {
                MoveCameraRight();
            }
            // Check if the mouse is at the left boundary
            else if (Input.mousePosition.x >= Screen.width - 30) {
                MoveCameraLeft();
            }
            // Check if the mouse is at the up boundary
            else if (Input.mousePosition.y <= 30) {
                MoveCameraUp();
            }
            // Check if the mouse is at the bottom boundary
            else if (Input.mousePosition.y >= Screen.height - 30) {
                MoveCameraDown();
            }
        }

        private void MoveCameraLeft()
        {
            // Move the camera to the left
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            ClampCameraPosition();
        }

        private void MoveCameraRight()
        {
            // Move the camera to the right
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            ClampCameraPosition();
        }

        private void MoveCameraUp() {
            // Move the camera upward
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
            ClampCameraPosition();
        }

        private void MoveCameraDown() {
            // Move the camera downward
            transform.position += Vector3.back * moveSpeed * Time.deltaTime;
            ClampCameraPosition();
        }

        private void ClampCameraPosition()
        {
            // Clamp the camera position within the boundaries
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, leftBoundary, rightBoundary);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, bottomBoundary, upBoundary);
            transform.position = clampedPosition;
        }

        // move the camera to focus point of the clicked planet
        private void FocusOnPlanet(Transform target) {
            if (isFocused) { return; }
            panoramicPosition = transform.position;
            panoramicRotation = transform.rotation;
            ZoomIn(target);
        }

        // move the camera back to its original position before focusing on planet
        private void ResetCamera() {
            if (!isFocused) { return; }
            // set focused planet to null
            ZoomOut();
            focusedPlanet = null;
        }

        private void ZoomIn(Transform target) {
            StartCoroutine(ZoomInPosition(target));
            StartCoroutine(ZoomInRotation(target));
            focusedPlanet = target;
        }

        private void ZoomOut() {
            StartCoroutine(ZoomOutPosition(panoramicPosition));
            StartCoroutine(ZoomOutRotation(panoramicRotation));
        }

        // move the camera smoothly to target position
        private IEnumerator ZoomInPosition(Transform target) {
            while (Vector3.Distance(transform.position, target.position) > 0.1) {
                transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = target.position;
        }

        // rotate the camera smoothy to target rotation
        private IEnumerator ZoomInRotation(Transform target) {
            while (Quaternion.Angle(transform.rotation, target.rotation) > 0.1) {
                transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.rotation = target.rotation;
            
            isFocused = true;
        }

        // move the camera smoothly to target position
        private IEnumerator ZoomOutPosition(Vector3 target) {
            while (Vector3.Distance(transform.position, target) > 0.1) {
                transform.position = Vector3.Lerp(transform.position, panoramicPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = target;
        }

        // rotate the camera smoothy to target rotation
        private IEnumerator ZoomOutRotation(Quaternion target) {
            while (Quaternion.Angle(transform.rotation, target) > 0.1) {
                transform.rotation = Quaternion.Lerp(transform.rotation, panoramicRotation, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.rotation = target;
            isFocused = false;
        }

        private void OnDisable() 
        {
            GameEntry.Event.Unsubscribe(FocusOnPlanetEventArgs.EventId, OnFocusOnPlanet);  
            GameEntry.Event.Unsubscribe(UnFocusOnPlanetEventArgs.EventId, OnResetCamera);  
        }
    }
}

