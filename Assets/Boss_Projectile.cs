using UnityEngine;

public class Boss_Projectile : MonoBehaviour
{
    public float speed = 5f; // Speed of the projectile

    private float speedHolder;

    public Animator animator; // Animator component

    private GameManager gameManager;

    private float remainingLifetime; // Remaining lifetime for the projectile
    private Rigidbody2D rb; // Rigidbody2D component
    private Vector2 storedVelocity; // To store the velocity when time is stopped
    private bool isFrozen = false; // Flag to check if projectile is currently frozen

    [Header("Audio Settings")]
    [HideInInspector] public AudioSource ShootSource;
    public AudioClip ShootAudioClip;
    public bool LoopShootAudio = false;
    [Range(0, 1)]
    public float VolumeLevel = 1;

    private bool isLaunched = false;

    void Start()
    {
        SetupAudio();
        animator.Play("Boss_Proj_Summon");
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0;
        Invoke(nameof(LaunchProjectile), 3f); // Use the summonDelay directly here if variable required

        remainingLifetime = 5f; // Use the lifetime directly here if variable required
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
        isLaunched = true;
        Vector2 moveDirection = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized * speed;
        rb.velocity = moveDirection;
        ShootSource.Play();
    }

    void FixedUpdate()
    {
        if (!gameManager.timeIsStopped && isFrozen)
        {
            // Resume movement with the stored velocity
            Debug.Log("BOSS BULLETS RESUME");
            rb.isKinematic = false;
            speed = speedHolder;
            rb.velocity = storedVelocity;
            isFrozen = false;
        }
        else if (gameManager.timeIsStopped && !isFrozen)
        {
            // Store the current velocity and stop movement
            if (isLaunched == true)
            {
                storedVelocity = rb.velocity;
                speedHolder = speed;
                speed = 0f;
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                isFrozen = true;
            } else
            {
                Vector2 moveDirection = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized * speed;
                storedVelocity = moveDirection;
                speedHolder = speed;
                speed = 0f;
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                isFrozen = true;
            }
            
        }

        if (!gameManager.timeIsStopped)
        {
            // Continue counting down lifetime only when time is not stopped
            remainingLifetime -= Time.fixedDeltaTime;
            if (remainingLifetime <= 0f)
            {
                isLaunched = false;
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
