using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public FindPath pathFinder;
    [Range(0f, 15f)] public float enemySpeed;
    public float enemyStamina;
    public bool isMoving;
    public Transform goal;
    public bool reachedNewDestination;

    private List<AStarNode> oldPath, newPath;

    // Update is called once per frame
    void Update(){
        newPath = RequestPath(transform.position, goal.position);
        if (oldPath != newPath) {
            reachedNewDestination = false;
            oldPath = newPath;
        }
        if (newPath != null && !reachedNewDestination) {
            MoveAlongPath();
        }
    }

    public void MoveAlongPath() {
        foreach (AStarNode pathNodes in newPath) {
            transform.position = Vector3.MoveTowards(transform.position, pathNodes.worldPosition, enemySpeed / 10);
        }
        reachedNewDestination = true;
    }

    public List<AStarNode> RequestPath(Vector3 startPos, Vector3 endPos) {
        pathFinder.CalculatePath(startPos, endPos);
        return pathFinder.path;
    }

}
