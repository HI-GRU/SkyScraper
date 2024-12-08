using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingButtonHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect scrollRect;
    private RectTransform buttonRect;
    private Vector2 originalPosition;
    private Vector2 dragStartPosition;
    private bool isVerticalDrag = false;
    private float maxVerticalDrag;
    private bool isHorizontalDrag = false;
    private float minHorizontalDrag;

    private GameObject buildingObj;

    public void Initialize(GameObject buildingObj)
    {
        this.buildingObj = buildingObj;
    }

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        buttonRect = GetComponent<RectTransform>();
        scrollRect = FindScrollRect(transform);
        originalPosition = buttonRect.anchoredPosition;
        maxVerticalDrag = buttonRect.rect.height / 1.8F;
        minHorizontalDrag = buttonRect.rect.height / 4F;
    }

    private ScrollRect FindScrollRect(Transform cur)
    {
        if (cur == null) return null;

        ScrollRect next = cur.GetComponent<ScrollRect>();
        if (next != null) return next;

        return FindScrollRect(cur.parent);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        eventData.eligibleForClick = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPosition = eventData.position;
        isVerticalDrag = true;
        scrollRect.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 수직, 수평 드래그 여부와 상관없이 바깥으로 드래그하면 빌딩 생성
        float dragDeltaY = eventData.position.y - dragStartPosition.y;
        if (dragDeltaY + originalPosition.y > maxVerticalDrag)
        {
            SetBuildingActive();
            ResetScrollState(eventData);
            return;
        }

        // 수직 드래그인지 수평드래그인지 체크
        if (isVerticalDrag)
        {
            float dragDeltaX = Math.Abs(eventData.position.x - dragStartPosition.x);
            if (dragDeltaX > minHorizontalDrag)
            {
                isVerticalDrag = false;
                isHorizontalDrag = true;
                scrollRect.enabled = true;
                ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
            }
        }

        if (isVerticalDrag)
        {
            float newY = originalPosition.y + Mathf.Clamp(dragDeltaY, 0f, buttonRect.rect.height / 2F);
            buttonRect.anchoredPosition = new Vector2(originalPosition.x, newY);
        }
        else if (isHorizontalDrag)
        {
            buttonRect.anchoredPosition = originalPosition;
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.dragHandler);
        }
    }

    private void SetBuildingActive()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, 0);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            buildingObj.transform.position = hitPoint;
            buildingObj.SetActive(true);
            gameObject.SetActive(false);
            TouchManager.Instance.selectedBuildingObj = buildingObj;
        }
    }

    private void ResetScrollState(PointerEventData eventData)
    {
        // 수평 드래그 중이었다면 드래그 종료 이벤트 발생
        if (isHorizontalDrag)
        {
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
        }

        // 스크롤뷰 활성화 및 위치 초기화
        scrollRect.enabled = true;
        scrollRect.velocity = Vector2.zero;

        // 버튼 상태 초기화
        buttonRect.anchoredPosition = originalPosition;
        isVerticalDrag = false;
        isHorizontalDrag = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isVerticalDrag && !isHorizontalDrag) return;

        if (isHorizontalDrag)
        {
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
        }

        isVerticalDrag = false;
        isHorizontalDrag = false;
        buttonRect.anchoredPosition = originalPosition;
        scrollRect.enabled = true;
    }
}
