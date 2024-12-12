using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearChecker
{
    private Stage stage;
    private GridSystem gridSystem;

    public ClearChecker(Stage stage, GridSystem gridSystem)
    {
        this.stage = stage;
        this.gridSystem = gridSystem;
    }

    public bool CheckClear()
    {
        foreach (var condition in stage.clearCondition)
        {
            int cnt = CountVisibleBuildings(condition.Key);
            if (cnt != condition.Value) return false;
        }

        return true;
    }

    public void DebugCheck()
    {
        foreach (var condition in stage.clearCondition)
        {
            int cnt = CountVisibleBuildings(condition.Key);
        }
    }

    private int CountVisibleBuildings(Direction direction)
    {
        Vector3 size = gridSystem.size;
        int count = 0;

        bool isNS = direction == Direction.North || direction == Direction.South;
        bool isNE = direction == Direction.North || direction == Direction.East;

        int firstLen = isNS ? (int)size.x : (int)size.z;
        int secondLen = isNS ? (int)size.z : (int)size.x;

        for (int f = 0; f < firstLen; f++)
        {
            int maxHeight = -1;

            for (int s = isNE ? secondLen - 1 : 0; isNE ? s >= 0 : s < secondLen; s += isNE ? -1 : 1)
            {
                int x = isNS ? f : s;
                int z = isNS ? s : f;

                int height = GetBuildingHeight(x, z);
                if (height > maxHeight)
                {
                    count++;
                    maxHeight = height;
                }
            }
        }

        Debug.Log(direction + " " + count);

        return count;
    }

    private int GetBuildingHeight(int x, int z)
    {
        int height = -1;
        for (int y = 0; y < gridSystem.size.y; y++)
        {
            Cell cell = gridSystem.GetCell(x, y, z);
            if (cell != null && cell.IsOccupied) height = y;
            else break;
        }
        return height;
    }
}
