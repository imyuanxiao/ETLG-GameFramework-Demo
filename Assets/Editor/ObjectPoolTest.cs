using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ETLG;
using UnityEngine.Assertions;

[CustomEditor(typeof(ObjectPoolManager))]
public class ObjectPoolTest : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // assign the script to a variable
        ObjectPoolManager objectPoolManager = (ObjectPoolManager) target;

        EditorGUILayout.LabelField("Script Testing");
        if (GUILayout.Button("Test", GUILayout.Width(90f)))
        {
            Debug.Log("Start Testing ...");
            ObjectPoolTestSimplePasses(objectPoolManager);
            ObjectPoolBasicTests(objectPoolManager);
            Debug.Log("Test Ends");
        }
    }

    private void ObjectPoolTestSimplePasses(ObjectPoolManager objectPoolManager)
    {
        objectPoolManager.Awake();

        GameObject testingObj = new GameObject("TestingObj");
        GameObject objPrefab = new GameObject("Bullet");
        GameObject obj = ObjectPoolManager.SpawnObject(objPrefab, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            
        objPrefab.transform.SetParent(testingObj.transform);
            
        Assert.AreEqual(Vector3.zero, obj.transform.position);
        Assert.AreEqual(1, ObjectPoolManager.ObjectPools.Count);
        Assert.AreEqual("Bullet", ObjectPoolManager.ObjectPools[0].LookupString);
        Assert.AreEqual(0, ObjectPoolManager.ObjectPools[0].InactiveObjects.Count);

        ObjectPoolManager.ReturnObjectToPool(obj);
        Assert.AreEqual(1, ObjectPoolManager.ObjectPools[0].InactiveObjects.Count);
        Assert.AreEqual(false, obj.activeSelf);

        GameObject obj2 = ObjectPoolManager.SpawnObject(objPrefab, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        Assert.AreEqual(Vector3.zero, obj2.transform.position);
        Assert.AreEqual(1, ObjectPoolManager.ObjectPools.Count);
        Assert.AreEqual("Bullet", ObjectPoolManager.ObjectPools[0].LookupString);
        Assert.AreEqual(0, ObjectPoolManager.ObjectPools[0].InactiveObjects.Count);

        DestroyImmediate(testingObj);

        objectPoolManager.OnDisable();
    }

    public void ObjectPoolBasicTests(ObjectPoolManager objectPoolManager)
    {
        objectPoolManager.Awake();

        GameObject testingObj = new GameObject("TestingObj");
        GameObject objPrefab1 = new GameObject("Bullet");
        GameObject objPrefab2 = new GameObject("Missile");

        objPrefab1.transform.SetParent(testingObj.transform);
        objPrefab2.transform.SetParent(testingObj.transform);

        Assert.AreEqual(0, ObjectPoolManager.ObjectPools.Count);
        Assert.IsNull(GetObjectPoolByLookUpString("Bullet"));
        Assert.IsNull(GetObjectPoolByLookUpString("Missile"));

        GameObject obj1 = ObjectPoolManager.SpawnObject(objPrefab1, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        GameObject obj2 = ObjectPoolManager.SpawnObject(objPrefab1, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        GameObject obj3 = ObjectPoolManager.SpawnObject(objPrefab1, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);

        Assert.AreEqual("Bullet(Clone)", obj1.name);
        Assert.AreEqual("Bullet(Clone)", obj2.name);
        Assert.AreEqual("Bullet(Clone)", obj3.name);

        Assert.IsNotNull(GetObjectPoolByLookUpString("Bullet"));
        Assert.IsNull(GetObjectPoolByLookUpString("Missile"));

        Assert.AreEqual(1, ObjectPoolManager.ObjectPools.Count);
        Assert.AreEqual(0, ObjectPoolManager.ObjectPools[0].InactiveObjects.Count);

        GameObject obj4 = ObjectPoolManager.SpawnObject(objPrefab2, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            
        Assert.IsNotNull(GetObjectPoolByLookUpString("Bullet"));
        Assert.IsNotNull(GetObjectPoolByLookUpString("Missile"));

        Assert.AreEqual(2, ObjectPoolManager.ObjectPools.Count);
        Assert.AreEqual(0, GetObjectPoolByLookUpString("Missile").InactiveObjects.Count);

        ObjectPoolManager.ReturnObjectToPool(obj1);
        Assert.AreEqual(false, obj1.activeSelf);
        Assert.AreEqual(1, GetObjectPoolByLookUpString("Bullet").InactiveObjects.Count);
        Assert.AreEqual(0, GetObjectPoolByLookUpString("Missile").InactiveObjects.Count);

        ObjectPoolManager.ReturnObjectToPool(obj2);
        Assert.AreEqual(false, obj2.activeSelf);
        Assert.AreEqual(2, GetObjectPoolByLookUpString("Bullet").InactiveObjects.Count);
        Assert.AreEqual(0, GetObjectPoolByLookUpString("Missile").InactiveObjects.Count);

        ObjectPoolManager.SpawnObject(objPrefab1, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        Assert.AreEqual(1, GetObjectPoolByLookUpString("Bullet").InactiveObjects.Count);
        Assert.AreEqual(0, GetObjectPoolByLookUpString("Missile").InactiveObjects.Count);

        ObjectPoolManager.ReturnObjectToPool(obj4);
        Assert.AreEqual(false, obj4.activeSelf);
        Assert.AreEqual(1, GetObjectPoolByLookUpString("Bullet").InactiveObjects.Count);
        Assert.AreEqual(1, GetObjectPoolByLookUpString("Missile").InactiveObjects.Count);

        Assert.AreEqual(2, ObjectPoolManager.ObjectPools.Count);

        DestroyImmediate(testingObj);

        objectPoolManager.OnDisable();
    }

    private PooledObjectInfo GetObjectPoolByLookUpString(string lookup)
    {
        if (ObjectPoolManager.ObjectPools.Count == 0)
        {
            return null;
        }

        foreach (var objectPool in ObjectPoolManager.ObjectPools)
        {
            if (objectPool.LookupString == lookup)
            {
                return objectPool;
            }
        }
        return null;
    }
}

