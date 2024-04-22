using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
	public GameManager gameManager;
	public GameSceneManager gameSceneManager;

	[Header("Health")]
	[Tooltip("The max health the player can have")]
	public int maxHealth = 100;

	[Tooltip("The current health the player has")]
	public int currentHealth;

	[Tooltip("If you're using segameManagerented health, this is gameObject that holds your health icons as its children")]
	public GameObject HealthIcons;

	[Tooltip("This is the Bar of health that you use if you're doing non-segameManagerented health")]
	public GameObject HealthBar;

	private List<GameObject> Hearts = new List<GameObject>();//List of the GameObject hearts that you are using. These need to be in order

	private List<GameObject> TempHearts = new List<GameObject>();

	[Tooltip("If you actually want to use a healthbar or not")]
	public bool useHealthBar = false;

	[Header("Audio")]
	public PlayerAudio playerAudio;

	private Animator anim;
    private SpriteRenderer spriteRender;

    public bool iFrames = false;
    public float iFramesDuration;
    private bool playerDead = false;


	[HideInInspector] public int index = 0; //for editor uses

	void Start()
	{
		SetUpHealth();
		anim = GetComponent<Animator>();
		playerAudio = GetComponent<PlayerAudio>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

	public void SetUpHealth()
	{
		if (!useHealthBar)
		{
			Hearts.Clear();
			TempHearts.Clear();
			foreach (Transform child in HealthIcons.transform)
			{
				child.gameObject.GetComponent<Image>().color = Color.white;//This makes the color to white, you can make this a public variable if you want to change it
				Hearts.Add(child.gameObject);
				TempHearts.Add(child.gameObject);
			}
			currentHealth = TempHearts.Count;
		}
		else
		{
			if (HealthBar)
			{
				HealthBar.GetComponent<Image>().type = Image.Type.Filled;
				// HealthBar.GetComponent<Image>().fillMethod = (int)Image.FillMethod.Horizontal;
				// HealthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
				currentHealth = maxHealth;
				UpdateHealthBar();
			}
		}
	}

    public void DecreaseHealth(int value)//This is the function to use if you want to decrease the player's health somewhere
    {
        //Debug.Log("IFRAMES: Player Hit");
        if (!useHealthBar)
        {
            if (iFrames == false)
            {
                iFrames = true;
                Debug.Log("IFRAMES: Player Hit");
                SegameManagerentedHealthDecrease(value);
                StartCoroutine(dmgFlicker());
                StartCoroutine(iFrameCD());
                return;
            }

        }

        if (iFrames == false)
        {

            currentHealth -= value;
            iFrames = true;
            StartCoroutine(iFrameCD());
        }


        if (playerAudio && !playerAudio.DamageSource.isPlaying && playerAudio.DamageSource.clip != null)
        {
            playerAudio.DamageSource.Play();
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        UpdateHealthBar();
    }

    public void IncreaseHealth(int value)//This is the function to use if you want to increase the player's heath somewhere
	{
		if (!useHealthBar)
		{
			SegameManagerentedHealthIncrease(value);
			return;
		}
		currentHealth += value;
		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}
		UpdateHealthBar();
	}

	private void SegameManagerentedHealthDecrease(int value)//Helper function
	{
		if (playerAudio && !playerAudio.DamageSource.isPlaying && playerAudio.DamageSource.clip != null)
        {
            playerAudio.DamageSource.Play();
        }
		if (value > TempHearts.Count)
		{
			value = TempHearts.Count;
		}
		for (int i = 0; i < value; i++)
		{
			TempHearts[currentHealth - 1].GetComponent<Image>().color = Color.black;
			TempHearts.RemoveAt(TempHearts.Count - 1);
			currentHealth--;
		}

		if (TempHearts.Count == 0)
		{
			currentHealth = 0;
		}
	}

	private void SegameManagerentedHealthIncrease(int value)//Helper function
	{
		if (value + TempHearts.Count > Hearts.Count)
		{
			value = Hearts.Count - TempHearts.Count;
		}

		for (int i = 0; i < value; i++)
		{
			var temp = Hearts[currentHealth];
			temp.GetComponent<Image>().color = Color.white;
			TempHearts.Add(temp);
			currentHealth++;
		}
	}

	public void ResetHealth()//Resets health back to normal
	{
		if (!useHealthBar)
		{
			for (int i = 0; i < Hearts.Count; i++)
			{
				Hearts[i].GetComponent<Image>().color = Color.white;
			}

			TempHearts.Clear();

			foreach (var VARIABLE in Hearts)
			{
				TempHearts.Add(VARIABLE);
			}
			currentHealth = TempHearts.Count;
		}
		else
		{
			currentHealth = maxHealth;
			UpdateHealthBar();
		}
	}

	void UpdateHealthBar()//Updates the health bar according to the new health amounts
	{
		if (useHealthBar)
		{
			float fillAmount = (float)currentHealth / maxHealth;
			if (fillAmount > 1)
			{
				fillAmount = 1.0f;
			}

			HealthBar.GetComponent<Image>().fillAmount = fillAmount;
		}
	}

	//This is where we handle the place where the health is dealth with
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Collider2D thisCollision = GetComponent<Collider2D>();
		if (collision.otherCollider == thisCollision)
		{
			if (collision.gameObject.TryGetComponent(out Damager weapon))
			{
				if (weapon.alignmnent == Damager.Alignment.Enemy ||
					weapon.alignmnent == Damager.Alignment.Environment)
				{
					DecreaseHealth(weapon.damageValue);
					if (currentHealth == 0)
					{
						TimeToDie();
					}
				}
			}
			if (collision.gameObject.TryGetComponent(out HealingItem healingValue))
			{
				IncreaseHealth(healingValue.HealAmount);
				if (healingValue.DestroyOnContact)
				{
					Destroy(collision.gameObject);
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Collider2D thisCollider = GetComponent<Collider2D>();
		if (collision.IsTouching(thisCollider))
		{
			if (collision.gameObject.TryGetComponent(out Damager weapon))
			{
				if ((weapon.alignmnent == Damager.Alignment.Enemy ||
					weapon.alignmnent == Damager.Alignment.Environment) && !playerDead)
				{
					DecreaseHealth(weapon.damageValue);
					if (currentHealth == 0)
					{
						TimeToDie();
					}
				}
			}
			if (collision.gameObject.TryGetComponent(out HealingItem healingValue))
			{
				IncreaseHealth(healingValue.HealAmount);
				if (healingValue.DestroyOnContact)
				{
					Destroy(collision.gameObject);
				}
			}
		}


	}

	private void TimeToDie()
	{
		Debug.Log("Time to Die");
		StartCoroutine(PlayerDies());
	}

    IEnumerator iFrameCD()
    {
        Debug.Log("IFRAMES: COOLDOWN BEGINS");
        yield return new WaitForSeconds(iFramesDuration);
        iFrames = false;
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
        // Begin I-Frame Flicker
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        // Return to Normal
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = Color.white;
    }


    IEnumerator PlayerDies()
{
    if (gameManager != null && gameSceneManager != null)
    {
		
        // Trigger the death animation
        anim.SetBool("isDead", true);
        Debug.Log("Player has died.");
		playerDead = true;
        // Disable player movement
        gameManager.DisablePlayerMovement();
        
        // Play death sound if available
        if (playerAudio && !playerAudio.DeathSource.isPlaying && playerAudio.DeathSource.clip != null)
        {
            playerAudio.DeathSource.Play();
        }
		yield return new WaitForSeconds(0.2f);
		anim.SetBool("isDead", false);
        // Wait for 2 seconds to play tanim.SetBool("isDead", false);he death animation
        yield return new WaitForSeconds(1.3f);

        // After the death animation, reset the isDead state to stop the animation
        anim.SetBool("isDead", false);
        
        // If you have any death cleanup or respawn animation, start it here
        StartCoroutine(gameSceneManager.FadeOut());

        // Wait for the fade out to complete
        yield return new WaitForSeconds(0.5f);
        
        // Respawning the player
        gameManager.Respawn(gameObject);
        
        // Start fading in
        StartCoroutine(gameSceneManager.FadeIn());
        
        // Wait for the fade in to complete before enabling movement
        yield return new WaitForSeconds(0.5f);

        // Reset player's health and status
		playerDead = false;
        ResetHealth();
        gameManager.EnablePlayerMovement();

        // Wait a bit before resetting iFrames
        yield return new WaitForSeconds(0.5f);
        iFrames = false;
    }
    else
    {
        Debug.Log("Game Manager or Game Scene Manager not assigned on player!");
    }
}

}
