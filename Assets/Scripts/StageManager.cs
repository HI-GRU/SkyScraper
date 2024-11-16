using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject[] buildingPrefabs; // Inspector에서 직접 넣는 방식
    private StageData stageData;
    private GameObject board;

    private Tile[,] tiles;
    private Building[] buildings;
    private int TileX => tiles?.GetLength(0) ?? 0;
    private int TileZ => tiles?.GetLength(1) ?? 0;

    private void Awake()
    {
        if (stageData == null) stageData = Resources.Load<StageData>("StageData");
        if (stageData == null) Debug.LogError("Unfinded StageData !!!");

        if (buildingPrefabs == null || buildingPrefabs.Length == 0)
        {
            Debug.LogError("Empty Building prefabs !!!");
        }

        // Board Object 생성
        board = new("Board");
        board.transform.parent = gameObject.transform;
    }

    //TODO: 시작 버튼 만들기
    private void Start()
    {
        CreateBoard(1); // 스테이지 테스트
    }

    private void CreateBoard(int level)
    {
        if (level <= 0 || level > stageData.stages.Length)
        {
            Debug.LogError("Invalid Level: {level}");
            return;
        }

        Stage stage = stageData.stages[level - 1];
        ClearBoard();
        GenerateTiles(stage);
        GenerateBuildings(stage);
    }

    private void GenerateTiles(Stage stage)
    {
        tiles = new Tile[stage.SizeX, stage.SizeZ];

        Vector3 tileSize = tilePrefab.transform.localScale;
        float gap = 0;

        for (int x = 0; x < stage.SizeX; x++)
        {
            for (int z = 0; z < stage.SizeZ; z++)
            {
                Vector3 position = new Vector3(x * (tileSize.x + gap), 0, z * (tileSize.z + gap));

                GameObject tileObject = Instantiate(tilePrefab, position, tilePrefab.transform.rotation, board.transform);
                tileObject.name = $"Tile ({x}, {z})";
                tileObject.transform.localScale = new Vector3(1F, 0.1F, 1F);

                Tile tile = tileObject.GetComponent<Tile>();
                tiles[x, z] = tile;
            }
        }
    }

    private void GenerateBuildings(Stage stage)
    {
        buildings = stage.buildings;

        for (int x = 0; x < buildings.Length; x++)
        {
            Building building = buildings[x];

            Vector3 position = new Vector3(x, 0, 0);

            GameObject buildingPrefab = GetBuildingPrefab(building.BuildingId);
            GameObject buildingObject = Instantiate(buildingPrefab, position, buildingPrefab.transform.rotation, board.transform);
            buildingObject.name = $"Building ({x})";
            buildingObject.transform.localScale = new Vector3(1F, 1F, 1F);
        }
    }

    private GameObject GetBuildingPrefab(string buildingId)
    {
        foreach (GameObject buildingPrefab in buildingPrefabs)
        {
            if (buildingPrefab.name.Equals(buildingId)) return buildingPrefab;
        }
        Debug.LogError("Undefined building prefab ID");
        return null;
    }

    private void ClearBoard()
    {
        if (tiles == null) return;

        for (int x = 0; x < TileX; x++)
        {
            for (int z = 0; z < TileZ; z++)
            {
                if (tiles[x, z] != null)
                {
                    Destroy(tiles[x, z].gameObject);
                }
            }
        }

        tiles = null;
    }
}
