using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{

    public int sceneIndex = 2;
    public GameObject interactionPrompt;
    //public string sceneName = "BossEncounter";

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Update()
    {

        if (interactionPrompt.activeInHierarchy && Input.GetKeyDown(KeyCode.E)) // Check if 'E' is pressed when prompt is active
        {
            ChangeScene();
            interactionPrompt.SetActive(false); // Hide the interaction prompt
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision gang");
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(true); // Show interaction prompt
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false); // Hide interaction prompt
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
