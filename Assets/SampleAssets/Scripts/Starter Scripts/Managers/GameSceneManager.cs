using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    //Like the GameManager, this should be it's own gameobject

    [Tooltip("The black screen transition that will be used")]
    public GameObject Transition;

    [Tooltip("If you want to open this scene with a fade in")]
    public bool startWithFadeIn = true;

    private GameObject player;
    private PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerHealth = player.GetComponent<PlayerHealth>();

        if (startWithFadeIn)
        {
            StartCoroutine(FadeIn());
        }
    }


    //This function should be called to other scripts so that way you have the transition working
    public void LoadScene(int SceneIndex)
    {
        StartCoroutine(FadeOut());
        StartCoroutine(LoadAsyncScene(SceneIndex));
    }

    IEnumerator LoadAsyncScene(int SceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        Transition.SetActive(true);
        Debug.Log("Transition Active: " + Transition.activeInHierarchy);
        //yield return new WaitForSeconds(0.1f);
        Transition.GetComponent<Animator>().SetTrigger("StartFadeIn");
        yield return new WaitForSeconds(0.7f);
        //Transition.GetComponent<Animator>().SetTrigger("StartFadeIn");
        //yield return new WaitForSeconds(0.4f);
        Transition.SetActive(false);
    }

    public IEnumerator FadeOut()
    {
        Transition.SetActive(true);
        Transition.GetComponent<Animator>().SetTrigger("StartFadeOut");
        yield return new WaitForSeconds(playerHealth.deathFadeTime);
        //Transition.GetComponent<Animator>().SetBool("StartFadeOut", false);
    }
}
