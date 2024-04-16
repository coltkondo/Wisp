using UnityEngine;

public class Boss_Projectile : MonoBehaviour
{
    public float speed = 5f;  // Speed of the projectile
    public Animator animator; // Animator component

    private Vector3 targetPosition;
    private bool isSummoned = false;

    void Start()
    {
        // Start the summoning animation
        animator.Play("SummoningAnimation");
        // Set to launch the projectile after a delay
        Invoke(nameof(LaunchProjectile), 1f); // Wait for 1 second
    }

    void LaunchProjectile()
    {
        // Set the target position to the player's current position at the time of launch
        targetPosition = GameObject.FindWithTag("Player").transform.position;
        isSummoned = true;
    }

    void Update()
    {
        if (isSummoned)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
