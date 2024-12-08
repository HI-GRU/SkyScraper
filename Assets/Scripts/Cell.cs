using UnityEngine;

public class Cell
{
    public Vector3 position { get; private set; }
    public Building building { get; private set; }
    public bool IsOccupied => building != null;

    public Cell(Vector3 position)
    {
        this.position = position;
        building = null;
    }

    public void SetBuilding(Building building)
    {
        this.building = building;
    }

    public void RemoveBuilding()
    {
        this.building = null;
    }
}
