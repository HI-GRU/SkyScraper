using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject buildingButtonPrefab;

    private GameObject[] buildingPrefabs;
    private GameObject tilePrefab;
    public Dictionary<string, Building> buildingInfo { get; private set; }
    public Dictionary<int, GameObject> buildingButtonObjs;
    private Stage currentStage;
    public GridSystem gridSystem { get; private set; }
    private GameObject boardObject;
    private ClearChecker clearChecker;
    public Camera mainCamera { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        buildingPrefabs = GameManager.Instance.BuildingPrefabs;
        tilePrefab = GameManager.Instance.TilePrefab;
        buildingInfo = new Dictionary<string, Building>();
        buildingButtonObjs = new Dictionary<int, GameObject>();
        mainCamera = CameraManager.Instance.mainCamera;

        LoadBoard();
        CreateStage();

        clearChecker = new ClearChecker(currentStage, gridSystem);
    }

    public void LoadBoard()
    {
        boardObject = new GameObject("Board");
        boardObject.transform.parent = transform;
    }

    public void CreateStage()
    {
        ClearStage();
        LoadStage();
        CreateGrid();
        CreateBuildings();
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

    private void LoadStage()
    {
        currentStage = GameManager.Instance.currentStage;
    }

    private void CreateGrid()
    {
        gridSystem = new GridSystem(currentStage.size);

        for (int x = 0; x < gridSystem.size.x; x++)
        {
            for (int z = 0; z < gridSystem.size.z; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject tileObj = Instantiate(tilePrefab, position, Quaternion.Euler(-90, 0, 0), boardObject.transform);
                tileObj.name = $"Tile_{x}_{z}";
                tileObj.layer = LayerMask.NameToLayer("Tile");
                CreateCollider(tileObj);
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
                Vector3 position = new Vector3(0F, 0F, i * 2F);
                GameObject buildingObj = Instantiate(buildingPrefab, position, Quaternion.Euler(-90, 0, 0), boardObject.transform);
                buildingObj.name = $"Building_{i}";
                buildingObj.layer = LayerMask.NameToLayer("Building");
                CreateCollider(buildingObj);

                // 현재 스테이지 Runtime Data 설정
                building.SetStageInfo(buildingObj.name, buildingObj.transform.position, i);
                buildingInfo[buildingObj.name] = building;

                buildingObj.SetActive(false);

                // building button 생성
                GameObject buttonObj = Instantiate(buildingButtonPrefab, content);
                buttonObj.name = $"BuildingButton_{i}";
                buildingButtonObjs[i] = buttonObj;

                BuildingButtonHandler buttonHandler = buttonObj.GetComponent<BuildingButtonHandler>();
                buttonHandler.Initialize(buildingObj);

                RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
                float size = content.rect.height;
                buttonRect.sizeDelta = new Vector2(size, size);

                // TODO: 빌딩 그림으로 변경
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = building.currentData.stageBuildingId;
                }
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

    private void CreateCollider(GameObject obj)
    {
        BoxCollider collider = obj.AddComponent<BoxCollider>();
    }

    public void CheckClearCondition()
    {
        bool isCleared = clearChecker.CheckClear();

        if (isCleared)
        {

            GameUIManager.Instance.ShowClearPanel();
        }

        // clearChecker.DebugCheck();
    }
}