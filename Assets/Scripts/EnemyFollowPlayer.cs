using UnityEngine;
using System.Collections;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform startPosition; // Use Transform to set in the Inspector
    [SerializeField] private Transform endPosition;   // Use Transform to set in the Inspector
    private int travelCount = 0;
    private bool movingToEnd = true;

    [SerializeField] private float lineOfSight = 10f;
    [SerializeField] private float meleeRange = 2f;
    private Transform[] bulletSpawnPoints;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnsParent;
    [SerializeField] private Animator animator;
    private Transform player;
    private bool isFiring = false;
    private bool isChasingPlayer = false;
    private bool isMoving = true; // Flag to control movement
    private bool hasAttacked = false; // Flag to prevent multiple attacks

    private Vector3 lastPosition; // To track movement direction

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        InitializeBulletSpawnPoints();
        transform.position = startPosition.position; // Start at the initial position
        lastPosition = transform.position;
    }

    private void InitializeBulletSpawnPoints()
    {
        bulletSpawnPoints = new Transform[bulletSpawnsParent.childCount];
        for (int i = 0; i < bulletSpawnsParent.childCount; i++)
        {
            bulletSpawnPoints[i] = bulletSpawnsParent.GetChild(i);
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else if (isMoving)
        {
            MoveBetweenPoints();
        }
    }

    private void MoveBetweenPoints()
    {
        Vector3 targetPosition = movingToEnd ? endPosition.position : startPosition.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f && !isFiring)
        {
            StartCoroutine(SummonBullets());
            isMoving = false; // Stop moving while summoning
        }
    }

    private IEnumerator SummonBullets()
    {
        FacePlayer(); // Adjust orientation to face the player before summoning
        isFiring = true;
        animator.SetTrigger("Summon");
        foreach (Transform spawnPoint in bulletSpawnPoints)
        {
            Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(1); // Cooldown between bullets
        }
        isFiring = false;
        isMoving = true; // Resume movement after summoning

        if (++travelCount >= 3) // Complete two cycles
        {
            isChasingPlayer = true; // Start chasing the player
            hasAttacked = false; // Reset attack flag when starting chase
        }
        else
        {
            movingToEnd = !movingToEnd; // Toggle the movement direction
        }
    }

    private void ChasePlayer()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer > meleeRange)
        {
            Vector3 floatPosition = new Vector3(player.position.x, player.position.y + Random.Range(-1f, 1f), player.position.z);
            transform.position = Vector2.MoveTowards(transform.position, floatPosition, speed * Time.deltaTime);
             // Player has moved out of range, reset attack flag
        }
        else if (!hasAttacked)
        {
            animator.SetTrigger("isAttacking"); // Play melee attack animation
            StartCoroutine(ResumePatrolAfterMelee());
            hasAttacked = true; // Ensure the attack only triggers once per engagement
        }
    }

    private void FacePlayer()
    {
        if (player == null) return;
        Vector3 directionToPlayer = player.position - transform.position;
        float xDirection = Mathf.Sign(directionToPlayer.x);
        Vector3 localScale = transform.localScale;
        float absoluteScaleX = Mathf.Abs(localScale.x);
        transform.localScale = new Vector3(xDirection * absoluteScaleX, localScale.y, localScale.z);
    }

    private IEnumerator ResumePatrolAfterMelee()
    {
        yield return new WaitForSeconds(1.2f); // Wait for the melee animation to finish
        isChasingPlayer = false; // Reset to start patrolling again
        travelCount = 0; // Reset travel count to restart the patrol cycles
        isMoving = true; // Allow movement again
        hasAttacked = true;
    }

    public void OnDeath()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject, 1f); // Wait for death animation to finish
    }
}
