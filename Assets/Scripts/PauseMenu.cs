using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject confirmQuit;

    public GameObject settingsMenu;

    public GameObject player;
    
    private PlayerCombat playerCombat;
    private Movement playerMovement;
    
    // Update is called once per frame

    void Start() {
        playerCombat = player.GetComponent<PlayerCombat>();
        playerMovement = player.GetComponent<Movement>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                Resume();
                GameIsPaused = false;
            } else {
                Pause();
                GameIsPaused = true;
            }
        }
    }

    public void Resume() {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        playerCombat.enabled = true;
        playerMovement.enabled = true;        
    }

    void Pause() {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        playerCombat.enabled = false;
        playerMovement.enabled = false;        
    }

    public void Menu() {
        confirmQuit.SetActive(true);
    }
    public void QuitYes() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameStart");
    }

    public void QuitNo() {
        confirmQuit.SetActive(false);
    }

    public void Settings() {
        settingsMenu.SetActive(true);
    }
}