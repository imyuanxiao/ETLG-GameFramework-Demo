using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        public static T Instance {
            get { return instance; }
        }

        protected virtual void Awake() {
            // delete redundent object
            if (instance != null) {
                Destroy(gameObject);
            } else {
                instance = (T)this;
            }
        }

        public static bool IsInitialized {
            get { return instance != null; }
        }

        // this method will be called when the gameObject is destroyed
        protected virtual void OnDestroy() {
            if (instance == this) {
                instance = null;
            }
        }
    }
}
