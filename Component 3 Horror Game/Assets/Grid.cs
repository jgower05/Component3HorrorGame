using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    AStarNode[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start() {
        nodeDiameter = nodeRadius;
        //Calculate the X and Y of the grid so that the number of nodes perfectly fits in
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    //Generates grid of nodes based on these values
    void CreateGrid() {
        grid = new AStarNode[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                //Calculate the position of each node
                Vector3 worldPoint = bottomLeft + Vector3.right * (i * nodeRadius + nodeDiameter) + Vector3.forward * (j * nodeRadius + nodeDiameter);
                //Check if any of the nodes are colliding with objects that are unwalkable.
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[i, j] = new AStarNode(walkable, worldPoint);
            }
        }
    }

    public AStarNode NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null) {
            AStarNode playerNode = NodeFromWorldPoint(player.position);
            foreach (AStarNode node in grid) {
                //Colour each node either white (walkable)or red (unwalkable) 
                Gizmos.color = (node.IsWalkable) ? Color.white : Color.red;
                if (node.WorldPosition == playerNode.WorldPosition) {
                    Gizmos.color = Color.green;
                }
                //Then draw each cube in the editor based on its position
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
