using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    private StageData stageData;

    private Tile[,] tiles;
    private Building[] buildings;
    private int TileX => tiles?.GetLength(0) ?? 0;
    private int TileZ => tiles?.GetLength(1) ?? 0;

    private void Awake()
    {
        if (stageData == null) stageData = Resources.Load<StageData>("StageData");
        if (stageData == null) Debug.LogError("Unfinded StageData");
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
        GenerateBoard(stage);
        GenerateBuildings(stage);
    }

    private void GenerateBoard(Stage stage)
    {
        tiles = new Tile[stage.SizeX, stage.SizeZ];
        GameObject board = new("Board");
        board.transform.parent = gameObject.transform;

        Vector3 tileSize = tilePrefab.transform.localScale;
        float gap = 0;

        for (int x = 0; x < stage.SizeX; x++)
        {
            for (int z = 0; z < stage.SizeZ; z++)
            {
                Vector3 position = new Vector3(x * (tileSize.x + gap), 0, z * (tileSize.z + gap));

                GameObject tileObject = Instantiate(tilePrefab, position, tilePrefab.transform.rotation, board.transform);
                tileObject.name = $"Tile ({x}, {z})";
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
            GameObject buildingObject = Instantiate(buildingPrefab, position, buildingPrefab.transform.rotation);
            buildingObject.name = $"Building ({x})";

            
        }
    }

    private GameObject GetBuildingPrefab(string buildingId)
    {
        return Resources.Load<GameObject>($"Prefabs/Buildings/{buildingId}");
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
