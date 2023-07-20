using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    // 自转速度
    public float rotationSpeed = 0.8f;
    public int direction = 0;

    void Update()
    {
        if (direction == 0)
        {
            // 让星球自转
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        } else {
            // 让星球自转
            transform.Rotate(Vector3.down, rotationSpeed * Time.deltaTime);
        }
    }
}

