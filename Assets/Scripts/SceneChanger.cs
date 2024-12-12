using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    AsyncOperation asyncLoad;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
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
}
