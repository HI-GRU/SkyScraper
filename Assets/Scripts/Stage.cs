using UnityEngine;

public class Stage
{
    public Vector3 size;
    public Building[] buildings { get; private set; }

    public Stage(Vector3 size, Building[] buildings)
    {
        this.size = size;
        this.buildings = buildings;
    }
}