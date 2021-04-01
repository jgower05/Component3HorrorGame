using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    MenuInput controls;
    public GameObject settingsMenu;
    public GameObject playGame, openSettings, quitGame;
    public Transform[] text;
    int pointer = 0;

    bool selected = false;
    bool back = false;
    void Awake() {
        controls.Menu.Select.performed += ctx => selected = true;
        controls.Menu.Back.performed += ctx => back = true;
    }

    void Update() {
        if (selected)
        {
            switch (pointer)
            {
                case 0:
                    selected = false;
                    PlayGame();
                    break;
                case 1:
                    selected = false;
                    Debug.Log("Settings");
                    break;
                case 2:
                    selected = false;
                    QuitGame();
                    break;
            }
        }
    }


    public void OnButtonUp() {
        if (pointer == 0)
        {
            pointer = text.Length - 1;
        }
        else {
            pointer--;
        }
    }

    public void OnButtonDown() {
        if (pointer == text.Length - 1)
        {
            pointer = 0;
        }
        else {
            pointer++;
        }
    }

    //When selected, transition the player to the main game
    public void PlayGame() {
        SceneManager.LoadScene("EnemyAI");
    }

    //When selected, transition the player to their desktop (stop the application)
    public void QuitGame() {
        Application.Quit();
    }

    void OnEnable() {
        controls.Menu.Enable();
    }

    void OnDisable() {
        controls.Menu.Disable();
    }
}
