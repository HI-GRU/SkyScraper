using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject clearPanel;

    public static GameUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ShowGamePanel()
    {
        gamePanel.SetActive(true);
        buttonPanel.SetActive(true);
        clearPanel.SetActive(false);
    }

    public void ShowClearPanel()
    {
        gamePanel.SetActive(false);
        buttonPanel.SetActive(false);
        clearPanel.SetActive(true);
    }
}
