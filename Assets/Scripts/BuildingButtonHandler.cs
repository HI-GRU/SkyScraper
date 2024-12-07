using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform buttonRect;
    private Vector2 originalPosition;
    private Vector2 dragStartPosition;
    private bool isDragging = false;
    private float maxDragHeight;

    private GameObject buildingObj;

    public void Initialize(GameObject buildingObj)
    {
        this.buildingObj = buildingObj;
    }

    private void Awake()
    {
        buttonRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        buttonRect = GetComponent<RectTransform>();
        Canvas.ForceUpdateCanvases();
        originalPosition = buttonRect.anchoredPosition;
        maxDragHeight = buttonRect.rect.height / 1.8F;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        eventData.eligibleForClick = false;
        dragStartPosition = eventData.position;
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;
        buttonRect.anchoredPosition = originalPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        float dragDeltaY = eventData.position.y - dragStartPosition.y;

        // 건물을 화면에 띄운다
        if (dragDeltaY + originalPosition.y > maxDragHeight)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, 0);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                buildingObj.transform.position = hitPoint;
                buildingObj.SetActive(true);
                gameObject.SetActive(false);
                isDragging = false;

                TouchManager.Instance.selectedBuildingObj = buildingObj;
                return;
            }
        }

        float newY = originalPosition.y + Mathf.Clamp(dragDeltaY, 0f, buttonRect.rect.height / 2F);
        buttonRect.anchoredPosition = new Vector2(originalPosition.x, newY);
    }
}
