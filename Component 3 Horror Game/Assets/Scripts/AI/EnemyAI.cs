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
    [SerializeField] private EnemyStats enemyStats;

    [SerializeField] public float attackCooldownTimer;
    [SerializeField] public float setAttackCooldownTimer = 1.25f;
    public bool attackedPlayer = false;

    public bool reachedNewDestination;
    private List<AStarNode> newPath;
    public float radius;
    public Transform previousWaypoint;
    public float damageTaken = 0.0f;

    private Vector3 positionOfPoint;
    public bool chooseNewWaypoint = true;
    Random random = new Random();
    private GameObject investigatePoint;

    private Selectors rootNode;
    private Sequence chaseSequence;
    private ActionNode IsPlayerInRangeNode;
    private ActionNode MoveToPlayerNode;

    private Sequence attackSequence;
    private ActionNode IsPlayerInAttackProximityNode;
    private ActionNode DealDamageToPlayerNode;

    private Sequence wanderSequence;
    private ActionNode CheckWaypointsAreAvailableNode;
    private ActionNode SelectWaypointNode;
    private ActionNode MoveToWaypointNode;

    [Header("Behaviour Tree Game Objects")]
    public GameObject o_rootNode;
    public GameObject o_chaseSequence;
    public GameObject o_attackSequence;
    public GameObject o_wanderSequence;
    public GameObject o_isPlayerInRange;
    public GameObject o_moveToPlayer;
    public GameObject o_checkWaypointsAreAvailable;
    public GameObject o_selectWaypoint;
    public GameObject o_moveToWaypoint;
    public GameObject o_isPlayerInRangeOfAttack;
    public GameObject o_dealDamageToPlayer;

    [Header("BT Colour States")]
    public Color c_running;
    public Color c_success;
    public Color c_failure;

    void Start() {
        attackCooldownTimer = setAttackCooldownTimer;
        ConstructBehaviourTree();
    }

    private void ConstructBehaviourTree() {
        //Nodes relating to the chase sequence
        IsPlayerInRangeNode = new ActionNode(IsPlayerInRange);
        MoveToPlayerNode = new ActionNode(MoveToPlayer);
        chaseSequence = new Sequence(new List<Node> { IsPlayerInRangeNode, MoveToPlayerNode });

        // Nodes relating to the wander sequence.
        CheckWaypointsAreAvailableNode = new ActionNode(CheckIfWaypointsExist);
        SelectWaypointNode = new ActionNode(SelectWaypoint);
        MoveToWaypointNode = new ActionNode(MoveToWaypoint);
        wanderSequence = new Sequence(new List<Node> { CheckWaypointsAreAvailableNode, SelectWaypointNode, MoveToWaypointNode});

        //Nodes relating to the attack sequence
        IsPlayerInAttackProximityNode = new ActionNode(IsPlayerInAttackProximity);
        DealDamageToPlayerNode = new ActionNode(DealDamageToPlayer);
        attackSequence = new Sequence(new List<Node> { IsPlayerInAttackProximityNode, DealDamageToPlayerNode });

        rootNode = new Selectors(new List<Node> { chaseSequence, attackSequence, wanderSequence});
    }

    //Update the states of the visual behaviour tree.
    private void UpdateNodes() {
        //Root Node
        if (rootNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_rootNode);
        }
        else if (rootNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_rootNode);
        }
        else {
            SetRunning(o_rootNode);
        }
        
        //Chase Sequence
        if (chaseSequence.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_chaseSequence);
        }
        else if (chaseSequence.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_chaseSequence);
        }
        else {
            SetRunning(o_chaseSequence);
        }

        //Is Player In Range Node
        if (IsPlayerInRangeNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_isPlayerInRange);
        }
        else if (IsPlayerInRangeNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_isPlayerInRange);
        }
        else {
            SetRunning(o_isPlayerInRange);
        }
        
        //Move To Player Node
        if (MoveToPlayerNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_moveToPlayer);
        }
        else if (MoveToPlayerNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_moveToPlayer);
        }
        else {
            SetRunning(o_moveToPlayer);
        }
        
        // Attack Sequence
        if (attackSequence.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_attackSequence);
        }
        else if (attackSequence.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_attackSequence);
        }
        else {
            SetRunning(o_attackSequence);
        }
        
        // Is Player In Range Of Attack Node
        if (IsPlayerInAttackProximityNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_isPlayerInRangeOfAttack);
        }
        else if (IsPlayerInAttackProximityNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_isPlayerInRangeOfAttack);
        }
        else {
            SetRunning(o_isPlayerInRangeOfAttack);
        }
        
        // Deal Damage To Player Node
        if (DealDamageToPlayerNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_dealDamageToPlayer);
        }
        else if (DealDamageToPlayerNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_dealDamageToPlayer);
        }
        else {
            SetRunning(o_dealDamageToPlayer);
        }

        // Wander Sequence
        if (wanderSequence.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_wanderSequence);
        }
        else if (wanderSequence.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_wanderSequence);
        }
        else {
            SetRunning(o_wanderSequence);
        }

        // Check If Waypoints Are Available Node
        if (CheckWaypointsAreAvailableNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_checkWaypointsAreAvailable);
        }
        else if (CheckWaypointsAreAvailableNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_checkWaypointsAreAvailable);
        }
        else {
            SetRunning(o_checkWaypointsAreAvailable);
        }
        
        // Select Waypoint Node
        if (SelectWaypointNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_selectWaypoint);
        }
        else if (SelectWaypointNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_selectWaypoint);
        }
        else {
            SetRunning(o_selectWaypoint);
        }
        
        // Move To Waypoint Node
        if (MoveToWaypointNode.nodeState == NodeStates.SUCCESS)
        {
            SetSucceeded(o_moveToWaypoint);
        }
        else if (MoveToWaypointNode.nodeState == NodeStates.FAILURE)
        {
            SetFailed(o_moveToWaypoint);
        }
        else {
            SetRunning(o_moveToWaypoint);
        }
    }

    private void SetRunning(GameObject box)
    {
        box.GetComponent<Renderer>().material.color = c_running;
    }

    private void SetSucceeded(GameObject box)
    {
        box.GetComponent<Renderer>().material.color = c_success;
    }

    private void SetFailed(GameObject box)
    {
        box.GetComponent<Renderer>().material.color = c_failure;
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
        UpdateNodes();
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

    //Check if the enemy has taken too much damage and needs to retreat.
    private NodeStates HasPlayerDamagedEnemyEnough() {
        if (enemyStats.enemyNeedsToRetreat)
        {
            return NodeStates.SUCCESS;
        }
        else {
            return NodeStates.FAILURE;
        }
    }

    /*private NodeStates Retreat() {
        //Force the enemy to retreat to a far-away location
    } */

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
