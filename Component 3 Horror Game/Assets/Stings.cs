using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stings : MonoBehaviour
{
    public bool isPlayerInProximityOfAttack = false;

    //Player can be attacked if they enter the collider of the sting
    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player"))
        {
            isPlayerInProximityOfAttack = true;
        }
    }

    //Player cannot be attacked if they exit the collider of the sting
    void OnTriggerExit(Collider collider) {
        if (collider.CompareTag("Player")) {
            isPlayerInProximityOfAttack = false;
        }
    }
}
