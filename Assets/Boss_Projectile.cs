using UnityEngine;

public class Boss_Projectile : MonoBehaviour
{
    public float speed = 5f; // Speed of the projectile
    public Animator animator; // Animator component

    private Vector3 targetPosition;
    private bool isSummoned = false;

    public float summonDelay = 3f;
    public float lifetime = 5f; // Lifetime of the projectile in seconds

    private GameManager gameManager;

    private float remainingLifetime; // Remaining lifetime for the projectile

    void Start()
    {
        // Start the summoning animation
        animator.Play("Boss_Proj_Summon");
        gameManager = FindObjectOfType<GameManager>(); // Corrected method name for finding the GameManager
        // Set to launch the projectile after a delay
        Invoke(nameof(LaunchProjectile), summonDelay); // Now using summonDelay for the delay

        remainingLifetime = lifetime; // Initialize remaining lifetime
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
            if (!gameManager.timeIsStopped)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                
                // Update the remaining lifetime only when the time is not stopped
                remainingLifetime -= Time.deltaTime;
                if (remainingLifetime <= 0f)
                {
                    Destroy(gameObject); // Destroy the projectile when its adjusted lifetime is up
                }
            }
        }
    }
}
