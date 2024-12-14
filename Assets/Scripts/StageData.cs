using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    private Stage[] stages = new Stage[]
   {
       new Stage(
           new Vector3(1, 3, 3),
           new Building[]
           {
               new Building(
                   new bool[1,3,1] {
                    {{true}, {true}, {true}}
                   },
                   "mansion_1.3.1"
               ),
               new Building(
                   new bool[1,2,1] {
                    {{true}, {true}}
                   },
                   "villa_1.2.1"
               ),
               new Building(
                   new bool[1,1,1] {
                    {{true}}
                   },
                   "house_1.1.1"
               )
           },
           new Dictionary<Direction, int>
           {
            { Direction.North, 1 },
            { Direction.South, 3 }
            }
       ) ,
        new Stage(
           new Vector3(2, 5, 2),
           new Building[]
           {
               new Building(
                   new bool[1,3,1] {
                    {{true}, {true}, {true}}
                   },
                   "mansion_1.3.1"
               ),
               new Building(
                   new bool[1,5,1] {
                    {{true}, {true}, {true}, {true}, {true}}
                   },
                   "empire_1.5.1"
               ),
               new Building(
                    new bool[2,2,1] { { {true}, {true} }, { {true}, {true} } },
                    "mall_2.2.1"
               ),
           },
           new Dictionary<Direction, int>
           {
            { Direction.North, 1 },
            { Direction.South, 2 }
            }
       ),
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
