using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    PlayerControls controls;
    bool pauseActivated = false;

    void Awake() {
        controls = new PlayerControls();
        controls.Player.Menu.performed += ctx => pauseActivated = true;
    }

    void FixedUpdate() {
        if (pauseActivated)
        {
            pauseMenu.SetActive(true);
        }
        else {
            pauseMenu.SetActive(false);
        }
    }

    //Carry on with the game
    public void Continue() {
        pauseMenu.SetActive(false);  
    }

    //Open the settings menu
    public void Settings() { 
    }

    //Quit the game to the main menu
    public void Quit() {
        SceneManager.LoadScene("MainMenu");

    }
}
