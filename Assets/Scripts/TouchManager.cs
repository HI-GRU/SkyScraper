using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance { get; private set; }

    private LayerMask buildingLayer;
    private LayerMask tileLayer;
    private Camera mainCamera;
    private GridSystem gridSystem;

    private readonly Plane XZPlane = new Plane(Vector3.up, 0);
    public GameObject selectedBuildingObj;
    Building selectedBuilding => StageManager.Instance.buildingInfo[selectedBuildingObj.name];

    private Vector3 dragOffset;
    private bool isDragging => selectedBuildingObj != null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        buildingLayer = GameManager.Instance.BuildingLayer;
        tileLayer = GameManager.Instance.TileLayer;
        gridSystem = StageManager.Instance.gridSystem;
        mainCamera = CameraManager.Instance.mainCamera;
    }

    private void Update()
    {
        if (isDragging)
        {
            HandleDragging();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleSelecting();
        }
    }

    private void HandleDragging()
    {
        if (Input.GetMouseButton(0)) // 드래그 진행
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (XZPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                Vector3 newPosition = new Vector3(
                    hitPoint.x - dragOffset.x,
                    selectedBuildingObj.transform.position.y,
                    hitPoint.z - dragOffset.z
                );

                if (gridSystem.isBound(newPosition.x, newPosition.z))
                {
                    newPosition.x = Mathf.Round(newPosition.x);
                    newPosition.z = Mathf.Round(newPosition.z);
                }

                selectedBuildingObj.transform.position = newPosition;
            }
        }
        else if (Input.GetMouseButtonUp(0)) // 드래그 종료
        {
            if (!gridSystem.PlaceBuilding(selectedBuildingObj.transform.position, selectedBuilding))
            {
                int btnNum = selectedBuilding.currentData.buttonNumber;
                GameObject buildingButton = StageManager.Instance.buildingButtonObjs[btnNum];
                buildingButton.SetActive(true);
                selectedBuildingObj.SetActive(false);
                selectedBuildingObj.transform.position = selectedBuilding.currentData.originalPosition;
            }

            selectedBuildingObj = null;
        }
    }

    private void HandleSelecting()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingLayer))
        {
            selectedBuildingObj = hit.transform.gameObject;
            if (selectedBuilding.currentData.isPlaced) gridSystem.RemoveBuilding(selectedBuilding); // 이미 놓여있는 빌딩 선택

            if (XZPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                dragOffset = hitPoint - selectedBuildingObj.transform.position;
            }
        }
    }
}