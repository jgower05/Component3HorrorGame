using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class AStarNode : IHeapItem<AStarNode>
{
    public bool isWalkable;
    public Vector3 worldPosition;
    public int gridX, gridY;

    public int gCost, hCost;
    public AStarNode parent;
    int heapIndex;

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

    public int HeapIndex
    {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }
    public int CompareTo(AStarNode nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
