using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    public Stage[] stages = new Stage[] {
        new Stage(3, 1, "ex_stage-1"),
        new Stage(5, 1, "ex_stage-2"),
        new Stage(4, 2, "ex_stage-3"),
    };
}

public struct Stage
{
    public int height;
    public int width;
    public string stageName;

    public Stage(int height, int width, string stageName)
    {
        this.height = height;
        this.width = width;
        this.stageName = stageName;
    }
}
