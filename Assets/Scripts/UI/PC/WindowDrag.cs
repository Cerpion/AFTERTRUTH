using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private RectTransform _window;

    private Vector2 _offset;
    private const float SIDE_MARGIN = 60;
    private const float BOTTOM_MARGIN = 400;

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _window.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 mousePosition
        );

        _offset = _window.anchoredPosition - mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _window.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 mousePosition
        );

        Vector2 position = mousePosition + _offset;

        RectTransform parent = _window.parent as RectTransform;

        float halfWidth = _window.rect.width * 0.5f;
        float halfHeight = _window.rect.height * 0.5f;


        float minX = -parent.rect.width * 0.5f + halfWidth - SIDE_MARGIN;
        float maxX = parent.rect.width * 0.5f - halfWidth + SIDE_MARGIN;

        float minY = -parent.rect.height * 0.5f + halfHeight - BOTTOM_MARGIN;
        float maxY = parent.rect.height * 0.5f - halfHeight;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        _window.anchoredPosition = position;
    }
}
