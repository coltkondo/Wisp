using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioSource audioSourceA; // Assign through the inspector
    public AudioSource audioSourceB; // Assign through the inspector
    public float fadeTime = 1.5f;    // Duration of the fade in seconds

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the player has a tag "Player"
        {
            // Determine the direction of the player
            if (transform.position.x < other.transform.position.x)
            {
                // Player is moving from A to B
                StartCoroutine(FadeOut(audioSourceA, fadeTime));
                StartCoroutine(FadeIn(audioSourceB, fadeTime));
            }
            else
            {
                // Player is moving from B to A
                StartCoroutine(FadeOut(audioSourceB, fadeTime));
                StartCoroutine(FadeIn(audioSourceA, fadeTime));
            }
        }
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
