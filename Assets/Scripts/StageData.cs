using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    private Stage[] stages = new Stage[]
   {
       new Stage(
           new Vector2(1, 3),
           new Building[]
           {
               new Building(
                   new bool[1][][] {
                       new bool[3][] {
                           new bool[] { true },
                           new bool[] { true },
                           new bool[] { true }
                       }
                   },
                   "blue_1.3.1"
               ),
               new Building(
                   new bool[1][][] {
                       new bool[2][] {
                           new bool[] { true },
                           new bool[] { true }
                       }
                   },
                   "green_1.2.1"
               ),
               new Building(
                   new bool[1][][] {
                       new bool[1][] {
                           new bool[] { true }
                       }
                   },
                   "orange_1.1.1"
               )
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
}
