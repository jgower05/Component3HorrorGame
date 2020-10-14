using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public FindPath pathFinder;
    [Range(0f, 15f)] public float enemySpeed;
    [Range(0f, 100f)] public float rotationSpeed;
    public float enemyStamina;
    public bool isMoving;
    public Transform goal;
    public bool reachedNewDestination;

    private List<AStarNode> oldPath, newPath;
    private Transform oldGoal;

    // Update is called once per frame
    void FixedUpdate(){
        newPath = RequestPath(transform.position, goal.position);
        if (oldPath != newPath) {
            reachedNewDestination = false;
            oldPath = newPath;
        }
        if (newPath != null && !reachedNewDestination)
        {
            MoveAlongPath();
        } 

    }
    public void MoveAlongPath() {
        foreach (AStarNode pathNodes in newPath) {
            transform.position = Vector3.MoveTowards(transform.position, pathNodes.worldPosition, enemySpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(pathNodes.worldPosition - transform.position);
            float step = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, step);
        }
        reachedNewDestination = true;
    }

    public List<AStarNode> RequestPath(Vector3 startPos, Vector3 endPos) {
        pathFinder.CalculatePath(startPos, endPos);
        return pathFinder.path;
    }

}
