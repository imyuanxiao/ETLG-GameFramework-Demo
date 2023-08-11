using UnityEngine;
using UnityEngine.EventSystems;

public class UISkillTreeMapDraggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IScrollHandler
{
    private bool isDragging;
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

            Vector2 newPosition = rectTransform.anchoredPosition + new Vector2(0f, mouseDelta.y);
            rectTransform.anchoredPosition = newPosition;
            ClampPosition();
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        float scrollDelta = eventData.scrollDelta.y * 30;
        Vector2 newPosition = rectTransform.anchoredPosition + new Vector2(0f, scrollDelta);
        rectTransform.anchoredPosition = newPosition;
        ClampPosition();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDragging = false;
        }
    }

    private void ClampPosition()
    {
        Vector2 clampedPosition = rectTransform.anchoredPosition;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -300f, 300f);
        rectTransform.anchoredPosition = clampedPosition;
    }
}
