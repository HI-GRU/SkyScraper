using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class TouchManager : MonoBehaviour
{
    private LayerMask buildingLayer;
    private LayerMask tileLayer;
    private Camera mainCamera;
    private GridSystem gridSystem;

    private readonly Plane XZPlane = new Plane(Vector3.up, 0);
    private GameObject selectedBuildingObj;
    Building selectedBuilding => StageManager.Instance.buildingInfo[selectedBuildingObj.name];

    private Vector3 dragOffset;
    private bool isDragging => selectedBuildingObj != null;

    private void Awake()
    {
        mainCamera = GameManager.Instance.mainCamera;
        buildingLayer = GameManager.Instance.BuildingLayer;
        tileLayer = GameManager.Instance.TileLayer;
        gridSystem = StageManager.Instance.gridSystem;
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
        if (Input.GetMouseButton(0)) // 드래그 중인 상태
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
            if (selectedBuilding.currentData.isPlaced) gridSystem.RemoveBuilding(selectedBuilding);

            if (XZPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                dragOffset = hitPoint - selectedBuildingObj.transform.position;
            }
        }
    }
}