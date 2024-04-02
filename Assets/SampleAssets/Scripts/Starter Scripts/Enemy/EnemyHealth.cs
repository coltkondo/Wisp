﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Settings")]
    public bool EnemyHealthBar = false;
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Health Bar")]
    public float padding = 2f;
    public Vector2 Dimensions;
    public GameObject HealthBar;
    public RectTransform canvasRectTransform;

    [Header("Item Drop Settings")]
    public GameObject itemPrefab; // The prefab for the item to drop
    public int numberOfItemsToDrop = 3; // The number of items to drop

    private Image healthBarImage;
    private RectTransform healthRectTransform;
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        if (EnemyHealthBar)
        {
            SetupHealthBar();
        }
    }

    void Update()
    {
        if (EnemyHealthBar)
        {
            UpdateHealthBarPosition();
        }
	
    }

    public void DecreaseHealth(int value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
        if (EnemyHealthBar)
            UpdateHealthBar();
    }

    private IEnumerator DestroyAfterAnimation()
    {
        DropItems(); // Drop items before destroying the enemy
        yield return new WaitForSeconds(2.0f); // Adjust the delay based on your death animation duration
        Destroy(gameObject);
        if (EnemyHealthBar) Destroy(healthBarImage.gameObject);
    }

    private void UpdateHealthBar()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        healthBarImage.fillAmount = fillAmount > 1 ? 1.0f : fillAmount;
    }

    private void SetupHealthBar()
    {
        if (canvasRectTransform == null)
            canvasRectTransform = GameObject.FindGameObjectWithTag("EnemyHealthCanvas").GetComponent<RectTransform>();

        GameObject newHealthBar = Instantiate(HealthBar, transform.position, Quaternion.identity);
        healthRectTransform = newHealthBar.GetComponent<RectTransform>();
        newHealthBar.transform.SetParent(canvasRectTransform);
        healthBarImage = newHealthBar.GetComponent<Image>();
        healthBarImage.type = Image.Type.Filled;
        healthRectTransform.sizeDelta += Dimensions;
        UpdateHealthBar();
    }

    private void UpdateHealthBarPosition()
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        healthRectTransform.anchoredPosition = screenPoint - canvasRectTransform.sizeDelta / 2f + new Vector2(0f, padding);
    }

    private void HandleDeath()
    {
        currentHealth = 0;
        anim.SetBool("isDead", true);
        StartCoroutine(DestroyAfterAnimation());
    }

    private void DropItems()
    {
        for (int i = 0; i < numberOfItemsToDrop; i++)
        {
            // Adjust the spawn position if necessary to prevent items from overlapping
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Collision detected");
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
