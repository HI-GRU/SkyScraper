using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GridSystem
{
    public Vector3 size { get; private set; }
    private Cell[,,] grid;

    public GridSystem(Vector3 size)
    {
        this.size = size;
        grid = new Cell[(int)size.x, (int)size.y, (int)size.z];

        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                for (int z = 0; z < size.z; z++)
                    grid[x, y, z] = new Cell(new Vector3(x, y, z));
    }

    public bool isBound(float x, float z)
    {
        int roundedX = Mathf.RoundToInt(x);
        int roundedZ = Mathf.RoundToInt(z);
        return roundedX >= 0 && roundedX < size.x && roundedZ >= 0 && roundedZ < size.z;
    }

    public Cell GetCell(float x, float y, float z)
    {
        if (!isBound(x, z))
        {
            return null;
        }

        return grid[(int)x, (int)y, (int)z];
    }

    public bool CanPlaceBuilding(Vector3 position, Building building)
    {
        bool[,,] shape = building.shape;
        int roundedX = Mathf.RoundToInt(position.x);
        int roundedZ = Mathf.RoundToInt(position.z);

        for (int x = 0; x < shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.GetLength(1); y++)
            {
                for (int z = 0; z < shape.GetLength(2); z++)
                {
                    int nx = roundedX + x;
                    int nz = roundedZ + z;

                    if (!isBound(nx, nz)) return false;
                    if (!shape[x, y, z]) continue;
                    if (grid[nx, y, nz].IsOccupied) return false;
                }
            }
        }
        return true;
    }

    public bool PlaceBuilding(Vector3 position, Building building)
    {
        if (!CanPlaceBuilding(position, building)) return false;

        bool[,,] shape = building.shape;
        int roundedX = Mathf.RoundToInt(position.x);
        int roundedZ = Mathf.RoundToInt(position.z);

        for (int x = 0; x < shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.GetLength(1); y++)
            {
                for (int z = 0; z < shape.GetLength(2); z++)
                {
                    int nx = roundedX + x;
                    int nz = roundedZ + z;

                    if (!shape[x, y, z]) continue;
                    grid[nx, y, nz].SetBuilding(building);
                }
            }
        }
        building.currentData.isPlaced = true;
        return true;
    }

    public void RemoveBuilding(Building building)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    if (grid[x, y, z].IsOccupied && grid[x, y, z].building.buildingId.Equals(building.buildingId))
                    {
                        grid[x, y, z].RemoveBuilding();
                    }
                }
            }
        }
        building.currentData.isPlaced = false;
    }
}
