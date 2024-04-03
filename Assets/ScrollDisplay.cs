using UnityEngine;
using UnityEngine.UI; // For UI elements
using TMPro; // Using TextMeshPro for text rendering

public class ScrollDisplay : MonoBehaviour
{
    public GameObject scrollUIPrefab; // Assign your Scroll UI GameObject here
    public TextAsset textFile; // Your text file
    public TMP_Text scrollText; // Reference to the TMP_Text component in your Scroll UI

    private void Awake()
    {
        // Since you already assign scrollText in the Inspector, you might not need to find it at runtime.
        // However, if you prefer to dynamically locate it in your scrollUIPrefab, ensure scrollUIPrefab is set to the correct GameObject in the Inspector.
        // scrollText = scrollUIPrefab.GetComponentInChildren<TMP_Text>();

        // Initially, the scroll UI should be inactive (hidden)
        scrollUIPrefab.SetActive(false);
    }

    private void Update()
    {
        // Check if the left mouse button is clicked and the scroll is currently displayed to hide it
        if (scrollUIPrefab.activeSelf && Input.GetMouseButtonDown(0))
        {
            HideScroll();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DisplayScroll();
        }
    }

    private void DisplayScroll()
    {
        // Load the text file content into the TMP_Text component
        scrollText.text = textFile.text;
        
        // Make the Scroll UI visible
        scrollUIPrefab.SetActive(true);
    }

    // Method to hide the scroll UI
    public void HideScroll()
    {
        // Make the Scroll UI invisible
        scrollUIPrefab.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
