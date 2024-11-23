using UnityEngine;

public class Stage
{
    public Vector2 size;
    public Building[] buildings { get; private set; }

    public Stage(Vector2 size, Building[] buildings)
    {
        this.size = size;
        this.buildings = buildings;
    }
}