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

    public bool isBound(float x, float z)
    {
        float roundedX = Mathf.Round(x);
        float roundedZ = Mathf.Round(z);
        return roundedX >= 0 && roundedX < size.x && roundedZ >= 0 && roundedZ < size.y;
    }

    public Cell GetCell(float x, float z)
    {
        if (!isBound(x, z))
        {
            return null;
        }

        return grid[(int)x, (int)z];
    }

    public bool CanPlaceBuilding(Vector3 position, Building building)
    {
        bool[,,] shape = building.shape;

        for (int x = 0; x < shape.GetLength(0); x++)
        {
            for (int z = 0; z < shape.GetLength(2); z++)
            {
                float nx = position.x + x;
                float nz = position.z + z;

                if (!isBound(nx, nz)) return false;
                if (!shape[(int)nx, 0, (int)nz]) continue;
                if (GetCell(nx, nz).building != null) return false;
            }
        }

        return true;
    }
}
