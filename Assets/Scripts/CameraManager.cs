using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[DefaultExecutionOrder(-1)]
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    public Camera mainCamera { get; private set; }
    private Stage currentStage;

    private Vector3 center; // 타일 중심
    private Vector3 cameraPosition => getCameraPos(); // 타일 중심을 기준으로 반지름만큼 떨어진 위치
    private float angleX = 30F; // YZ 평면으로부터 회전한 값
    private float angleY = -45F; // 카메라 회전 시 변하는 값
    private float radius => Math.Max(currentStage.size.x, Math.Max(currentStage.size.y, currentStage.size.z)) * 10F;

    // 카메라 회전
    private bool isDragging = false;
    private float dragSensitivity = 0.4F;
    private Vector2 dragStartPosition;
    private const float ANGLE_SMOOTHING = 10F;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        mainCamera = Camera.main;
        currentStage = GameManager.Instance.currentStage;
        center = new Vector3(currentStage.size.x / 2F, 0F, currentStage.size.z / 2F);
        mainCamera.orthographicSize = radius / 5F;
    }

    private void Start()
    {
        SetMainView();
    }

    private void Update()
    {
        if (isDragging)
        {
            RotateCamera();
            return;
        }
        if (Input.GetMouseButtonDown(0)) CheckRotate();
    }

    private void RotateCamera()
    {
        if (Input.GetMouseButton(0))
        {
            // 현재 마우스 위치와 드래그 시작 위치의 차이 계산
            float dragDeltaX = Input.mousePosition.x - dragStartPosition.x;

            // angleY 업데이트
            float targetAngleY = angleY - dragDeltaX * dragSensitivity;

            // angleY를 0 ~ 360 범위로 조정
            angleY = (targetAngleY % 360 + 360) % 360;
            SetMainView();
            dragStartPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void SetMainView()
    {
        mainCamera.transform.position = cameraPosition + center;
        mainCamera.transform.LookAt(center);
    }

    private Vector3 getCameraPos()
    {
        Vector3 res = new Vector3();
        res.y = radius * Mathf.Sin(angleX * Mathf.Deg2Rad);
        res.x = radius * Mathf.Cos(angleX * Mathf.Deg2Rad) * Mathf.Cos(angleY * Mathf.Deg2Rad);
        res.z = radius * Mathf.Cos(angleX * Mathf.Deg2Rad) * Mathf.Sin(angleY * Mathf.Deg2Rad);

        return res;
    }

    private void CheckRotate()
    {
        bool CanRotate = true;

        // 3D 오브젝트 클릭 여부
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            CanRotate = !hit.collider.gameObject.activeInHierarchy;
        }

        // GamePanel 터치 여부 확인
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            CanRotate &= "GamePanel".Equals(results[0].gameObject.tag);
        }

        isDragging = CanRotate;
        if (isDragging) dragStartPosition = Input.mousePosition;
    }
}
