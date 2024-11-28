using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    private static float maxHeight = 100F;
    private Stage[] stages = new Stage[]
   {
       new Stage(
           new Vector3(1, maxHeight, 3),
           new Building[]
           {
               new Building(
                   new bool[1,3,1] {
                    {{true},
                    {true},
                    {true}}
                   },
                   "blue_1.3.1"
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
