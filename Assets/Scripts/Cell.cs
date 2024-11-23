using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector2 position { get; private set; }
    public Building building;
    public bool IsOccupied => building != null;

    public Cell(Vector2 position)
    {
        this.position = position;
        building = null;
    }
}
