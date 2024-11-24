using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private LayerMask buildingLayer;
    [SerializeField] private LayerMask tileLayer;

    private readonly Plane XZPlane = new Plane(Vector3.up, 0);
    private Camera mainCamera;
    private GameObject selectedBuilding;
    private Vector3 dragOffset;
    private bool isDragging => selectedBuilding != null;

    private void Start()
    {
        mainCamera = Camera.main;
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
            float distance;

            if (XZPlane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                Vector3 newPosition = new Vector3(
                    hitPoint.x - dragOffset.x,
                    selectedBuilding.transform.position.y,
                    hitPoint.z - dragOffset.z
                );

                selectedBuilding.transform.position = newPosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedBuilding = null;
            Debug.Log("Drag Off !!!");
        }
    }

    private void HandleSelecting()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingLayer))
        {
            selectedBuilding = hit.transform.gameObject;
            dragOffset = hit.point - selectedBuilding.transform.position;
        }
    }
}