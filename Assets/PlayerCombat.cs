using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    private Vector2 worldPosition;
    private Vector2 lookDirection;
    private float angle;
    private GameObject bulletInstance;

    [Header("Audio")]
    public PlayerAudio playerAudio;

    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleShooting();

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                if (playerAudio && !playerAudio.AttackSource.isPlaying && playerAudio.AttackSource.clip != null)
                {
                    playerAudio.AttackSource.Play();
                }
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        Debug.Log("hit enemies: " + hitEnemies.Length);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit enemy: " + enemy.name);
            enemy.GetComponent<EnemyHealth>().DecreaseHealth(attackDamage);

        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            bulletInstance = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        }
    }
}
