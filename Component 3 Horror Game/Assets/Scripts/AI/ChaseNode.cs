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
            //Enemy can chase player
            newPath = RequestPath(enemyUnit.position, los.visibleTargets[0].position);
            MoveAlongPath();
            return NodeStates.RUNNING;
        }
        else if (los.visibleTargets != null && distance <= 0.2f)
        {
            //Player is in range of attack
            return NodeStates.SUCCESS;
        }
        else {
            return NodeStates.FAILURE;
        }
    }


    public void MoveAlongPath()
    {
        foreach (AStarNode pathNodes in newPath)
        {
            enemyUnit.position = Vector3.MoveTowards(enemyUnit.position, pathNodes.worldPosition, EnemySpeed * Time.deltaTime);
            Vector3 targetDestination = new Vector3(target.position.x, enemyUnit.position.y, target.position.z);
            enemyUnit.LookAt(targetDestination);
        }
        reachedNewDestination = true;
    }

    public List<AStarNode> RequestPath(Vector3 startPos, Vector3 endPos)
    {
        PathFinder.CalculatePath(startPos, endPos);
        return PathFinder.path;
    }
}
