using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        float maxLen = Math.Max(currentStage.size.x, Math.Max(currentStage.size.y, currentStage.size.z));
        Vector3 centerPosition = new Vector3(
            currentStage.size.x / 2F,
            0F,
            currentStage.size.z / 2F
        );
        mainCamera.transform.rotation = Quaternion.Euler(30F, -45F, 0F);
        mainCamera.transform.position = new Vector3(maxLen, maxLen, -maxLen) * 2 + centerPosition;
    }
}
