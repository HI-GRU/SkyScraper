using System;
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

        sortBuildings();
    }

    public void sortBuildings()
    {
        Array.Sort(buildings, (a, b) =>
        {
            int heightA = a.shape.GetLength(1);
            int heightB = b.shape.GetLength(1);
            return b.shape.GetLength(1) - a.shape.GetLength(1);
        });
    }
}

public enum Direction
{
    North,
    South,
    East,
    West
}