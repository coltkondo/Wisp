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
    private Rigidbody2D rb; // Rigidbody2D component

    [Header("Audio Settings")]
    [HideInInspector] public AudioSource ShootSource;
    public AudioClip ShootAudioClip;
    public bool LoopShootAudio = false;
    [Range(0, 1)]
    public float VolumeLevel = 1;

    void Start()
    {
        SetupAudio();
        animator.Play("Boss_Proj_Summon");
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody2D>(); // Add Rigidbody2D if not already attached
        }
        rb.gravityScale = 0; // Ensure the projectile does not fall downwards due to gravity
        Invoke(nameof(LaunchProjectile), summonDelay);

        remainingLifetime = lifetime;
    }

    void SetupAudio()
    {
        GameObject ShootGameObject = new GameObject("ShootAudioSource");
        AssignParent(ShootGameObject);
        ShootSource = ShootGameObject.AddComponent<AudioSource>();
        ShootSource.clip = ShootAudioClip;
        ShootSource.volume = VolumeLevel;
        ShootSource.loop = LoopShootAudio;
    }

    void AssignParent(GameObject obj)
    {
        obj.transform.parent = transform;
    }

    void LaunchProjectile()
    {
        targetPosition = GameObject.FindWithTag("Player").transform.position;
        isSummoned = true;
        Vector2 moveDirection = (targetPosition - transform.position).normalized * speed;
        rb.velocity = moveDirection;
        ShootSource.Play();
    }

    void Update()
    {
        if (isSummoned && !gameManager.timeIsStopped)
        {
            // Remaining lifetime update
            remainingLifetime -= Time.deltaTime;
            if (remainingLifetime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("TileMap"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("TileMap"))
        {
            Destroy(gameObject);
        }
    }
}
