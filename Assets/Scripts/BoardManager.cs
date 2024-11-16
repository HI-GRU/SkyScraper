using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    private StageData stageData;

    private Tile[,] tiles;
    private int height => tiles?.GetLength(0) ?? 0;
    private int width => tiles?.GetLength(1) ?? 0;

    private void Awake()
    {
        if (stageData == null)
        {
            stageData = Resources.Load<StageData>("StageData");
        }

        if (stageData == null)
        {
            Debug.LogError("Unfinded StageData");
        }
    }

    //TODO: 시작 버튼 만들기
    private void Start()
    {
        CreateBoard(3); // 1스테이지 테스트
    }

    public void CreateBoard(int level)
    {
        if (level <= 0 || level > stageData.stages.Length)
        {
            Debug.LogError("Invalid Level: {level}");
            return;
        }

        Stage stage = stageData.stages[level - 1];
        GenerateBoard(stage);
    }

    private void GenerateBoard(Stage stage)
    {
        ClearBoard();
        tiles = new Tile[stage.height, stage.width];
        GameObject board = new("Board");
        board.transform.parent = gameObject.transform;

        // TODO: 크기 및 간격 조절 메소드 수정 필요
        // SpriteRenderer spriteRenderer = tilePrefab.GetComponent<SpriteRenderer>();
        // if (spriteRenderer == null)
        // {
        //     Debug.LogError("tilePrefab에 spriteRenderer가 없습니다.");
        //     return;
        // }
        // Vector3 tileSize = spriteRenderer.bounds.size;
        // float gap = tileSize.x * 0.1F;

        Vector3 tileSize = new Vector3(1, 1, 1);
        float gap = 0;

        for (int z = 0; z < stage.height; z++)
        {
            for (int x = 0; x < stage.width; x++)
            {
                Vector3 position = new Vector3(x * (tileSize.x + gap), 0, z * (tileSize.z + gap));

                GameObject tileObject = Instantiate(tilePrefab, position, tilePrefab.transform.rotation, board.transform);
                tileObject.name = $"Tile ({x}, {z})";
                Tile tile = tileObject.GetComponent<Tile>();
                tiles[z, x] = tile;
            }
        }
    }

    private void ClearBoard()
    {
        if (tiles == null) return;

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (tiles[z, x] != null)
                {
                    Destroy(tiles[z, x].gameObject);
                }
            }
        }

        tiles = null;
    }
}
