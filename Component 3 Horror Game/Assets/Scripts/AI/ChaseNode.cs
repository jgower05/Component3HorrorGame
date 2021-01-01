using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : Node
{
    private FindPath pathFinder;
    private Transform target;
    private Transform enemyUnit;
    private float enemySpeed;
    private LineOfSight los;

    private List<AStarNode> oldPath, newPath;
    private bool reachedNewDestination;

    public ChaseNode(FindPath pathFinder, Transform target, Transform enemyUnit, float enemySpeed, LineOfSight los) {
        this.PathFinder = pathFinder;
        this.Target = target;
        this.enemyUnit = enemyUnit;
        this.EnemySpeed = enemySpeed;
        this.Los = los;
    }

    public FindPath PathFinder { get => pathFinder; set => pathFinder = value; }
    public Transform Target { get => target; set => target = value; }
    public float EnemySpeed { get => enemySpeed; set => enemySpeed = value; }
    public LineOfSight Los { get => los; set => los = value; }

    public override NodeStates Evaluate() {
        float distance = Vector3.Distance(enemyUnit.position, los.visibleTargets[0].position);
        if (los.visibleTargets != null && distance > 0.2f)
        {
            Debug.Log("Chase player");
            //Enemy can chase player
            newPath = RequestPath(enemyUnit.position, los.visibleTargets[0].position);
            MoveAlongPath();
            return NodeStates.RUNNING;
        }
        else
        {
            //Player is in range of attack
            return NodeStates.SUCCESS;
        }
    }


    public void MoveAlongPath()
    {
        foreach (AStarNode pathNodes in newPath)
        {
            Vector3 newPos = new Vector3(pathNodes.worldPosition.x, enemyUnit.position.y, pathNodes.worldPosition.z);
            enemyUnit.position = Vector3.MoveTowards(enemyUnit.position, newPos, EnemySpeed * Time.deltaTime);
        }
        reachedNewDestination = true;
    }

    public List<AStarNode> RequestPath(Vector3 startPos, Vector3 endPos)
    {
        PathFinder.CalculatePath(startPos, endPos);
        return PathFinder.path;
    }
}
