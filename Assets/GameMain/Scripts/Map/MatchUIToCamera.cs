using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUIToCamera : MonoBehaviour
{
    private Camera mainCamera;
    private RectTransform uiRectTransform;

    void Start()
    {
        // 获取主相机的引用
        mainCamera = Camera.main;

        // 获取UI的RectTransform组件
        uiRectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        // 获取主相机的旋转角度，并应用到UI的角度
        uiRectTransform.rotation = mainCamera.transform.rotation;
    }
}

