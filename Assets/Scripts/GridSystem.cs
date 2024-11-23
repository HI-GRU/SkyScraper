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

    private bool isBound(Vector2 pos)
    {
        return pos.x >= 0 && pos.x < size.x && pos.y >= 0 && pos.y <= size.y;
    }

    public Cell GetCell(Vector2 pos)
    {
        if (!isBound(pos))
        {
            Debug.Log("Invalid position: {pos}");
            return null;
        }

        return grid[(int)pos.x, (int)pos.y];
    }
}
