using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interactionPrompt; // Assign your UI Text element in the inspector
    public GameObject dialogueUI; // Assign your dialogue UI GameObject

    private void Start()
    {
        interactionPrompt.SetActive(false); // Hide interaction prompt initially
        dialogueUI.SetActive(false); // Ensure dialogue UI is hidden initially
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object is the player
        {
            interactionPrompt.SetActive(true); // Show interaction prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false); // Hide interaction prompt when player leaves
        }
    }

    private void Update()
    {
        if (interactionPrompt.activeInHierarchy && Input.GetKeyDown(KeyCode.E)) // Check if 'E' is pressed when prompt is active
        {
            dialogueUI.SetActive(true); // Show dialogue UI
            interactionPrompt.SetActive(false); // Optionally, hide the interaction prompt
            // Add your dialogue triggering logic here
        }
    }
}
