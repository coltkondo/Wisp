using UnityEngine;
using System.Collections;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform startPosition; 
    [SerializeField] private Transform endPosition;   
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
    private bool isMoving = true; 
    private bool hasAttacked = false; 

    private GameManager gameManager;
    private Vector3 lastPosition; 

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        InitializeBulletSpawnPoints();
        transform.position = startPosition.position;
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
        if (player == null || animator.GetBool("isDead")) return;

        if (isChasingPlayer && !gameManager.timeIsStopped)
        {
            ChasePlayer();
        }
        else if (isMoving && !gameManager.timeIsStopped)
        {
            MoveBetweenPoints();
        }
    }

    private void MoveBetweenPoints()
    {
        if (animator.GetBool("isDead")) return;

        Vector3 targetPosition = movingToEnd ? endPosition.position : startPosition.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f && !isFiring && !gameManager.timeIsStopped)
        {
            StartCoroutine(SummonBullets());
            isMoving = false;
        }
    }

    private IEnumerator SummonBullets()
    {
        if (animator.GetBool("isDead")) yield break;

        FacePlayer();
        isFiring = true;
        animator.SetTrigger("Summon");
        foreach (Transform spawnPoint in bulletSpawnPoints)
        {
            Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        isFiring = false;
        isMoving = true;

        if (++travelCount >= 3)
        {
            isChasingPlayer = true;
            hasAttacked = false;
        }
        else
        {
            movingToEnd = !movingToEnd;
        }
    }

    private void ChasePlayer()
    {
        if (animator.GetBool("isDead")) return;

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer > meleeRange)
        {
            Vector3 floatPosition = new Vector3(player.position.x, player.position.y + Random.Range(-1f, 1f), player.position.z);
            transform.position = Vector2.MoveTowards(transform.position, floatPosition, speed * Time.deltaTime);
        }
        else if (!hasAttacked)
        {
            animator.SetTrigger("isAttacking");
            StartCoroutine(ResumePatrolAfterMelee());
            hasAttacked = true;
        }
    }

    private void FacePlayer()
    {
        if (player == null || animator.GetBool("isDead")) return;
        Vector3 directionToPlayer = player.position - transform.position;
        float xDirection = Mathf.Sign(directionToPlayer.x);
        Vector3 localScale = transform.localScale;
        float absoluteScaleX = Mathf.Abs(localScale.x);
        transform.localScale = new Vector3(xDirection * absoluteScaleX, localScale.y, localScale.z);
    }

    private IEnumerator ResumePatrolAfterMelee()
    {
        if (animator.GetBool("isDead")) yield break;

        yield return new WaitForSeconds(1.2f);
        isChasingPlayer = false;
        travelCount = 0;
        isMoving = true;
        hasAttacked = true;
    }

    public void OnDeath()
    {
        animator.SetBool("isDead", true); // Set isDead to true when the enemy dies
        Destroy(gameObject, 1f); // Wait for death animation to finish
    }
}
