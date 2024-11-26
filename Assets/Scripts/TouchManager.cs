using System;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private LayerMask buildingLayer;
    private LayerMask tileLayer;
    private Camera mainCamera;

    private readonly Plane XZPlane = new Plane(Vector3.up, 0);
    private GameObject selectedBuilding;
    private Vector3 dragOffset;
    private bool isDragging => selectedBuilding != null;

    private void Start()
    {
        mainCamera = GameManager.Instance.MainCamera;
        buildingLayer = GameManager.Instance.BuildingLayer;
        tileLayer = GameManager.Instance.TileLayer;
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
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (XZPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                Vector3 newPosition = new Vector3(
                    hitPoint.x - dragOffset.x,
                    selectedBuilding.transform.position.y,
                    hitPoint.z - dragOffset.z
                );

                Cell cell = StageManager.Instance.GridSystem.GetCell(newPosition);

                if (cell != null)
                {
                    newPosition.x = Mathf.Round(newPosition.x);
                    newPosition.z = Mathf.Round(newPosition.z);
                }

                selectedBuilding.transform.position = newPosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedBuilding = null;
        }
    }

    private void HandleSelecting()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingLayer))
        {
            selectedBuilding = hit.transform.gameObject;

            if (XZPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                dragOffset = hitPoint - selectedBuilding.transform.position;
            }
        }
    }
}