using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class AStarNode
{
    public bool isWalkable;
    public Vector3 worldPosition;
    public int gridX, gridY;

    public int gCost, hCost;
    public AStarNode parent;

    public AStarNode(bool _isWalkable, Vector3 _worldPosition, int _gridX, int _gridY) {
        this.isWalkable = _isWalkable;
        this.worldPosition = _worldPosition;
        this.gridX = _gridX;
        this.gridY = _gridY;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}
