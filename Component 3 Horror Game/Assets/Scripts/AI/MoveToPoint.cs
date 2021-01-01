using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint : Node
{
    private Vector3 target;
    private Transform enemy;
    private float enemySpeed;
    private WaypointIdentifier waypointIdentifier;
    private FindPath pathfinder;

    private List<AStarNode> newPath;
    private bool reachedNewDestination;

    public MoveToPoint(Vector3 _target, Transform _enemy, float _enemySpeed, WaypointIdentifier _waypointIdentifier, FindPath _pathfinder) {
        target = _target;
        enemy = _enemy;
        enemySpeed = _enemySpeed;
        waypointIdentifier = _waypointIdentifier;
        pathfinder = _pathfinder;
    }

    public override NodeStates Evaluate() {
        if (enemy.position == target)
        {
            return NodeStates.SUCCESS;
        }
        else {
            newPath = RequestPath(enemy.position, target);
            MoveAlongPath();
            return NodeStates.RUNNING; 
        }
    }

    //Handles the movement of the enemy unit through the nodes on the A* grid.
    public void MoveAlongPath()
    {
        foreach (AStarNode pathNodes in newPath)
        {
            Vector3 newPos = new Vector3(pathNodes.worldPosition.x, enemy.position.y, pathNodes.worldPosition.z);
            enemy.position = Vector3.MoveTowards(enemy.position, newPos, enemySpeed * Time.deltaTime);
        }
        reachedNewDestination = true;
    }

    //Requests a path from the A* algorithm and stores it in a list of nodes.
    public List<AStarNode> RequestPath(Vector3 startPos, Vector3 endPos)
    {
        pathfinder.CalculatePath(startPos, endPos);
        return pathfinder.path;
    }
}
