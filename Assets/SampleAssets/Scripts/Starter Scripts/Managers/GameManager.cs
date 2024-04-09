using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//This component should be placed on a gameobject in your scene
	public bool timeIsStopped = false;
	public float timeFreezeDuration;
	public GameObject[] enemies;
	public Animator[] anim;

	[HideInInspector]
	public Vector3 RespawnPlace;

	private bool timeStopRunning = false;

	[Tooltip("Place your player game object in here so this knows where to handle respawns")]
	public GameObject Player;
	// Start is called before the first frame update
	void Start()
	{
		if (Player == null)
		{
			Player = FindObjectOfType<Movement>().gameObject;
		}
		RespawnPlace = Player.transform.position;

		enemies = GameObject.FindGameObjectsWithTag("Enemy"); //Searches for all of the enemies within the Level
		Debug.Log("NUMBER OF ENEMIES: " + enemies.Length + "");

		/*for (int i = 0; i < enemies.Length; i++) // Finds all of the enemies Animators
		{
			anim = new Animator[enemies.Length];
			anim[i] = enemies[i].GetComponent<Animator>();
		}
        Debug.Log("NUMBER OF ENEMY ANIMATORS: " + anim.Length + ""); */
    }

    private void Update()
    {
        if (timeIsStopped == true)
		{
			StartCoroutine(FreezeTime());
		}

    }

    public void Respawn(GameObject Player)//This is just where we respawn the player
	{
		Player.transform.position = RespawnPlace;
	}

	public void SetNewRespawnPlace(GameObject newPlace)//This is 
	{
		RespawnPlace = newPlace.transform.position;
	}

	public void DisablePlayerMovement()
	{
		Movement Movement = Player.GetComponent<Movement>();
		PlayerAudio playerAudio = Player.GetComponent<PlayerAudio>();

        Movement.DisablePlayer();
		if (playerAudio)
			playerAudio.StopAll();
	}

    public void EnablePlayerMovement()
    {
        Movement Movement = Player.GetComponent<Movement>();
        PlayerAudio playerAudio = Player.GetComponent<PlayerAudio>();

        Movement.EnablePlayer();
		if (playerAudio)
			playerAudio.StopAll();
    }

	IEnumerator FreezeTime()
	{
		if (timeStopRunning == false) {
			timeStopRunning = true;
            Debug.Log("Time is stopped");
            //lock enemy speed
            /*for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetBool("isTimeStopped", true); //Freezes all enemies in place
            }*/
            for (int i = 0; i < enemies.Length; i++) // Finds all of the enemies Animators
            {
                anim = new Animator[enemies.Length];
                anim[i] = enemies[i].GetComponent<Animator>();
				anim[i].SetBool("isTimeStopped", true);
            }
			//lock all projectiles
			// other stuff
            yield return new WaitForSeconds(timeFreezeDuration);
            for (int i = 0; i < enemies.Length; i++)
            {
                anim = new Animator[enemies.Length];
                anim[i] = enemies[i].GetComponent<Animator>();
                anim[i].SetBool("isTimeStopped", false); //Un-Freezes all enemies
            }
            timeIsStopped = false;
            Debug.Log("Time is resumed");
            timeStopRunning = false;
        }
    }
}
