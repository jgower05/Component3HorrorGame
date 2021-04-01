using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject artifact;
    public int itemsCollected = 0;
    public int numberOfItemsNeeded = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (itemsCollected >= numberOfItemsNeeded)
        {
            //Sort out the end game as the player has won
        }

        if (player.currentHealth <= 0f) {
            //Player has died and failed
        }
    }

}
