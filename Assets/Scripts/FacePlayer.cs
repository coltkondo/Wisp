using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform player; // Assign this in the inspector, or find it dynamically

    void Start()
    {
        if (player == null)
        {
            // Find the player by tag if not assigned
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        // Determine the direction by comparing positions
        Vector3 direction = player.position - transform.position;
        
        // If player is to the right of NPC
        if (direction.x > 0)
        {
            // Face right (assuming the sprite faces right by default)
            transform.localScale = new Vector3(-2, 2, 1);
        }
        else
        {
            // Face left
            transform.localScale = new Vector3(2, 2, 1);
        }
    }
}
