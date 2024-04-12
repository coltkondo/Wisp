using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Timestop_Eric : MonoBehaviour
{
    public GameObject timeBar;
    public GameObject player;


    public int currentTimePoints;
    public int maxTimePoints;

    private PlayerHealth playerHealth;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        gameManager = FindAnyObjectByType<GameManager>();

        timeBar.GetComponent<Image>().type = Image.Type.Filled;
        currentTimePoints = maxTimePoints;
        updateTimeBar();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) //XC lick for Input - Main Time Stop
        {
            decreaseTimePoints(1);
            //perform time stop effects here
            gameManager.timeIsStopped = true;
        }

        if (Input.GetKeyDown(KeyCode.Z)) //Z Click for Heal
        {
            Debug.Log("Heal Key Pressed");
            decreaseTimePoints(1);
            StartCoroutine(PlayerHealFreeze());

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
        yield return new WaitForSeconds(0.3f);
        playerHealth.IncreaseHealth(1);
        yield return new WaitForSeconds(0.2f);
        gameManager.EnablePlayerMovement();
    }
}
