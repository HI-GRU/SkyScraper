using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 20f;
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private LayerMask buildingLayer;
    [SerializeField] private LayerMask tileLayer;

    private GameObject selectedBuilding;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isDragging = false;
    private bool canPlace = false;
    private const float worldPosZ = 10f;

    private void Update()
    {
        if (!isDragging)
        {
            HandleBuildingSelection();
        }
        else
        {
            HandleDragging();
            HandleRotation();
            CheckPlacement();
        }
    }

    private void HandleBuildingSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildingLayer))
        {
            selectedBuilding = hit.collider.gameObject;
            isDragging = true;
            originalPosition = selectedBuilding.transform.position;
            originalRotation = selectedBuilding.transform.rotation;

            // 드래그 시작할 때 건물을 살짝 들어올림
            Vector3 liftedPosition = originalPosition + Vector3.up * 0.5f;
            selectedBuilding.transform.position = liftedPosition;
        }
    }

    private void HandleDragging()
    {
        if (!Input.GetMouseButton(0))
        {
            FinalizePlacement();
            return;
        }

        Vector3 screenPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, worldPosZ));

        // 그리드에 스냅
        worldPos.x = Mathf.Round(worldPos.x);
        worldPos.z = Mathf.Round(worldPos.z);

        selectedBuilding.transform.position = Vector3.Lerp(
            selectedBuilding.transform.position,
            worldPos,
            Time.deltaTime * dragSpeed
        );
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            selectedBuilding.transform.Rotate(Vector3.up, rotationAngle);
            CheckPlacement(); // 회전 후 배치 가능 여부 다시 체크
        }
    }

    private void CheckPlacement()
    {
        canPlace = true;
        Bounds buildingBounds = selectedBuilding.GetComponent<Collider>().bounds;

        // 건물 크기만큼 그리드 체크
        Vector3 size = buildingBounds.size;
        Vector3 center = buildingBounds.center;

        // 충돌 체크
        Collider[] colliders = Physics.OverlapBox(center, size / 2, selectedBuilding.transform.rotation, buildingLayer);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != selectedBuilding)
            {
                canPlace = false;
                SetBuildingColor(Color.red);
                return;
            }
        }

        // 타일 위치 체크
        Ray ray = new Ray(center, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, tileLayer))
        {
            SetBuildingColor(Color.green);
        }
        else
        {
            canPlace = false;
            SetBuildingColor(Color.red);
        }
    }

    private void FinalizePlacement()
    {
        if (canPlace)
        {
            // 최종 위치에 배치
            Vector3 finalPos = selectedBuilding.transform.position;
            finalPos.y = 0; // 바닥에 맞춤
            selectedBuilding.transform.position = finalPos;
        }
        else
        {
            // 원래 위치로 복귀
            selectedBuilding.transform.position = originalPosition;
            selectedBuilding.transform.rotation = originalRotation;
        }

        SetBuildingColor(Color.white);
        selectedBuilding = null;
        isDragging = false;
    }

    private void SetBuildingColor(Color color)
    {
        Renderer renderer = selectedBuilding.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            material.color = color;
        }
    }
}