using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public Vector3 size;
    public Building[] buildings { get; private set; }
    public Dictionary<Direction, int> clearCondition { get; private set; }

    public Stage(Vector3 size, Building[] buildings, Dictionary<Direction, int> clearCondition)
    {
        this.size = size;
        this.buildings = buildings;
        this.clearCondition = clearCondition;
    }
}

public enum Direction
{
    North,
    South,
    East,
    West
}