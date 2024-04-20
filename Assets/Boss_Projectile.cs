using UnityEngine;

public class Boss_Projectile : MonoBehaviour
{
    public float speed = 5f;  // Speed of the projectile
    public Animator animator; // Animator component

    private Vector3 targetPosition;
    private bool isSummoned = false;

    public float summonDelay = 3f;
    public float lifetime = 5f;     // Lifetime of the projectile in seconds

    private GameManager gameManager;

    void Start()
    {
        // Start the summoning animation
        animator.Play("Boss_Proj_Summon");
        gameManager = FindAnyObjectByType<GameManager>();
        // Set to launch the projectile after a delay
        Invoke(nameof(LaunchProjectile), 1f); // Wait for 1 second

        Destroy(gameObject, lifetime);
        
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
            if(!gameManager.timeIsStopped){
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
    }
}
