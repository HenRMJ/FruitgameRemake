using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class UICustomizationElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private string elementName;
    [SerializeField] private bool inGame;

    private RectTransform uiElement;
    private Vector2 initialOffset;
    private Vector2 initialPosition;

    private void Awake()
    {
        uiElement = GetComponent<RectTransform>();
        initialOffset = uiElement.anchoredPosition;
    }

    private void Start()
    {
        if (MySaveManager.Instance.TryLoadUIPosition(elementName, out Vector2 position))
        {
            uiElement.anchoredPosition = position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        

        initialOffset = uiElement.anchoredPosition - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inGame) return;

        uiElement.anchoredPosition = eventData.position + initialOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (inGame) return;

        MySaveManager.Instance.SaveUIPoistions(elementName, uiElement.anchoredPosition);
    }

    public void ResetPosition()
    {
        uiElement.anchoredPosition = initialPosition;
    }

    public string GetElementKey() => elementName;
}
