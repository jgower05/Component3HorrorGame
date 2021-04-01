using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    PlayerControls controls;
    [SerializeField] private GameObject inventoryUI;
    public bool hasOpened = false;
    public bool hasPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        controls.Player.Enable();
        controls.Player.Inventory.performed += ctx => hasPressed = true;
        controls.Player.Inventory.canceled += ctx => hasPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPressed && !hasOpened)
        {
            hasOpened = true;
            inventoryUI.SetActive(true);
        }
        else if(hasPressed && hasOpened) {
            hasOpened = false;
            inventoryUI.SetActive(false);
        }
    }

}
