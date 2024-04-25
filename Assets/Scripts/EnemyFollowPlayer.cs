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

    public AudioClip shootSound;
    public AudioClip slashSound;

    [HideInInspector] public AudioSource ShootSource;
    [HideInInspector] public AudioSource SlashSource;

    [Range(0, 1)]
    public float VolumeLevel;
    public float volumeValue;

    private void Start()
    {
        volumeValue = PlayerPrefs.GetFloat("VideoVolume");
        VolumeLevel = volumeValue;
        gameManager = FindAnyObjectByType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        InitializeBulletSpawnPoints();
        transform.position = startPosition.position;
        SetupAudio();
    }

    void SetupAudio()
    {
        GameObject ShootGameObject = new GameObject("ShootAudioSource");
        GameObject SlashGameObject = new GameObject("SlashAudioSource");

        AssignParent(ShootGameObject);
        AssignParent(SlashGameObject);

        ShootSource = ShootGameObject.AddComponent<AudioSource>();
        SlashSource = SlashGameObject.AddComponent<AudioSource>();

        ShootSource.clip = shootSound;
        SlashSource.clip = slashSound;

        ShootSource.volume = VolumeLevel;
        SlashSource.volume = VolumeLevel;

        ShootSource.loop = false;
        SlashSource.loop = false;
    }

    void AssignParent(GameObject obj)
    {
        obj.transform.parent = transform;
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
        volumeValue = PlayerPrefs.GetFloat("VideoVolume");
        VolumeLevel = volumeValue;
        ShootSource.volume = VolumeLevel;
        SlashSource.volume = VolumeLevel;

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
            lastPosition = player.position; // Save player's last known position when starting to chase
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

        if (!hasAttacked)
        {
            if (Vector2.Distance(transform.position, lastPosition) > meleeRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, lastPosition, speed * Time.deltaTime);
            }
            else
            {
                animator.SetTrigger("isAttacking");
                SlashSource.Play();
                StartCoroutine(ResumePatrolAfterMelee());
                hasAttacked = true;
            }
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
        hasAttacked = false;
    }

    public void OnDeath()
    {
        animator.SetBool("isDead", true);
        Destroy(gameObject, 1f);
    }
}
