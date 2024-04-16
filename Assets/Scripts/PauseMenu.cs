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
    
    // Update is called once per frame
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
    }

    void Pause() {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
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