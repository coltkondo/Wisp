using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Timestop_Eric : MonoBehaviour
{
    public GameObject timeBar;
    public GameObject player;
    public GameObject manager;

    public int currentTimePoints;
    public int maxTimePoints;

    private PlayerAudio playerAudio;
    private PlayerHealth playerHealth;
    private GameManager gameManager;
    private DialogueManager diaManager;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerAudio = player.GetComponent<PlayerAudio>();
        playerHealth = player.GetComponent<PlayerHealth>();
        gameManager = FindAnyObjectByType<GameManager>();
        diaManager = manager.GetComponent<DialogueManager>();

        timeBar.GetComponent<Image>().type = Image.Type.Filled;
        currentTimePoints = maxTimePoints;
        updateTimeBar();
    }
    private void Update()
    {
        if (diaManager.isTalking == false)
        {
            if (Input.GetKeyDown(KeyCode.X) && !gameManager.timeStopRunning) //X Click for Input - Main Time Stop
            {
                if (currentTimePoints != 0)
                {
                    decreaseTimePoints(1);
                    //perform time stop effects here
                    gameManager.timeIsStopped = true;
                }

            }

            if (playerHealth.currentHealth != 3)
            {
                if (Input.GetKeyDown(KeyCode.Z)) //Z Click for Heal
                {
                    if (currentTimePoints != 0)
                    {
                        Debug.Log("Heal Key Pressed");
                        Debug.Log("Audio playing: " + playerAudio.HealSource.isPlaying);
                        decreaseTimePoints(1);
                        StartCoroutine(PlayerHealFreeze());
                    }
                }
            }
        
        }
    }

        // Time Bar Section - Updates the UI for changes to player's time points.
       public void increaseTimePoints(int value)
    {
        currentTimePoints += value;
        if (currentTimePoints > maxTimePoints)
        {
            currentTimePoints = maxTimePoints;
        }
        updateTimeBar();
    }

    public void decreaseTimePoints(int value)
    {
        currentTimePoints -= value;
        if (currentTimePoints <= 0)
        {
            currentTimePoints = 0;
        }
        updateTimeBar();
    }

    void resetTimePoints()
    {
        currentTimePoints = maxTimePoints;
        updateTimeBar();
    }

    void updateTimeBar()
    {
        float fillAmount = (float)currentTimePoints / maxTimePoints;
        if (fillAmount > 1)
        {
            fillAmount = 1.0f;
        }

        timeBar.GetComponent<Image>().fillAmount = fillAmount;
    }

    IEnumerator PlayerHealFreeze()
    {
        gameManager.DisablePlayerMovement();
        // INSERT HEAL ANIMATION FLAG HERE
        if (playerAudio.HealSource.isPlaying != true)
        {
            playerAudio.HealSource.Play();
        } 
        yield return new WaitForSeconds(1.4f);
        playerHealth.IncreaseHealth(1);
        yield return new WaitForSeconds(0.2f);
        gameManager.EnablePlayerMovement();
    }
}
