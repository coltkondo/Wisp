using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject player;
    public float speed;
    public float distanceBetween;

    private float distance;
    public LayerMask groundLayer; // Assign a layer mask for the ground in the inspector
    public float groundCheckDistance = 0.2f; // Distance to check for the ground

    public float stopOffsetX;

    private bool facingRight = true;

    private Animator animator;
    private Vector2 lastPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        // Check if we are grounded
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        bool grounded = hit.collider != null;

        if(grounded)
        {
            if((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
            {
                facingRight = !facingRight; // Change the facing direction
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                stopOffsetX = -stopOffsetX; // Reverse the offset
            }

            // Here's the change: we keep the NPC's current y position instead of using the player's y position
            Vector2 targetPosition = new Vector2(player.transform.position.x + stopOffsetX, transform.position.y);

            // Move towards the player until within offset distance
            if(distance > Mathf.Abs(stopOffsetX))
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }

        float conditionSpeed = (transform.position - (Vector3)lastPosition).magnitude / Time.deltaTime;
        animator.SetFloat("Speed", conditionSpeed);
        lastPosition = transform.position;
    }
}
