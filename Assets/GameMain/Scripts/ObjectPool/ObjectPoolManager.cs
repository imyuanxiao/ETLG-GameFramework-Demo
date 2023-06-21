using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.ObjectPool;
using System;

namespace ETLG
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

        private GameObject _objectPoolEmptyHolder;
        private static GameObject _particleSystemsEmpty;
        private static GameObject _gameObjectsEmpty;

        public enum PoolType { ParticleSystem, GameObject, None }
        public static PoolType PoolingType;

        private void Awake() 
        {
            SetupEmpties();    
        }

        private void SetupEmpties()
        {
            _objectPoolEmptyHolder = new GameObject("Pooled Objects");

            _particleSystemsEmpty = new GameObject("Particle Effects");
            _gameObjectsEmpty = new GameObject("GameObjects");

            _particleSystemsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
            _gameObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
        }

        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
        {
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

            // If the pool doesn't exist, create it
            if (pool == null)
            {
                pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
                ObjectPools.Add(pool);
            }

            // check if there are any inactive objects in the pool
            GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObj == null)
            {
                // Find the parent object of the empty object
                GameObject parentObject = SetParentObject(poolType);

                // If there are no inactive objects, create a new one
                spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

                if (parentObject != null)
                {
                    spawnableObj.transform.SetParent(parentObject.transform);
                }
            }
            else
            {
                // If there is an inactive object, reactive it
                spawnableObj.transform.position = spawnPosition;
                spawnableObj.transform.rotation = spawnRotation;
                pool.InactiveObjects.Remove(spawnableObj);
                spawnableObj.SetActive(true);
            }

            return spawnableObj;
        }

        public static void ReturnObjectToPool(GameObject obj)
        {
            string goName = obj.name.Substring(0, obj.name.Length - 7); // by taking off 7, we are removing the (Clone) from the name of the passed in obj
            PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

            if (pool == null)
            {
                Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
            }
            else 
            {
                obj.SetActive(false);
                pool.InactiveObjects.Add(obj);
            }
        }

        public static GameObject SetParentObject(PoolType poolType)
        {
            switch (poolType)
            {
                case PoolType.ParticleSystem:
                    return _particleSystemsEmpty;
                case PoolType.GameObject:
                    return _gameObjectsEmpty;
                case PoolType.None:
                    return null;
                default:
                    return null;
            }
        }
    }

    public class PooledObjectInfo
    {
        public string LookupString;
        public List<GameObject> InactiveObjects = new List<GameObject>();
    }
}
