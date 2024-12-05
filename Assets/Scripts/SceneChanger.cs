using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject stagePanel;
    [SerializeField] private GameObject buttonPrefab;
    AsyncOperation asyncLoad;
    private int stageLength;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        stageLength = GameManager.Instance.stageData.GetLength();

        // 스테이지 버튼들을 담을 새로운 패널 생성
        GameObject stageButtonsPanel = new GameObject("StageButtonsPanel");
        stageButtonsPanel.transform.SetParent(stagePanel.transform, false);

        // 스테이지 버튼 패널의 RectTransform 설정
        RectTransform stageButtonsRect = stageButtonsPanel.AddComponent<RectTransform>();
        stageButtonsRect.anchorMin = new Vector2(0.2f, 0.2f);
        stageButtonsRect.anchorMax = new Vector2(0.8f, 0.8f);
        stageButtonsRect.sizeDelta = Vector2.zero;

        // 스테이지 버튼들의 그리드 레이아웃 설정
        GridLayoutGroup layoutGroup = stageButtonsPanel.AddComponent<GridLayoutGroup>();
        layoutGroup.cellSize = new Vector2(200, 100);
        layoutGroup.spacing = new Vector2(20, 20);
        layoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        layoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;

        // 스테이지 버튼 생성
        CreateStageButtons(stageButtonsPanel.transform);

        // 뒤로가기 버튼 생성
        GameObject backButton = Instantiate(buttonPrefab, stagePanel.transform);
        backButton.name = "BackButton";
        RectTransform backButtonRect = backButton.GetComponent<RectTransform>();
        backButtonRect.anchorMin = new Vector2(0, 1);
        backButtonRect.anchorMax = new Vector2(0, 1);
        backButtonRect.pivot = new Vector2(0, 1);
        backButtonRect.anchoredPosition = new Vector2(50, -50);

        // 뒤로가기 버튼 설정
        Button backButtonComp = backButton.GetComponent<Button>();
        TextMeshProUGUI backButtonText = backButton.GetComponentInChildren<TextMeshProUGUI>();
        backButtonText.text = "Back";
        backButtonComp.onClick.AddListener(ShowMainPanel);
    }

    private void CreateStageButtons(Transform parent)
    {
        for (int i = 1; i <= stageLength; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, parent);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = $"Stage {i}";
            int stageNumber = i;
            button.onClick.AddListener(() => LoadGameScene(stageNumber));
        }
    }

    public async void LoadMainScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync("MainScene");

        while (!asyncLoad.isDone)
        {
            await System.Threading.Tasks.Task.Yield();
        }
    }

    public async void LoadGameScene(int level)
    {
        GameManager.Instance.SetLevel(level);
        asyncLoad = SceneManager.LoadSceneAsync("GameScene");

        while (!asyncLoad.isDone)
        {
            await System.Threading.Tasks.Task.Yield();
        }
    }

    public void ShowStagePanel()
    {
        mainPanel.SetActive(false);
        stagePanel.SetActive(true);
    }

    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        stagePanel.SetActive(false);
    }
}
