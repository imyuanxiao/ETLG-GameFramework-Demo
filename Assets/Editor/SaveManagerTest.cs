using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ETLG;
using UnityEngine.Assertions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerTest : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SaveManager saveManager = (SaveManager) target;

        EditorGUILayout.LabelField("Script Testing");
        if (GUILayout.Button("Test", GUILayout.Width(90f)))
        {
            Debug.Log("Tests Start ... ");
            SaveManagerSimplePasses(saveManager);
            SaveManagerAlterTest(saveManager);
            SaveManagerArrayTest(saveManager);
            SaveManagerDeserialzeObjTest(saveManager);
            SaveManagerDeserialzeObjAlterTest(saveManager);
            SaveManagerLoadTest(saveManager);
            Debug.Log("All Tests passed");
        }
    }

    private void SaveManagerSimplePasses(SaveManager saveManager)
    {
        PlayerPrefs.DeleteKey("TestData");

        SaveTestClass data = new SaveTestClass();
        saveManager.Save("TestData", data);
        JObject jsonData = saveManager.LoadJsonObject("TestData");
        
        Assert.AreEqual(10, jsonData["intVar"]);
        Assert.AreEqual(12.3, jsonData["floatVar"]);
        Assert.AreEqual(15.67, jsonData["doubleVar"]);
        Assert.AreEqual("test class", jsonData["stringVar"]);

        Assert.AreEqual(0, jsonData["arrayVar"][0]);
        Assert.AreEqual(1, jsonData["arrayVar"][1]);
        Assert.AreEqual(2, jsonData["arrayVar"][2]);
        Assert.AreEqual(3, jsonData["arrayVar"][3]);
        Assert.AreEqual(4, jsonData["arrayVar"][4]);
        Assert.AreEqual(89, jsonData["arrayVar"][5]);

        Assert.AreEqual(4, jsonData["listVar"][0]);
        Assert.AreEqual(5, jsonData["listVar"][1]);
        Assert.AreEqual(6, jsonData["listVar"][2]);
        Assert.AreEqual(7, jsonData["listVar"][3]);

        Assert.AreEqual(11, jsonData["dictVar"]["1001"]);
        Assert.AreEqual(12, jsonData["dictVar"]["1002"]);

        Assert.AreEqual("sub class", jsonData["subSaveTestClass"]["stringVar"]);
        Assert.AreEqual(199, jsonData["subSaveTestClass"]["intVar"]);

        JsonConvert.DeserializeObject<SaveTestClass>(JsonConvert.SerializeObject(data));

        PlayerPrefs.DeleteKey("TestData");
    }

    private void SaveManagerAlterTest(SaveManager saveManager)
    {
        PlayerPrefs.DeleteKey("TestData");

        SaveTestClass data = new SaveTestClass();
        saveManager.Save("TestData", data);
        JObject jsonData = saveManager.LoadJsonObject("TestData");

        Assert.AreEqual(10, jsonData["intVar"]);

        data.intVar += 10;
        Assert.AreEqual(20, data.intVar);
        jsonData = saveManager.LoadJsonObject("TestData");
        Assert.AreEqual(10, jsonData["intVar"]);
        
        saveManager.Save("TestData", data);
        jsonData = saveManager.LoadJsonObject("TestData");
        Assert.AreEqual(20, jsonData["intVar"]);

        data.listVar[1] = 99;
        Assert.AreEqual(99, data.listVar[1]);
        jsonData = saveManager.LoadJsonObject("TestData");
        Assert.AreEqual(5, jsonData["listVar"][1]);

        saveManager.Save("TestData", data);
        jsonData = saveManager.LoadJsonObject("TestData");
        Assert.AreEqual(99, jsonData["listVar"][1]);

        PlayerPrefs.DeleteKey("TestData");
    }

    private void SaveManagerArrayTest(SaveManager saveManager)
    {
        PlayerPrefs.DeleteKey("TestData");

        SaveTestClass data = new SaveTestClass();
        saveManager.Save("TestData", data);
        JObject jsonData = saveManager.LoadJsonObject("TestData");

        JArray jsonArray = (JArray) jsonData["arrayVar"];
        for (int i=0; i < jsonArray.Count; i++)
        {
            Assert.AreEqual(data.arrayVar[i], jsonArray[i]);
        }

        JArray jsonList = (JArray) jsonData["listVar"];
        for (int i=0; i < jsonList.Count; i++)
        {
            Assert.AreEqual(data.listVar[i], jsonList[i]);
        }

        PlayerPrefs.DeleteKey("TestData");
    }

    private void SaveManagerDeserialzeObjTest(SaveManager saveManager)
    {
        PlayerPrefs.DeleteKey("TestData");

        SaveTestClass data = new SaveTestClass();
        saveManager.Save("TestData", data);

        SaveTestClass newData = saveManager.LoadObject<SaveTestClass>("TestData");

        Assert.AreEqual(data.intVar, newData.intVar);
        Assert.AreEqual(data.floatVar, newData.floatVar);
        Assert.AreEqual(data.doubleVar, newData.doubleVar);
        Assert.AreEqual(data.stringVar, newData.stringVar);

        Assert.AreEqual(data.arrayVar[0], newData.arrayVar[0]);
        Assert.AreEqual(data.arrayVar[1], newData.arrayVar[1]);
        Assert.AreEqual(data.arrayVar[2], newData.arrayVar[2]);
        Assert.AreEqual(data.arrayVar[3], newData.arrayVar[3]);
        Assert.AreEqual(data.arrayVar[4], newData.arrayVar[4]);
        Assert.AreEqual(data.arrayVar[5], newData.arrayVar[5]);

        Assert.AreEqual(data.listVar[0], newData.listVar[0]);
        Assert.AreEqual(data.listVar[1], newData.listVar[1]);
        Assert.AreEqual(data.listVar[2], newData.listVar[2]);
        Assert.AreEqual(data.listVar[3], newData.listVar[3]);

        Assert.AreEqual(data.dictVar[1001], newData.dictVar[1001]);
        Assert.AreEqual(data.dictVar[1002], newData.dictVar[1002]);
        Assert.AreEqual(data.dictVar.Count, newData.dictVar.Count);

        Assert.AreEqual(data.subSaveTestClass.intVar, newData.subSaveTestClass.intVar);
        Assert.AreEqual(data.subSaveTestClass.stringVar, newData.subSaveTestClass.stringVar);

        PlayerPrefs.DeleteKey("TestData");
    }

    private void SaveManagerDeserialzeObjAlterTest(SaveManager saveManager)
    {
        PlayerPrefs.DeleteKey("TestData");

        SaveTestClass data = new SaveTestClass();
        saveManager.Save("TestData", data);

        SaveTestClass newData = saveManager.LoadObject<SaveTestClass>("TestData");
        Assert.AreEqual(10, newData.intVar);

        data.intVar += 10;
        Assert.AreEqual(20, data.intVar);

        newData = saveManager.LoadObject<SaveTestClass>("TestData");
        Assert.AreEqual(10, newData.intVar);

        saveManager.Save("TestData", data);
        newData = saveManager.LoadObject<SaveTestClass>("TestData");
        Assert.AreEqual(20, newData.intVar);

        data.dictVar.Add(1003, 13);
        Assert.AreEqual(13, data.dictVar[1003]);

        newData = saveManager.LoadObject<SaveTestClass>("TestData");
        Assert.IsFalse(newData.dictVar.ContainsKey(1003));

        saveManager.Save("TestData", data);
        newData = saveManager.LoadObject<SaveTestClass>("TestData");
        Assert.AreEqual(13, newData.dictVar[1003]);

        PlayerPrefs.DeleteKey("TestData");
    }

    private void SaveManagerLoadTest(SaveManager saveManager)
    {
        PlayerPrefs.DeleteKey("TestData");

        SaveTestClass data = new SaveTestClass();
        saveManager.Save("TestData", data);

        data.intVar += 10;
        data.dictVar.Add(1003, 13);
        data.subSaveTestClass.intVar += 10;
        Assert.AreEqual(20, data.intVar);
        Assert.AreEqual(13, data.dictVar[1003]);
        Assert.AreEqual(209, data.subSaveTestClass.intVar);

        SaveTestClass newData = new SaveTestClass();
        saveManager.Load("TestData", newData);
        Assert.AreEqual(10, newData.intVar);
        Assert.IsFalse(newData.dictVar.ContainsKey(1003));
        Assert.AreEqual(199, newData.subSaveTestClass.intVar);

        saveManager.Save("TestData", data);
        saveManager.Load("TestData", newData);
        Assert.AreEqual(20, newData.intVar);
        Assert.AreEqual(13, newData.dictVar[1003]);
        Assert.AreEqual(209, newData.subSaveTestClass.intVar);

        PlayerPrefs.DeleteKey("TestData");
    }
}

public class SaveTestClass
{
    public int intVar = 10;
    public float floatVar = 12.3f;
    public double doubleVar = 15.67;
    public string stringVar = "test class";
    public int[] arrayVar = {0, 1, 2, 3, 4, 89};
    public List<int> listVar = new List<int>(){ 4, 5, 6, 7 };
    public Dictionary<int, int> dictVar = new Dictionary<int, int>(){ {1001, 11}, {1002, 12} };
    public SubSaveTestClass subSaveTestClass = new SubSaveTestClass();

    public SaveTestClass()
    {
        this.intVar = 10;
    }

    private void SelfDefinedMethod()
    {

    }

    public void SelfDefinedMethod2()
    {

    }
}

public class SubSaveTestClass
{
    public string stringVar = "sub class";
    public int intVar = 199;
}
