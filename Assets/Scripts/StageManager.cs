using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("GRU Game Asset Setting")]
    // [SerializeField] private GameObject groundPlanePrefab; // 에셋으로 들어갈 평면. 실제 기능은 없음
    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private GameObject tilePrefab;
    private StageData stageData;
    private Stage currentStage;

    private GridSystem gridSystem;
    private GameObject boardObject;

    private const string buildingTag = "Building";
    private const string tileTag = "Tile";

    private void Awake()
    {
        LoadStageData();
        LoadBoard();
    }

    private void LoadStageData()
    {
        stageData = Resources.Load<StageData>("StageData");
        if (stageData == null) Debug.LogError("StageData not found!");

        if (buildingPrefabs == null || buildingPrefabs.Length == 0)
            Debug.LogError("Building prefabs not assigned!");
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
                tileObj.layer = LayerMask.NameToLayer(tileTag);
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
                buildingObj.layer = LayerMask.NameToLayer(buildingTag);
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

}