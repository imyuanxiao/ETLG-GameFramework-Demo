using UnityEngine;
using UnityEngine.EventSystems;

public class UISkillTreeMapDraggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool isDragging;
    private Vector2 startPosition;
    private Vector2 previousMousePosition;

    public RectTransform rectTransform;

    private void Start()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDragging = true;
            startPosition = rectTransform.anchoredPosition;
            previousMousePosition = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 currentMousePosition = eventData.position;
            Vector2 mouseDelta = currentMousePosition - previousMousePosition;
            previousMousePosition = currentMousePosition;

            rectTransform.anchoredPosition += new Vector2(0f, mouseDelta.y);

//            rectTransform.anchoredPosition += mouseDelta;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDragging = false;
        }
    }
}
