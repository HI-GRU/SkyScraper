using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    public Stage[] stages = new Stage[] {
        new Stage(1, 3, "ex_stage-1"),
        new Stage(1, 5, "ex_stage-2"),
        new Stage(2, 4, "ex_stage-3"),
    };
}
