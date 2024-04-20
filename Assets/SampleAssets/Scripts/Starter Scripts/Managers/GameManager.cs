using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
	//This component should be placed on a gameobject in your scene
	public bool timeIsStopped = false;
	public float timeFreezeDuration;
	public GameObject[] enemies;
	public Animator[] anim;
    public GameObject timestop_UI;

    public Vector3 RespawnPlace;

	[HideInInspector]

	public bool timeStopRunning = false;
    private Animator timestop_anim;

	[Tooltip("Place your player game object in here so this knows where to handle respawns")]
	public GameObject Player;

    PostProcessVolume m_Volume;
    Vignette m_Vignette;
    ColorGrading m_ColorGrading;


	// Start is called before the first frame update
	void Start()
	{
		if (Player == null)
		{
			Player = FindObjectOfType<Movement>().gameObject;
		}
		// RespawnPlace = Player.transform.position;

		enemies = GameObject.FindGameObjectsWithTag("Enemy"); //Searches for all of the enemies within the Level
		Debug.Log("NUMBER OF ENEMIES: " + enemies.Length + "");

        timestop_anim = timestop_UI.GetComponent<Animator>();
        if (timestop_anim != null)
        {
            Debug.Log("Timestop_Anim found!");
        }

        //Timestop Greyscale Stuff
        m_ColorGrading = ScriptableObject.CreateInstance<ColorGrading>();
        m_ColorGrading.enabled.Override(false);
        m_ColorGrading.saturation.Override(-100f);

        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_ColorGrading);

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
            PlayerAudio playerAudio = Player.GetComponent<PlayerAudio>();
            playerAudio.TimeStopSource.Play();
			timeStopRunning = true;
            Debug.Log("Time is stopped");
            m_ColorGrading.enabled.Override(true);
            timestop_UI.SetActive(true);
            timestop_anim.SetBool("Play", true);
            // Freezes all of the Enemies in place
            for (int i = 0; i < enemies.Length;) 
            {
				if (enemies[i] != null)
				{
                    anim = new Animator[enemies.Length];
                    anim[i] = enemies[i].GetComponent<Animator>();
                    anim[i].SetBool("isTimeStopped", true);
					i++;
                } else
				{
					i++;
				}
                
            }
            // lock all projectiles
            // other stuff
            // Un-Freezes Enemies
            yield return new WaitForSeconds(timeFreezeDuration);
            for (int i = 0; i < enemies.Length;)
            {
                if (enemies[i] != null)
                {
                    anim = new Animator[enemies.Length];
                    anim[i] = enemies[i].GetComponent<Animator>();
                    anim[i].SetBool("isTimeStopped", false);
                    i++;
                }
                else
                {
                    i++;
                }

            }
            timeIsStopped = false;
            Debug.Log("Time is resumed");
            m_ColorGrading.enabled.Override(false);
            timeStopRunning = false;
        }
    }
}
