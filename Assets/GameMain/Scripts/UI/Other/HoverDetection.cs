using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDetection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标进入组件时的处理逻辑
        Debug.Log("Mouse entered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标离开组件时的处理逻辑
        Debug.Log("Mouse exited");
    }
}
