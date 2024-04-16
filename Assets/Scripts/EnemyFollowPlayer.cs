using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lineOfSight = 10f;
    [SerializeField] private float shootingRange = 5f;
    [SerializeField] private float fireRate = 1f;
    private float nextFireTime;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Animator animator;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSight && distanceFromPlayer > shootingRange)
        {
            MoveTowardsPlayer();
        }
        else if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
        {
            SummonBullet();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Random floating logic (up and down movement)
        Vector3 floatPosition = new Vector3(player.position.x, player.position.y + Random.Range(-1f, 1f), player.position.z);
        transform.position = Vector2.MoveTowards(transform.position, floatPosition, speed * Time.deltaTime);
        animator.SetTrigger("Move");
    }

    private void SummonBullet()
    {
        animator.SetTrigger("Summon");
        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        nextFireTime = Time.time + fireRate;
    }

    public void OnDeath()
    {
        animator.SetTrigger("Die");
        // Optional: Destroy or disable enemy game object after animation
        Destroy(gameObject, 1f); // Wait for death animation to finish
    }
}

