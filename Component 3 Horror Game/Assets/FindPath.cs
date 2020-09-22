using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour
{
    public Grid grid;
    public Transform seeker, target;
    public bool hasDestinationBeenFound = false;

    void Update()
    {
        CalculatePath(seeker.position, target.position);
    }

    public void CalculatePath(Vector3 startPosition, Vector3 endPosition) {
        //Figure out the start and end nodes based on their object position
        hasDestinationBeenFound = false;
        AStarNode startNode = grid.NodeFromWorldPoint(startPosition);
        AStarNode endNode = grid.NodeFromWorldPoint(endPosition);
        //First step of the algorithm is to establish the open and closed sets and
        // add the start node to the open set
        List<AStarNode> openSet = new List<AStarNode>();
        HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
        openSet.Add(startNode);
        //Now we need to loop through the open set to find the node with the smallest fCost and set it
        //as the current node
        while (openSet.Count > 0) {
            AStarNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode) {
                RetracePath(startNode, endNode);
                hasDestinationBeenFound = true;
                UnityEngine.Debug.Log("You've found the end position");
                return;
            }

            foreach (AStarNode neighbour in grid.GetNeighbours(currentNode)) {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour)) {
                    continue;
                }
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    public void RetracePath(AStarNode startNode, AStarNode endNode) {
        List<AStarNode> path = new List<AStarNode>();
        AStarNode currentNode = endNode;
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
    }

    int GetDistance(AStarNode nodeA, AStarNode nodeB) {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (distanceX > distanceY) {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
