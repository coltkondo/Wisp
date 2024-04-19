using UnityEngine;
using UnityEngine.UI; // For UI elements
using TMPro; // Using TextMeshPro for text rendering

public class ScrollDisplay : MonoBehaviour
{
    public GameObject scrollUIPrefab; // Assign your Scroll UI GameObject here
    public TextAsset textFile; // Your text file
    public TMP_Text scrollText; // Reference to the TMP_Text component in your Scroll UI

    public GameObject player;

    public AudioSource pageFlipSound;

    private PlayerCombat playerCombat;

    private bool isActive = false;

    private void Start()
    {
        // Initially, the scroll UI should be inactive (hidden)
        pageFlipSound.Stop();
        scrollUIPrefab.SetActive(false);
        isActive = false;
        playerCombat = player.GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        // Toggle the visibility of the scroll UI when the E key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {   
            if (isActive)
            {
                HideScroll();
                playerCombat.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        playerCombat.enabled = false;

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Page flip sound played");
            pageFlipSound.Play();
            DisplayScroll();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerCombat.enabled = true;

        if (other.gameObject.CompareTag("Player"))
        {
                HideScroll();
        }
    }

    private void DisplayScroll()
    {
        // Load the text file content into the TMP_Text component

        scrollText.text = textFile.text;
        pageFlipSound.Play();
        Debug.Log("Page flip sound played");
        // Make the Scroll UI visible and update isActive
        scrollUIPrefab.SetActive(true);
        isActive = true;
        Time.timeScale = 0f;
    }

    // Method to hide the scroll UI
    public void HideScroll()
    {
        // Make the Scroll UI invisible and update isActive

        scrollUIPrefab.SetActive(false);
        isActive = false;
        Debug.Log("Hiding scroll");
        Time.timeScale = 1f;
        // If you don't want to disable the gameObject this script is attached to, comment out or remove the next line
        //this.gameObject.SetActive(false);
    }
}
