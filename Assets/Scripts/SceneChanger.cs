using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject stagePanel;
    AsyncOperation asyncLoad;

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
