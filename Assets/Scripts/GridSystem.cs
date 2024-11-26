using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GridSystem
{
    public Vector2 size { get; private set; }
    private Cell[,] grid;

    public GridSystem(Vector2 size)
    {
        this.size = size;
        grid = new Cell[(int)size.x, (int)size.y];

        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                grid[x, y] = new Cell(new Vector2(x, y));
    }

    private bool isBound(Vector3 pos)
    {
        float x = Mathf.Round(pos.x);
        float z = Mathf.Round(pos.z);
        return x >= 0 && x < size.x && z >= 0 && z < size.y;
    }

    public Cell GetCell(Vector3 pos)
    {
        if (!isBound(pos))
        {
            // Debug.Log($"Invalid position: {pos}");
            return null;
        }

        return grid[(int)pos.x, (int)pos.y];
    }
}
