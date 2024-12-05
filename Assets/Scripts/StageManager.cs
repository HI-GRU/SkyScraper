using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    // private GameObject groundPlanePrefab; // 에셋으로 들어갈 평면. 실제 기능은 없음
    private GameObject[] buildingPrefabs;
    private GameObject tilePrefab;
    public Dictionary<string, Building> buildingInfo { get; private set; }

    private StageData stageData;
    private Stage currentStage;
    public GridSystem gridSystem { get; private set; }
    private GameObject boardObject;
    public Camera mainCamera { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        buildingPrefabs = GameManager.Instance.BuildingPrefabs;
        tilePrefab = GameManager.Instance.TilePrefab;
        buildingInfo = new Dictionary<string, Building>();
        stageData = GameManager.Instance.stageData;
        mainCamera = CameraManager.Instance.mainCamera;

        LoadBoard();
        CreateStage(GameManager.Instance.level);
    }

    public void LoadBoard()
    {
        boardObject = new GameObject("Board");
        boardObject.transform.parent = transform;
    }

    public void CreateStage(int level)
    {
        ClearStage();
        LoadStage(level);
        CreateGrid();
        CreateBuildings();
        SetupCamera();
    }

    private void ClearStage()
    {
        if (boardObject != null)
        {
            foreach (Transform child in boardObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (currentStage != null)
        {
            foreach (Building building in currentStage.buildings)
            {
                building.ClearStageInfo();
            }
        }

        buildingInfo.Clear();
    }

    private void LoadStage(int level)
    {
        currentStage = stageData.GetStage(level);
    }

    private void CreateGrid()
    {
        gridSystem = new GridSystem(currentStage.size);

        for (int x = 0; x < gridSystem.size.x; x++)
        {
            for (int z = 0; z < gridSystem.size.z; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject tileObj = Instantiate(tilePrefab, position, Quaternion.identity, boardObject.transform);
                tileObj.name = $"Tile_{x}_{z}";
                tileObj.layer = LayerMask.NameToLayer("Tile");
            }
        }
    }

    private void CreateBuildings()
    {
        if (currentStage == null) return;

        for (int i = 0; i < currentStage.buildings.Length; i++)
        {
            Building building = currentStage.buildings[i];
            GameObject buildingPrefab = GetBuildingPrefab(building.buildingId);

            if (buildingPrefab != null)
            {
                Vector3 position = new Vector3(-2f, 0f, i * 2f);
                GameObject buildingObj = Instantiate(buildingPrefab, position, Quaternion.Euler(90F, 0F, 0F), boardObject.transform);
                buildingObj.name = $"Building_{i}";
                buildingObj.layer = LayerMask.NameToLayer("Building");

                building.SetStageInfo(buildingObj.name, buildingObj.transform.position);
                buildingInfo[buildingObj.name] = building;
            }
        }
    }

    private GameObject GetBuildingPrefab(string buildingId)
    {
        foreach (GameObject prefab in buildingPrefabs)
        {
            if (prefab.name.Equals(buildingId)) return prefab;
        }
        Debug.LogError($"Building prefab not found: {buildingId}");
        return null;
    }

    private void SetupCamera()
    {
        mainCamera.transform.rotation = Quaternion.Euler(45F, 45F, 0F);
    }
}