using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject stagePanel;

    [Header("Stage Button Settings")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform stageButtonsContainer;

    private void Start()
    {
        CreateStageButtons();
        ShowMainPanel();
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

    private void CreateStageButtons()
    {
        int stageLength = GameManager.Instance.stageData.GetLength();

        for (int i = 1; i <= stageLength; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, stageButtonsContainer);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = $"Stage {i}";
            int stageNumber = i;
            button.onClick.AddListener(() =>
            {
                FindObjectOfType<SceneChanger>().LoadGameScene(stageNumber);
            });
        }
    }
}
