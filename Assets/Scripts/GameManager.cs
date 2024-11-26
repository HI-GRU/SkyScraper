using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("GRU Game Asset Settings")]
    // [SerializeField] private GameObject groundPlanePrefab; // 에셋으로 들어갈 평면. 실제 기능은 없음
    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private GameObject tilePrefab;

    [Header("GRU Layer Settings")]
    [SerializeField] private LayerMask buildingLayer;
    [SerializeField] private LayerMask tileLayer;

    private Camera mainCamera;
    private StageData stageData;

    // getter
    public GameObject[] BuildingPrefabs => buildingPrefabs;
    public GameObject TilePrefab => tilePrefab;

    public LayerMask BuildingLayer => buildingLayer;
    public LayerMask TileLayer => tileLayer;
    public Camera MainCamera => mainCamera;
    public StageData StageData => stageData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        mainCamera = Camera.main;
        stageData = Resources.Load<StageData>("StageData");
        if (stageData == null) Debug.LogError("StageData not found!");
        if (buildingPrefabs == null || buildingPrefabs.Length == 0) Debug.LogError("Building prefabs not assigned!");
    }
}
