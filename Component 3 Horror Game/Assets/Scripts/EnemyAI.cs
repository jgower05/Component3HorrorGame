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
    [SerializeField] private float enemySprintingSpeed;
    [SerializeField] private float enemyDamage;
    [SerializeField] private float enemyRotationSpeed;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private LineOfSight lineOfSight;
    [SerializeField] private FindPath pathFinder;
    [SerializeField] private WaypointIdentifier waypointIdentifier;
    [SerializeField] private Stings sting;

    [SerializeField] public float attackCooldownTimer;
    [SerializeField] public float setAttackCooldownTimer = 1.25f;
    public bool attackedPlayer = false;

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
        attackCooldownTimer = setAttackCooldownTimer;
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

        //Nodes relating to the attack sequence
        ActionNode IsPlayerInAttackProximityNode = new ActionNode(IsPlayerInAttackProximity);
        ActionNode DealDamageToPlayerNode = new ActionNode(DealDamageToPlayer);
        Sequence attackSequence = new Sequence(new List<Node> { IsPlayerInAttackProximityNode, DealDamageToPlayerNode });

        rootNode = new Selectors(new List<Node> { chaseSequence, attackSequence, wanderSequence});
    }

    public void FindPoint(Vector3 pointPosition) {
        positionOfPoint = pointPosition;
    }

    private void Update() {
        rootNode.Evaluate();
        if (attackedPlayer) {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer <= 0) {
                attackedPlayer = false;
                attackCooldownTimer = setAttackCooldownTimer;
            }
        }
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
        positionOfPoint = lineOfSight.visibleTargets[0].position;
        if (Vector3.Distance(transform.position, positionOfPoint) > 0.75f)
        {
            newPath = RequestPath(transform.position, positionOfPoint);
            MoveAlongPath(enemySprintingSpeed);
            return NodeStates.SUCCESS;
        }
        return NodeStates.FAILURE;
    }

    //Move the enemy to a waypoint position
    private NodeStates MoveToWaypoint() {
        //UnityEngine.Debug.Log($"Distance between enemy and waypoint: {Vector3.Distance(transform.position, positionOfPoint)}");
        if (Vector3.Distance(transform.position, positionOfPoint) > 3f)
        {
            newPath = RequestPath(transform.position, positionOfPoint);
            MoveAlongPath(enemySpeed);
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

    //Check if player is close enough for the enemy to attack.
    private NodeStates IsPlayerInAttackProximity() {
        if (sting.isPlayerInProximityOfAttack)
        {
            return NodeStates.SUCCESS;
        }
        else {
            return NodeStates.FAILURE;
        }
    }

    //Deal damage to player
    private NodeStates DealDamageToPlayer() {
        if (!attackedPlayer)
        {
            //Deal damage to player
            playerMovement.currentHealth -= enemyDamage;
            attackedPlayer = true;
            return NodeStates.SUCCESS;
        }
        else {
            return NodeStates.FAILURE;
        }
    }

    public void MoveAlongPath(float speed)
    {
        foreach (AStarNode pathNodes in newPath)
        {
            Vector3 newPos = new Vector3(pathNodes.worldPosition.x, transform.position.y, pathNodes.worldPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
            SmoothLookAt(positionOfPoint);
            
        }
        reachedNewDestination = true;
    }

    //Rotates the enemy to face the target position on the Y-axis
    public void SmoothLookAt(Vector3 targetPosition) {
        var rotation = Quaternion.LookRotation(targetPosition - transform.position);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * enemyRotationSpeed);
    }

    public List<AStarNode> RequestPath(Vector3 startPos, Vector3 endPos)
    {
        pathFinder.CalculatePath(startPos, endPos);
        return pathFinder.path;
    }
}
