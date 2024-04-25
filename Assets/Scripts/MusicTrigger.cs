using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioSource audioSourceA; // Assign through the inspector
    public AudioSource audioSourceB; // Assign through the inspector
    public float fadeTime = 1.5f;    // Duration of the fade in seconds

    public float triggerCooldown;

    private bool onCooldown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the player has a tag "Player"
        {
            // Player is moving from B to A
            if (audioSourceA.isPlaying == false)
            {
                Debug.Log("Playing Track A");
                StartCoroutine(FadeOut(audioSourceB, fadeTime));
                StartCoroutine(FadeIn(audioSourceA, fadeTime));
            }
            

            /*Debug.Log("Cooldown Status: " + onCooldown + "");
            if (onCooldown == false)
            {
                // Determine the direction of the player
                if (transform.position.x < other.transform.position.x)
                {
                    // Player is moving from A to B
                    onCooldown = true;
                    Debug.Log("Playing Track B");
                    StartCoroutine(FadeOut(audioSourceA, fadeTime));
                    StartCoroutine(FadeIn(audioSourceB, fadeTime));
                    StartCoroutine(TriggerCD());

                }
                else
                {
                    // Player is moving from B to A
                    onCooldown = true;
                    Debug.Log("Playing Track A");
                    StartCoroutine(FadeOut(audioSourceB, fadeTime));
                    StartCoroutine(FadeIn(audioSourceA, fadeTime));
                    StartCoroutine(TriggerCD());
                }
            }*/
            
        }
    }

    private IEnumerator TriggerCD()
    {
        onCooldown = true;
        yield return new WaitForSeconds(triggerCooldown);
        onCooldown = false;
    }

     private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        audioSource.Play();
        audioSource.volume = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, 0.3f, currentTime / duration);
            yield return null;
        }
        //gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
            yield return null;
        }
        audioSource.Stop();
    }
}
