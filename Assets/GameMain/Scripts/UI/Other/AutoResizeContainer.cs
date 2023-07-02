using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoResizeContainer : MonoBehaviour
{
    public RectTransform container;
    public TextMeshProUGUI text;

    private void Start()
    {
        // 在开始时根据文本长度调整容器高度
        ResizeContainer();
    }

    private void ResizeContainer()
    {
        // 获取文本内容的长度
        float textHeight = text.preferredHeight;

        // 调整容器高度为文本内容的长度
        container.sizeDelta = new Vector2(container.sizeDelta.x, textHeight);
    }
}
