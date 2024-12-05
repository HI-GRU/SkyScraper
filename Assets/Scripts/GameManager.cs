using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int level { get; private set; }

    [Header("GRU Game Asset Settings")]
    // [SerializeField] private GameObject groundPlanePrefab; // 에셋으로 들어갈 평면. 실제 기능은 없음
    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private GameObject tilePrefab;

    [Header("GRU Layer Settings")]
    [SerializeField] private LayerMask buildingLayer;
    [SerializeField] private LayerMask tileLayer;

    public StageData stageData { get; private set; }

    public GameObject[] BuildingPrefabs => buildingPrefabs;
    public GameObject TilePrefab => tilePrefab;

    public LayerMask BuildingLayer => buildingLayer;
    public LayerMask TileLayer => tileLayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        stageData = Resources.Load<StageData>("StageData");
        if (stageData == null) Debug.LogError("StageData not found!");
        if (buildingPrefabs == null || buildingPrefabs.Length == 0) Debug.LogError("Building prefabs not assigned!");
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }
}
