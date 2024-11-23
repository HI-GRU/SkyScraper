using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Game Asset Setting")]
    // [SerializeField] private GameObject groundPlanePrefab; // 에셋으로 들어갈 평면. 실제 기능은 없음
    [SerializeField] private GameObject[] buildingPrefabs;
    private StageData stageData;
    private Stage currentStage;

    private GridSystem gridSystem;
    private GameObject boardObject;

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

        currentStage = stageData.GetStage(level);
        CreateBuildings();

    }

    private void ClearStage()
    {
        foreach (Transform child in boardObject.transform)
        {
            Destroy(child.gameObject);
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
                buildingObj.tag = "Building";
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