using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectLoadScene : MonoBehaviour
{
    public GameObject fadeOutImage;
    public int sceneIndex = 2;

    public GameObject player;

    private Movement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<Movement>();
    }
    private void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision gang");
        if (other.CompareTag("Player"))
        {
            playerMovement.enabled = false;
            ChangeScene();
        }
    }
    void ChangeScene()
    {
        StartCoroutine(Transition());
    }

    private	IEnumerator Transition() {
        Animator anim = fadeOutImage.GetComponent<Animator>();
        anim.SetTrigger("StartFadeOut"); // Make sure the trigger name matches the one in the Animator
        // Wait for the animation to finish
        yield return new WaitForSeconds(2); // Adjust this time based on the animation length

		anim.ResetTrigger("StartFadeOut");
		anim.ResetTrigger("StartFadeIn");

        SceneManager.LoadScene(sceneIndex);
	}
    
}
