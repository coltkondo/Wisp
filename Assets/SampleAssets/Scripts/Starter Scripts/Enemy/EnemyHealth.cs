using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Settings")]
    public bool EnemyHealthBar = false;
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Health Bar for enemy")]
    public float padding = 2f;
    public Vector2 Dimensions;
    public GameObject HealthBar;

    public GameObject HealthBar_fill;
    public RectTransform canvasRectTransform;

    [Header("Item Drop Settings")]
    public GameObject itemPrefab; // The prefab for the item to drop
    public int numberOfItemsToDrop = 3; // The number of items to drop

    private Image healthBarImage;
    private RectTransform healthRectTransform;
    private Animator anim;
    private SpriteRenderer spriteRender;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        if (EnemyHealthBar)
        {
            SetupHealthBar();
        }
    }

    void Update()
    {
        
    }

    public void DecreaseHealth(int value)
    {

        currentHealth -= value;
        StartCoroutine(dmgFlicker());
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
        if (EnemyHealthBar)
            UpdateHealthBar();
    }

    private IEnumerator DestroyAfterAnimation()
    {
        
        yield return new WaitForSeconds(2.0f); // Adjust the delay based on your death animation duration
		DropItems(); // Drop items before destroying the enemy
        Destroy(gameObject);
        if (EnemyHealthBar) Destroy(healthBarImage.gameObject);
    }

    IEnumerator dmgFlicker()
    {
        // Red Damage Flicker
        spriteRender.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = Color.red;
        // Return to Normal
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = Color.white;
    }

    private void UpdateHealthBar()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        HealthBar_fill.GetComponent<Image>().fillAmount = fillAmount > 1 ? 1.0f : fillAmount;
    }

    private void SetupHealthBar()
    {
        HealthBar.SetActive(true);
        //HealthBar.GetComponent<Image>().type = Image.Type.Filled;
        UpdateHealthBar();
    }


    private void HandleDeath()
    {
        currentHealth = 0;
		CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        if (rb != null)
        {
            rb.Sleep();
        }
        anim.SetBool("isDead", true);
        StartCoroutine(DestroyAfterAnimation());
    }

    private void DropItems()
    {
        for (int i = 0; i < numberOfItemsToDrop; i++)
        {
            // Adjust the spawn position if necessary to prevent items from overlapping
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
    }


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out Damager weapon))
		{
			if (weapon.alignmnent == Damager.Alignment.Player || weapon.alignmnent == Damager.Alignment.Environment)
			{
				DecreaseHealth(weapon.damageValue);

				if (EnemyHealthBar)
					UpdateHealthBar();
			}
		}
	}
}
