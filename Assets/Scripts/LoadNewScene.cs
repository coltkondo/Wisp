using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{

    public int sceneIndex = 2;
    //public string sceneName = "BossEncounter";

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision gang");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected in scene transition");
            SceneManager.LoadScene(sceneIndex);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}