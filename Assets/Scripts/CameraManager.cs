using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    public Camera mainCamera { get; private set; }
    private Stage currentStage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        mainCamera = Camera.main;
        currentStage = GameManager.Instance.currentStage;
        SetMainView();
    }

    private void SetMainView()
    {
        mainCamera.transform.rotation = Quaternion.Euler(45f, -45f, 0f);

    }
}
