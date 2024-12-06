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

    private Vector3 center;
    private Vector3 cameraPosition => getCameraPos();
    private float angleX = 30F;
    private float radius => Math.Max(currentStage.size.x, Math.Max(currentStage.size.y, currentStage.size.z)) * 2F;
    private float angleY = -45F;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        mainCamera = Camera.main;
        currentStage = GameManager.Instance.currentStage;
        center = new Vector3(currentStage.size.x / 2F, 0F, currentStage.size.z / 2F);
        Debug.Log(center);
    }

    private void Update()
    {
        SetMainView();
    }

    private void SetMainView()
    {
        mainCamera.transform.position = cameraPosition + center;
        mainCamera.transform.LookAt(center);
        mainCamera.orthographicSize = 5F;
    }

    private Vector3 getCameraPos()
    {
        Vector3 res = new Vector3();
        res.y = radius * Mathf.Sin(angleX * Mathf.Deg2Rad);
        res.x = radius * Mathf.Cos(angleX * Mathf.Deg2Rad) * Mathf.Cos(angleY * Mathf.Deg2Rad);
        res.z = radius * Mathf.Cos(angleX * Mathf.Deg2Rad) * Mathf.Sin(angleY * Mathf.Deg2Rad);

        return res;
    }
}
