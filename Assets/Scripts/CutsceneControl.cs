using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class CutsceneControl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button skipButton;
    public Image fadeImage;
    public Image pauseImage;
    public float fadeSpeed = 1.0f;
    public float textFade = 1.0f;
    public bool isFading = false;

    public bool buttonActive = false;

    public bool isPaused = false;



    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.Stop();
        videoPlayer.time = 0;
        videoPlayer.Prepare();

        StartCoroutine(BeginCutsceneAfterDelay(1.5f));
        videoPlayer.loopPointReached += EndReached;
        skipButton.onClick.AddListener(OnSkipPressed);    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !buttonActive) {
            ActivateButton();
        }

        if (isFading) {
            float alpha = fadeImage.color.a + (Time.deltaTime / fadeSpeed);
            fadeImage.color = new Color(0, 0, 0, alpha); 

            if (fadeImage.color.a >= 1) {
                SceneManager.LoadScene(2);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                videoPlayer.Play();
                pauseImage.gameObject.SetActive(false);
                isPaused = false;
            } else {
                videoPlayer.Pause();
                pauseImage.gameObject.SetActive(true);
                isPaused = true;
            }
        }
    }

    public void OnSkipPressed() {
        isFading = true;
        videoPlayer.Stop();
    }

    IEnumerator BeginCutsceneAfterDelay(float delay)
    {
        videoPlayer.Stop();
        yield return new WaitForSeconds(delay); // Wait for 5 seconds

        if (videoPlayer != null) {
            videoPlayer.Play();
        }

    }

    void EndReached(VideoPlayer vp)
    {
        isFading = true;
        SceneManager.LoadScene(2);
    }

    void ActivateButton()
    {
        skipButton.gameObject.SetActive(true);
        buttonActive = true;
        TextMeshProUGUI buttonText = skipButton.GetComponentInChildren<TextMeshProUGUI>();
        Color startColor = buttonText.color;
        startColor.a = 0;
        buttonText.color = startColor;
        StartCoroutine(FadeInButton(buttonText, textFade)); 
    }
    IEnumerator FadeInButton(TextMeshProUGUI text, float fadeSpeed)
    {
        Color c = text.color;
        float time = 0;

        while (time < fadeSpeed) {
            time += Time.deltaTime;
            c.a += Time.deltaTime / fadeSpeed;
            text.color = c;
            yield return null;
        }
    }

}
