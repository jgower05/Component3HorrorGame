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

    // Update is called once per frame
    void Update(){
        if (pathFinder.hasDestinationBeenFound) {
            isMoving = true;
            pathFinder.CalculatePath(transform.position, goal.position);
        }
    }

}
