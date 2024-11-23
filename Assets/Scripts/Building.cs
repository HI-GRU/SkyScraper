using UnityEngine;

public class Building
{
    private bool[][][] shape;
    public string BuildingId { get; private set; }

    public Building(bool[][][] shape, string BuildingId)
    {
        this.shape = shape;
        this.BuildingId = BuildingId;
    }

    public bool IsOccupied(int x, int y, int z)
    {
        return shape[x][y][z];
    }
}