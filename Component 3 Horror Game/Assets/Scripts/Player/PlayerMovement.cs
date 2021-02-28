using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;
    public float maxSpeed = 2.0f;
    public float playerStamina = 100.0f;
    float playerMaxStamina = 100.0f;
    public float staminaRegenTimer = 0.0f;
    public float staminaTimeToRegen = 5.0f;
    public bool isRunning;
    public bool isSneaking;

    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public Slider slider;
    public Slider staminaSlider;

    PlayerControls controls;
    Vector2 move;

    void Start() {
        //Sets the values for the Health Bar UI as soon as the game starts. 
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        //Sets the values for the Stamina Bar UI as soon as the game starts.
        staminaSlider.maxValue = playerMaxStamina;
        staminaSlider.value = playerStamina;
    }

    void Awake() {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
    }

    void Update()
    {
        slider.value = currentHealth;
        staminaSlider.value = playerStamina;
        //Vector3 keyboardInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        /*if (move != Vector2.zero)
        {
            MoveAround(new Vector3(move.x, 0f, move.y));
        }
        if (isSneaking)
        { //Checks if the player wants to sneak when they're holding down the left control key
            Debug.Log("Sneaking!");
            playerSpeed = maxSpeed - 1.5f;
            MoveAround(keyboardInput);
            RegenerateStamina();
        }
        if ((isRunning && Input.GetKey(KeyCode.W)) && playerStamina > 0.0f)
        {
            playerSpeed = maxSpeed + 1.5f;
            MoveAround(keyboardInput);
            playerStamina = Mathf.Clamp(playerStamina - (20.0f * Time.deltaTime), 0.0f, playerMaxStamina);
            staminaRegenTimer = 0.0f;
        }
        else if ((playerStamina < playerMaxStamina) && !isSneaking)
        { //Only does this part if the player isn't sneaking
            playerSpeed = maxSpeed;
            RegenerateStamina();
            MoveAround(keyboardInput);
        } */
        Vector3 input = new Vector3(move.x, 0, move.y);
        Vector3 direction = input.normalized;
        Vector3 velocity = playerSpeed * direction;
        Vector3 moveAmount = velocity * Time.deltaTime;
        transform.Translate(moveAmount);
    }

    void RegenerateStamina() { //Regenerates stamina over time once the regen timer allows it to.
        if (staminaRegenTimer >= staminaTimeToRegen){
            playerStamina = Mathf.Clamp(playerStamina + (10.0f * Time.deltaTime), 0.0f, playerMaxStamina);
            //Mathf.Clamp keeps the stamina from regenerating above the maximum stamina value.
        }
        else{
            staminaRegenTimer += Time.deltaTime;
        }
    }

    //Move the player around towards direction they've inputted (WASD)
    void MoveAround(Vector3 input) {
        //Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        Vector3 velocity = playerSpeed * direction;
        Vector3 moveAmount = velocity * Time.deltaTime;
        transform.Translate(moveAmount);
    }
}
