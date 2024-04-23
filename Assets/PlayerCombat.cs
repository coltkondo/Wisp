using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    public Animator animator;
    public GameObject manager;
    public Transform attackPoint;
    public Transform attackPointAbove; // New attack point for attacking above
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;

    [Header("Attack Rate")]
    public float attackRate = 2f;  // Rate at which the player can melee attack
    float nextAttackTime = 0f;

    [Header("Shooting")]
    public float shootCooldown = 1f;  // Cooldown time between shots
    private float nextShootTime = 0f;  // When the player is allowed to shoot again

    private GameObject bulletInstance;

    [Header("Audio")]
    public PlayerAudio playerAudio;

    private DialogueManager diaManager;

    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        diaManager = manager.GetComponent<DialogueManager>();

    }

    private bool isHoldingW = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            isHoldingW = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            isHoldingW = false;
        }

        if (diaManager.isTalking == false)
        {
            HandleShooting();

            if (Time.time >= nextAttackTime)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    playerAudio.AttackSource.Play();
                    if (isHoldingW)
                    {
                        AttackAbove();
                    }
                    else
                    {
                        Attack();
                    }
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }

        }
        
    }

    void AttackAbove()
    {
        animator.SetTrigger("AttackAbove");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointAbove.position, attackRange, enemyLayers);

        Debug.Log("Hit enemies above: " + hitEnemies.Length);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().DecreaseHealth(attackDamage);
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        Debug.Log("Hit enemies: " + hitEnemies.Length);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().DecreaseHealth(attackDamage);
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(1) && Time.time >= nextShootTime)
        {
            bulletInstance = Instantiate(bullet, attackPoint.position, Quaternion.identity);
            nextShootTime = Time.time + shootCooldown;

            if (playerAudio && !playerAudio.ShootSource.isPlaying && playerAudio.ShootSource.clip != null)
            {
                playerAudio.ShootSource.Play();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        
        if (attackPointAbove != null)
            Gizmos.DrawWireSphere(attackPointAbove.position, attackRange);
    }
}
