using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public Camera mainCamera { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        mainCamera = Camera.main;
    }
}
