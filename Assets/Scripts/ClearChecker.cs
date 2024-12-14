using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearChecker
{
    private Stage stage;
    private GridSystem gridSystem;
    private int count;
    private int maxHeight;
    private bool[] isCounted;

    public ClearChecker(Stage stage, GridSystem gridSystem)
    {
        this.stage = stage;
        this.gridSystem = gridSystem;
    }

    public bool CheckClear()
    {
        foreach (var condition in stage.clearCondition)
        {
            isCounted = new bool[stage.buildings.Length];
            count = 0;
            CountVisibleBuildings(condition.Key);
            if (count != condition.Value) return false;
        }

        return true;
    }

    private void CountVisibleBuildings(Direction direction)
    {
        Vector3 size = gridSystem.size;

        bool isNS = direction == Direction.North || direction == Direction.South;
        bool isNE = direction == Direction.North || direction == Direction.East;

        int firstLen = isNS ? (int)size.x : (int)size.z;
        int secondLen = isNS ? (int)size.z : (int)size.x;

        for (int f = 0; f < firstLen; f++)
        {
            maxHeight = -1;

            for (int s = isNE ? secondLen - 1 : 0; isNE ? s >= 0 : s < secondLen; s += isNE ? -1 : 1)
            {
                int x = isNS ? f : s;
                int z = isNS ? s : f;

                int height = GetHeight(x, z);
                if (height > maxHeight) maxHeight = height;
            }
        }
    }

    private int GetHeight(int x, int z)
    {
        int height = -1;
        for (int y = 0; y < gridSystem.size.y; y++)
        {
            Cell cell = gridSystem.GetCell(x, y, z);
            if (cell == null || !cell.IsOccupied) break;
            height = y;

            int seq = cell.building.currentData.buttonNumber;

            if (height <= maxHeight) continue;
            if (isCounted[seq]) continue;
            isCounted[seq] = true;
            count++;
        }

        return height;
    }

    // public void DebugCheck()
    // {
    //     foreach (var condition in stage.clearCondition)
    //     {
    //         isCounted = new bool[stage.buildings.Length];
    //         count = 0;
    //         CountVisibleBuildings(condition.Key);
    //         Debug.Log(condition.Key + " " + count);
    //     }
    // }
}
