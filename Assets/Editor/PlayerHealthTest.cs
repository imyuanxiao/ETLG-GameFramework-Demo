using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ETLG;
using UnityEngine.Assertions;
using GameFramework.Event;


[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthTest : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // assign the script to a variable
        PlayerHealth playerHealth = (PlayerHealth) target;

        EditorGUILayout.LabelField("Script Testing");
        if (GUILayout.Button("Test", GUILayout.Width(90f)))
        {
            Debug.Log("Start Testing ...");
            // Write your test functions
            PlayerHealthSimplePasses(playerHealth);
            Debug.Log("Test Ends");
        }
    }

    private void PlayerHealthSimplePasses(PlayerHealth playerHealth)
    {
        playerHealth.CurrentHealth = 50;
        playerHealth.CurrentShield = 10;
        playerHealth.MaxHealth = 50;
        playerHealth.MaxShield = 10;

        playerHealth.TakeDamage(5);
        Assert.AreEqual(5, playerHealth.CurrentShield);
        Assert.AreEqual(10, playerHealth.MaxShield);
        Assert.AreEqual(50, playerHealth.CurrentHealth);
        Assert.AreEqual(50, playerHealth.MaxHealth);

        playerHealth.TakeDamage(3);
        Assert.AreEqual(2, playerHealth.CurrentShield);
        Assert.AreEqual(10, playerHealth.MaxShield);
        Assert.AreEqual(50, playerHealth.CurrentHealth);
        Assert.AreEqual(50, playerHealth.MaxHealth);

        playerHealth.TakeDamage(30);
        Assert.AreEqual(0, playerHealth.CurrentShield);
        Assert.AreEqual(10, playerHealth.MaxShield);
        Assert.AreEqual(50, playerHealth.CurrentHealth);
        Assert.AreEqual(50, playerHealth.MaxHealth);

        // playerHealth.TakeDamage(1);
        // Assert.AreEqual(0, playerHealth.CurrentShield);
        // Assert.AreEqual(10, playerHealth.MaxShield);
        // Assert.AreEqual(49, playerHealth.CurrentHealth);
        // Assert.AreEqual(50, playerHealth.MaxHealth);
    }
}
