using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Torch : MonoBehaviour
{
    public float battery = 100.0f;
    public float batteryAmountToDecreaseBy = 1.0f;
    const float batteryDegradationTimer = 10.0f;
    public float batteryTimer = 0.0f;
    public GameObject light;
    public bool isOn = false;
    public bool hasClicked = false;
    PlayerControls controls;

    void Awake() {
        light.SetActive(false);
        controls = new PlayerControls();
        controls.Player.Fire.performed += ctx => TorchOn();
    }

    void Update(){
        if (isOn) {
            if (batteryTimer < batteryDegradationTimer){
                batteryTimer += Time.deltaTime;
            } else if (batteryTimer >= batteryDegradationTimer){
                battery -= batteryAmountToDecreaseBy;
                batteryTimer = 0.0f;
            }
        }

        if (battery <= 0) {
            isOn = false;
            light.SetActive(false);
        }
        
    }

    void TorchOn() {
        if (battery > 0 && !isOn)
        {
            isOn = true;
            light.SetActive(true);
            Debug.Log("Torch on!");
        }
        else if (battery > 0 && isOn)
        {
            isOn = false;
            light.SetActive(false);
        }
    }

    void OnEnable() {
        controls.Player.Enable();
    }

}
