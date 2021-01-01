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
    [SerializeField] private float health;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyDamage;
    [SerializeField] private float enemyRotationSpeed;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private LineOfSight lineOfSight;
    [SerializeField] private FindPath pathFinder;
    [SerializeField] private WaypointIdentifier waypointIdentifier;

    public bool reachedNewDestination;
    private List<AStarNode> newPath;
    public float radius;
    public Transform previousWaypoint;

    [SerializeField] private EnemyAI enemyAI;
    private Vector3 positionOfPoint;
    public bool chooseNewWaypoint = true;
    Random random = new Random();

    private Node rootNode;
    private GameObject investigatePoint;

    void Start() {
        ConstructBehaviourTree();
    }

    private void ConstructBehaviourTree() {
        //Nodes relating to the chase sequence
        ActionNode IsPlayerInRangeNode = new ActionNode(IsPlayerInRange);
        ActionNode MoveToPlayerNode = new ActionNode(MoveToPlayer);
        Sequence chaseSequence = new Sequence(new List<Node> { IsPlayerInRangeNode, MoveToPlayerNode });

        // Nodes relating to the wander sequence.
        ActionNode CheckWaypointsAreAvailableNode = new ActionNode(CheckIfWaypointsExist);
        ActionNode SelectWaypointNode = new ActionNode(SelectWaypoint);
        ActionNode MoveToWaypointNode = new ActionNode(MoveToWaypoint);
        Sequence wanderSequence = new Sequence(new List<Node> { CheckWaypointsAreAvailableNode, SelectWaypointNode, MoveToWaypointNode});

        rootNode = new Selectors(new List<Node> { chaseSequence, wanderSequence});
    }

    public void FindPoint(Vector3 pointPosition) {
        positionOfPoint = pointPosition;
    }

    private void Update() {
        rootNode.Evaluate();
    }


    //Check if player is in range
    private NodeStates IsPlayerInRange() {
        //UnityEngine.Debug.Log("Checking to see if player is in vision!");
        if (!lineOfSight.isPlayerInLineOfSight)
        {
            return NodeStates.FAILURE;
        }
        else {
            return NodeStates.SUCCESS;
        }
    }

    //Move To Player
    private NodeStates MoveToPlayer() {
        if (Vector3.Distance(transform.position, lineOfSight.visibleTargets[0].position) > 0.75f)
        {
            newPath = RequestPath(transform.position, lineOfSight.visibleTargets[0].position);
            MoveAlongPath();
            return NodeStates.SUCCESS;
        }
        return NodeStates.FAILURE;
    }

    //Move the enemy to a waypoint position
    private NodeStates MoveToWaypoint() {
        UnityEngine.Debug.Log($"Distance between enemy and waypoint: {Vector3.Distance(transform.position, positionOfPoint)}");
        if (Vector3.Distance(transform.position, positionOfPoint) > 3f)
        {
            newPath = RequestPath(transform.position, positionOfPoint);
            MoveAlongPath();
            return NodeStates.RUNNING;
        }
        else {
            UnityEngine.Debug.Log("Made it to waypoint");
            chooseNewWaypoint = true;
            return NodeStates.SUCCESS;
        }
    }

    //Select a random waypoint from the list - makes sure the new waypoint is different from the last one.
    private NodeStates SelectWaypoint() {
        if (chooseNewWaypoint)
        {
            int index = UnityEngine.Random.Range(0, waypointIdentifier.visibleWaypoints.Count - 1);
            positionOfPoint = waypointIdentifier.visibleWaypoints[index].position;
            if (positionOfPoint != previousWaypoint.position)
            {
                previousWaypoint = waypointIdentifier.visibleWaypoints[index];
                chooseNewWaypoint = false;
                return NodeStates.SUCCESS;
            }
            else
            {
                return NodeStates.FAILURE;
            }
        }
        else {
            return NodeStates.SUCCESS;
        }
    }

    //Checks to make sure waypoints have been identified by the enemy
    private NodeStates CheckIfWaypointsExist() {
        if (waypointIdentifier.visibleWaypoints.Count == 0)
        {
            return NodeStates.FAILURE;
        }
        else {
            return NodeStates.SUCCESS;
        }
    }

    public void MoveAlongPath()
    {
        foreach (AStarNode pathNodes in newPath)
        {
            Vector3 newPos = new Vector3(pathNodes.worldPosition.x, transform.position.y, pathNodes.worldPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, enemySpeed * Time.deltaTime);
            //SmoothLookAt(newPos);
            
        }
        reachedNewDestination = true;
    }

    public void SmoothLookAt(Vector3 targetPosition) {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPosition), enemyRotationSpeed * Time.deltaTime);
    }

    public List<AStarNode> RequestPath(Vector3 startPos, Vector3 endPos)
    {
        pathFinder.CalculatePath(startPos, endPos);
        return pathFinder.path;
    }
}
