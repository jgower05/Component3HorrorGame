using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed      = 5f;
    [SerializeField] private float playerMaxSpeed   = 5f;
    [SerializeField] private float playerStamina    = 100f;
    [SerializeField] private float playerMaxStamina = 100f;
    [SerializeField] private float playerStaminaRegenTimer = 0f;
    [SerializeField] private float playerTimeToRegen       = 5f;
    [SerializeField] private GunRecoil recoil;
    public float camRotationSpeed = 5f;
    public float mouseSenstivity = 100f;
    public float xRotation = 0f;
    public float cameraMinimumY = -60f;
    public float cameraMaximumY = 75f;
    public float rotationSmoothSpeed = 10f;
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public bool isRunning = false;
    public bool isSneaking = false;
    public Transform camera;
    private float bodyRotationX;
    private float camRotationY;
    PlayerControls controls;
    Vector2 move;
    Vector2 rotation;

    void Awake() {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        controls.Player.Look.performed += ctx => rotation = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => rotation = Vector2.zero;

        controls.Player.Sprint.performed += ctx => isRunning = true;
        controls.Player.Sprint.canceled += ctx => isRunning = false;
        
    }

    void Update() {

        if (isRunning && playerStamina > 0.0f)
        {
            playerSpeed = 5f;
            playerStamina = Mathf.Clamp(playerStamina - (20.0f * Time.deltaTime), 0.0f, playerMaxStamina);
            playerStaminaRegenTimer = 0f;
        }
        else if(!isRunning) {
            playerSpeed = playerMaxSpeed;
            RegenerateStamina();
        }

        Move();
        LookRotation();
    }

    void OnEnable() {
        controls.Player.Enable();
    }

    void Move() {
        Vector3 input = new Vector3(move.x, 0f, move.y);
        Vector3 moveAmount = playerSpeed * input.normalized;
        transform.Translate(moveAmount * Time.deltaTime);
    }

    void RegenerateStamina() {
        if (playerStaminaRegenTimer >= playerTimeToRegen)
        {
            playerStamina = Mathf.Clamp(playerStamina + (10.0f * Time.deltaTime), 0.0f, playerMaxStamina);
        }
        else {
            playerStaminaRegenTimer += Time.deltaTime;
        }
    }

    void LookRotation() {
        /*
        //Debug.Log("Joystick: {" + rotation.x + "," + rotation.y + "}");
        bodyRotationX += rotation.x * camRotationSpeed * Time.deltaTime;
        camRotationY += rotation.y * camRotationSpeed * Time.deltaTime;

        camRotationY = Mathf.Clamp(camRotationY, cameraMinimumY, cameraMaximumY);
        //create rotation targets and handle the rotations of the body and camera
        Quaternion camTargetRotation = Quaternion.Euler(-camRotationY - recoil.rot.x, 0 - recoil.rot.y, 0 - recoil.rot.z);
        Quaternion bodyTargetRotation = Quaternion.Euler(0, bodyRotationX, 0);

        //handle the rotations
        transform.rotation = Quaternion.Lerp(transform.rotation, bodyTargetRotation, Time.deltaTime * rotationSmoothSpeed);
        camera.localRotation = Quaternion.Lerp(camera.localRotation, camTargetRotation, Time.deltaTime * rotationSmoothSpeed); */

        float mouseX = rotation.x * mouseSenstivity * Time.deltaTime;
        float mouseY = rotation.y * mouseSenstivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        camera.localRotation = Quaternion.Euler(xRotation - recoil.rot.x, 0f - recoil.rot.y, 0f - recoil.rot.z);
        transform.Rotate(Vector3.up * mouseX);
    }
}
