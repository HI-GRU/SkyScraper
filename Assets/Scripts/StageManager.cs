using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    // private GameObject groundPlanePrefab; // 에셋으로 들어갈 평면. 실제 기능은 없음
    private GameObject[] buildingPrefabs;
    private GameObject tilePrefab;
    private Dictionary<string, Vector3> originalBuildingPositions;

    private StageData stageData;
    private Stage currentStage;
    private GridSystem gridSystem;
    private GameObject boardObject;

    public GridSystem GridSystem => gridSystem;
    public Dictionary<string, Vector3> OriginalBuildingPositions => originalBuildingPositions;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        buildingPrefabs = GameManager.Instance.BuildingPrefabs;
        tilePrefab = GameManager.Instance.TilePrefab;
        originalBuildingPositions = new Dictionary<string, Vector3>();
        stageData = GameManager.Instance.StageData;

        LoadBoard();
    }

    private void LoadBoard()
    {
        boardObject = new GameObject("Board");
        boardObject.transform.parent = transform;
        CreateStage(1);
    }

    private void CreateStage(int level)
    {
        ClearStage();
        currentStage = LoadStage(level);
        CreateGrid();
        CreateBuildings();
    }

    private void ClearStage()
    {
        foreach (Transform child in boardObject.transform)
        {
            Destroy(child.gameObject);
        }
        originalBuildingPositions.Clear();
    }

    private Stage LoadStage(int level)
    {
        return stageData.GetStage(level);
    }

    private void CreateGrid()
    {
        gridSystem = new GridSystem(currentStage.size);

        for (int x = 0; x < gridSystem.size.x; x++)
        {
            for (int y = 0; y < gridSystem.size.y; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                GameObject tileObj = Instantiate(tilePrefab, position, Quaternion.identity, boardObject.transform);
                tileObj.name = $"Tile_{x}_{y}";
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
            GameObject buildingPrefab = GetBuildingPrefab(building.BuildingId);

            if (buildingPrefab != null)
            {
                Vector3 position = new Vector3(-2f, 0f, i * 2f);
                GameObject buildingObj = Instantiate(buildingPrefab, position, Quaternion.Euler(90F, 0F, 0F), boardObject.transform);
                buildingObj.name = $"Building_{i}";
                buildingObj.layer = LayerMask.NameToLayer("Building");

                originalBuildingPositions[buildingObj.name] = buildingObj.transform.position;
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

    public Vector3 GetOriginalPosition(string id)
    {
        return originalBuildingPositions[id];
    }
}