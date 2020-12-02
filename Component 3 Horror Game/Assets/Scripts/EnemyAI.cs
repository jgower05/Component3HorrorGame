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
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private LineOfSight lineOfSight;
    [SerializeField] private FindPath pathFinder;

    private Node rootNode;

    void Start() {
        ConstructBehaviourTree();
    }

    private void ConstructBehaviourTree() {
        ChaseNode chaseNode = new ChaseNode(pathFinder, playerTransform, enemyTransform, enemySpeed, lineOfSight);
        IsInRangeNode isInRangeNode = new IsInRangeNode(lineOfSight);
        IsPlayerInRangeNode isPlayerInRangeNode = new IsPlayerInRangeNode(playerTransform, enemyTransform);
        AttackNode attackNode = new AttackNode(enemyDamage, player);

        Sequence chaseSequence = new Sequence(new List<Node> { isInRangeNode, chaseNode});
        Sequence attackSequence = new Sequence(new List<Node> { isPlayerInRangeNode, attackNode });

        rootNode = new Selectors(new List<Node> { chaseSequence, attackSequence });
    }

    private void Update() {
        rootNode.Evaluate();
    }
}
