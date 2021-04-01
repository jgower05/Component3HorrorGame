using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField] private GameRules gameRules;
    [SerializeField] private GameObject text;
    PlayerControls controls;
    public bool hasRequestedPickup = false;
    public bool hasEnteredRadius = false;


    void Awake() {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => hasRequestedPickup = true;
        controls.Player.Interact.canceled += ctx => hasRequestedPickup = false;
    }

    void Update() {
        if (hasRequestedPickup && hasEnteredRadius)
        {
            Debug.Log("Requested Pickup");
            gameRules.itemsCollected++;
            hasRequestedPickup = false;
            Destroy(gameObject);
            text.SetActive(false);
        }

    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player") {
            text.SetActive(true);
            hasEnteredRadius = true;
        }
    }

    void OnTriggerExit(Collider col) { 
        hasEnteredRadius = false;
    }

    void OnEnable() {
        controls.Player.Enable();
    }
}
