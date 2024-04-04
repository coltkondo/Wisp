using UnityEngine;

public class TimeCollectible : MonoBehaviour
{
    [SerializeField] public int timePointsToAdd = 1; // Amount of time points this collectible gives

    private GameObject player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.tag == "Player") // Make sure your player GameObject has the tag "Player"
        {
            this.player = GameObject.FindWithTag("Player");

            Debug.Log("Got player collision");
            // Find the Timestop_Eric component and call increaseTimePoints
            Timestop_Eric timeControl = player.GetComponent<Timestop_Eric>();
            if (timeControl != null)
            {
                Debug.Log("doing increase time");
                this.gameObject.SetActive(false);
                Debug.Log("ITEM SHOULD GET DESTROYED");
                timeControl.increaseTimePoints(timePointsToAdd);
                
            }
            // Optionally, destroy the collectible after it's been collected
            
        }
    }
}
