using UnityEngine;


namespace ETLG
{
    public class MouseManager : Singleton<MouseManager>
    {
        private RaycastHit hitInfo;  // store the information of the object that the ray hitted
        private PlanetBase currentlyFocusedPlanet;

        private void OnEnable() 
        {
            currentlyFocusedPlanet = null;    
        }

        private void MouseControl() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // store the returned value into hitInfo
            Physics.Raycast(ray, out hitInfo);

            // if left mouse button is clicked and the object that the ray has hit has collider
            if (Input.GetMouseButtonDown(0) && hitInfo.collider != null) {
                // if clicked on a planet
                if (hitInfo.collider.gameObject.CompareTag("Planet") && currentlyFocusedPlanet == null) {
                    currentlyFocusedPlanet = hitInfo.collider.gameObject.GetComponent<PlanetBase>();
                    GameEntry.Event.Fire(this, FocusOnPlanetEventArgs.Create(currentlyFocusedPlanet));
                }
                if (hitInfo.collider.gameObject.CompareTag("LandingPoint") && currentlyFocusedPlanet != null)
                {
                    Debug.Log("Click on landingpoint");
                }
                
            }

            // if right clicked
            if (Input.GetMouseButtonDown(1)) {
                if (currentlyFocusedPlanet != null)
                {
                    GameEntry.Event.Fire(this, UnFocusOnPlanetEventArgs.Create(currentlyFocusedPlanet));
                    currentlyFocusedPlanet = null;
                }
            }
        }

        private void Update() {
            MouseControl();      
        }

        private void OnDisable() 
        {
            currentlyFocusedPlanet = null;    
        }
    }
}

