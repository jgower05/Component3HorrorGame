using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float damageTaken = 0.0f;
    public float threshold = 30f;
    public float damageConditioner = 0.25f;
    public float damageReplenisherSpeed = 2f;
    public float timeSinceLastHit = 0.0f;
    public bool damaged = false;
    public bool enemyNeedsToRetreat = false;

    public void LateUpdate() {
        if (damaged)
        {
            timeSinceLastHit -= Time.deltaTime; //Initiate a timer once the enemy has been hit
            if (timeSinceLastHit <= 0f)
            {
                damaged = false;    //Once the enemy hasn't been hit in a while, they can start to heal
                timeSinceLastHit = 5f;
            }
        }
        else {
            if (damageTaken > 0f) 
            {
                damageTaken -= Time.deltaTime * damageReplenisherSpeed; //If the enemy hasn't been hit in a while, then damage taken can be reduced.
            }
        }

        if (damageTaken >= threshold) {
            enemyNeedsToRetreat = true; //Kicks starts the retreating process
        }
    }

    public void TakeDamage(int damage) {
        damageTaken += damage * damageConditioner; //Increases the damage taken by the enemy
        damaged = true;
        timeSinceLastHit = 5f;
    }
}
