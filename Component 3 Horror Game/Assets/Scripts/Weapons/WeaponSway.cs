using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    public float amount;
    public float maxAmount;
    public float smoothAmount;

    private Vector3 initialPosition;
    PlayerControls controls;
    public Vector2 rotation;

    void Awake() {
        initialPosition = transform.localPosition;
        controls = new PlayerControls();

        controls.Player.Look.performed += ctx => rotation = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => rotation = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //Track the inverse of the mouse movement
        float movementX = -rotation.x * amount;
        float movementY = -rotation.y * amount;
        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);

    }

    void OnEnable() {
        controls.Player.Enable();
    }
}
