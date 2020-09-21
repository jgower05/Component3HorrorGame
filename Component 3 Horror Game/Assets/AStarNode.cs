using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class AStarNode
{
    private bool isWalkable;
    private Vector3 worldPosition;

    public bool IsWalkable { get => isWalkable; set => isWalkable = value; }
    public Vector3 WorldPosition { get => worldPosition; set => worldPosition = value; }

    public AStarNode(bool isWalkable, Vector3 worldPosition) {
        this.isWalkable = isWalkable;
        this.worldPosition = worldPosition;
    }
}
