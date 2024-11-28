using UnityEngine;

public class Building
{
    public bool[,,] shape { get; }
    public string buildingId { get; }

    public Building(bool[,,] shape, string buildingId)
    {
        this.shape = shape;
        this.buildingId = buildingId;
    }

    public bool IsOccupied(int x, int y, int z)
    {
        return shape[x, y, z];
    }

    public class RuntimeData
    {
        public string stageBuildingId { get; }
        public Vector3 originalPosition { get; }
        public bool isPlaced { get; set; }

        public RuntimeData(string stageBuildingId, Vector3 originalPosition)
        {
            this.stageBuildingId = stageBuildingId;
            this.originalPosition = originalPosition;
            this.isPlaced = false;
        }
    }
    public RuntimeData currentData { get; private set; }

    public void SetStageInfo(string stageBuildingId, Vector3 originalPosition)
    {
        currentData = new RuntimeData(stageBuildingId, originalPosition);
    }

    public void ClearStageInfo()
    {
        currentData = null;
    }
}