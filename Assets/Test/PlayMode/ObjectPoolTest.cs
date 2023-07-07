using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ETLG
{
    public class ObjectPoolTest
    {
        private GameObject objectPoolManager;
        [SetUp]
        public void Setup()
        {
            this.objectPoolManager = new GameObject("ObjectPoolManager");
            this.objectPoolManager.AddComponent<ObjectPoolManager>();
        }
        

        [Test]
        public void ObjectPoolTestSimplePasses()
        {
            GameObject objPrefab = new GameObject("Bullet");

            GameObject obj = ObjectPoolManager.SpawnObject(objPrefab, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
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

            
        }

        [Test]
        public void ObjectPoolBasicTests()
        {
            GameObject objPrefab1 = new GameObject("Bullet");
            GameObject objPrefab2 = new GameObject("Missile");

            Assert.AreEqual(0, ObjectPoolManager.ObjectPools.Count);
            Assert.IsNull(GetObjectPoolByLookUpString("Bullet"));
            Assert.IsNull(GetObjectPoolByLookUpString("Missile"));

            GameObject obj1 = ObjectPoolManager.SpawnObject(objPrefab1, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            GameObject obj2 = ObjectPoolManager.SpawnObject(objPrefab1, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            GameObject obj3 = ObjectPoolManager.SpawnObject(objPrefab1, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);

            Assert.AreEqual("Bullet(Clone)", obj1.name);
            Assert.AreEqual("Bullet(Clone)", obj2.name);
            Assert.AreEqual("Bullet(Clone)", obj3.name);

            Assert.NotNull(GetObjectPoolByLookUpString("Bullet"));
            Assert.IsNull(GetObjectPoolByLookUpString("Missile"));

            Assert.AreEqual(1, ObjectPoolManager.ObjectPools.Count);
            Assert.AreEqual(0, ObjectPoolManager.ObjectPools[0].InactiveObjects.Count);

            GameObject obj4 = ObjectPoolManager.SpawnObject(objPrefab2, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            
            Assert.NotNull(GetObjectPoolByLookUpString("Bullet"));
            Assert.NotNull(GetObjectPoolByLookUpString("Missile"));

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

            objectPoolManager.SetActive(false);

            Assert.AreEqual(0, ObjectPoolManager.ObjectPools.Count);
        }


        [UnityTest]
        public IEnumerator ObjectPoolTestWithEnumeratorPasses()
        {
            yield return null;
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
}
