using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    private static float maxHeight = 50F;
    private Stage[] stages = new Stage[]
   {
       new Stage(
           new Vector3(1, 3, 3),
           new Building[]
           {
               new Building(
                   new bool[1,3,1] {
                    {{true},
                    {true},
                    {true}}
                   },
                   "pink_1.3.1"
               ),
               new Building(
                   new bool[1,2,1] {
                    {{true},
                    {true}}
                   },
                   "green_1.2.1"
               ),
               new Building(
                   new bool[1,1,1] {
                    {{true}}
                   },
                   "blue_1.1.1"
               )
           },
           new Dictionary<Direction, int> {
            { Direction.North, 1 },
            { Direction.South, 3 }
            }
       )
   };

    public Stage GetStage(int level)
    {
        if (level == 0 || level > stages.Length)
        {
            Debug.LogError($"Invalid Level: {level}");
            return null;
        }
        return stages[level - 1];
    }

    public int GetLength()
    {
        return stages.Length;
    }
}
