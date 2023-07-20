using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloat : MonoBehaviour
{
    // 设置浮动的最大偏移量
    public float maxOffset = 0.1f;

    // 设置浮动的速度
    public float floatSpeed = 1.0f;

    // 设置旋转的速度
    public float rotationSpeed = 0.80f;

    // 初始位置
    private Vector3 initialPosition;

    void Start()
    {
        // 保存初始位置
        initialPosition = transform.position;
    }

    void Update()
    {
        // 计算浮动的偏移量，使用Perlin噪声可以产生更平滑的随机效果
        float offsetX = Mathf.PerlinNoise(Time.time * floatSpeed, 0) * 2 - 1;
        float offsetY = Mathf.PerlinNoise(0, Time.time * floatSpeed) * 2 - 1;
        float offsetZ = Mathf.PerlinNoise(Time.time * floatSpeed, Time.time * floatSpeed) * 2 - 1;

        // 将偏移量缩放到maxOffset范围内
        offsetX *= maxOffset;
        offsetY *= maxOffset;
        offsetZ *= maxOffset;

        // 应用偏移量到模型的位置
        transform.position = initialPosition + new Vector3(offsetX, offsetY, offsetZ);

        // 中心旋转
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

}
