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
    private StageData stageData;

    private void Start()
    {
        stageData = GameManager.Instance.stageData;
        if (stageData == null)
        {
            Debug.Log("Empty stage data");
            return;
        }

        GridLayoutGroup layoutGroup = stagePanel.AddComponent<GridLayoutGroup>();
        layoutGroup.cellSize = new Vector2(200, 100);
        layoutGroup.spacing = new Vector2(20, 20);
        layoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        layoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;

        CreateStageButton();
    }

    private void CreateStageButton()
    {
        for (int i = 1; i <= stageData.GetLength(); i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, stagePanel.transform);
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
