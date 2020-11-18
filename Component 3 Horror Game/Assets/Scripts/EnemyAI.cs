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

    public LineOfSight los;
    public Transform playerChecker;

    private List<AStarNode> oldPath, newPath;
    private Transform oldGoal;

    // Update is called once per frame
    void FixedUpdate(){

        //Move the enemy to the player if seen
        playerChecker.GetComponent<Renderer>().material.color = Color.red;
        if (los.visibleTargets != null)
        { //Checks to make sure the visible targets isn't null
            foreach (Transform item in los.visibleTargets)
            {
                goal = item; //Set the goal position as the visible target (player)
                playerChecker.GetComponent<Renderer>().material.color = Color.green;
                newPath = RequestPath(transform.position, goal.position); //Request a path from the A* pathfinding algorithm
                MoveAlongPath();
            }
        }

    }
    public void MoveAlongPath() {
        foreach (AStarNode pathNodes in newPath) {
            transform.position = Vector3.MoveTowards(transform.position, pathNodes.worldPosition, enemySpeed * Time.deltaTime);
            /*
            Quaternion targetRotation = Quaternion.LookRotation(goal.position - transform.position);
            float step = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, step); */
        }
        reachedNewDestination = true;
    }

    public List<AStarNode> RequestPath(Vector3 startPos, Vector3 endPos) {
        pathFinder.CalculatePath(startPos, endPos);
        return pathFinder.path;
    }

}
