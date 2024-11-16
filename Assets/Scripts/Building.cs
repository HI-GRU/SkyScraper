using UnityEngine;

public class Building
{
    public int SizeX { get; }
    public int SizeY { get; } // 높이
    public int SizeZ { get; }
    public string BuildingId { get; }

    public Building(int SizeX, int SizeY, int SizeZ, string BuildingId)
    {
        this.SizeX = SizeX;
        this.SizeY = SizeY;
        this.SizeZ = SizeZ;
        this.BuildingId = BuildingId;
    }
}