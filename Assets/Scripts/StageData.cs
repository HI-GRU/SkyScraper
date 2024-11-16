using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    public Stage[] stages = new Stage[] {
        new(
            1,
            3,
            new Building[] {
                new Building(1, 3, 1, "blue_1_3_1"),
                new Building(1, 2, 1, "green_1_2_1"),
                new Building(1, 1, 1, "orange_1_1_1"),
            },
            "ex_stage-1"
            ),
        // new(
        //     1,
        //     5,
        //     new Building[] {
        //         new Building(1, 3, 1),
        //         new Building(1, 2, 1),
        //         new Building(1, 1, 1),
        //     },
        //     "ex_stage-2"
        //     ),
        // new(
        //     2,
        //     4,
        //     new Building[] {
        //         new Building(1, 3, 1),
        //         new Building(1, 2, 1),
        //         new Building(1, 1, 1),
        //     },
        //     "ex_stage-3"
        //     ),
    };
}
