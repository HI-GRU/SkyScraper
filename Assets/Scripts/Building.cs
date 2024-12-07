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
        public int buttonNumber { get; }
        public bool isPlaced { get; set; }

        public RuntimeData(string stageBuildingId, Vector3 originalPosition, int buttonNumber)
        {
            this.stageBuildingId = stageBuildingId;
            this.originalPosition = originalPosition;
            this.buttonNumber = buttonNumber;
            this.isPlaced = false;
        }
    }
    public RuntimeData currentData { get; private set; }

    public void SetStageInfo(string stageBuildingId, Vector3 originalPosition, int buttonNumber)
    {
        currentData = new RuntimeData(stageBuildingId, originalPosition, buttonNumber);
    }

    public void ClearStageInfo()
    {
        currentData = null;
    }
}