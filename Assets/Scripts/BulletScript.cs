using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    private Rigidbody2D bulletRb;
    private Animator animator;
    private bool hasImpacted = false;

    private void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator.SetTrigger("Summon");
        StartCoroutine(StartFollowing(0.5f)); // Delay to simulate summoning animation
        StartCoroutine(DespawnAfterTimeout(5f)); // Despawn after 5 seconds if no impact
    }

    private IEnumerator StartFollowing(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target != null && !hasImpacted)
        {
            Vector2 moveDirection = (target.position - transform.position).normalized * speed;
            bulletRb.velocity = moveDirection;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Ground"))
        {
            Impact();
        }
    }

    private void Impact()
    {
        hasImpacted = true;
        bulletRb.velocity = Vector2.zero; // Stop the bullet movement
        animator.SetTrigger("Impact");
        Destroy(gameObject, 0.5f); // Destroy after impact animation
    }

    private IEnumerator DespawnAfterTimeout(float timeout)
    {
        yield return new WaitForSeconds(timeout);
        if (!hasImpacted)
        {
            animator.SetTrigger("DeSummon");
            Destroy(gameObject, 0.5f); // Allow time for de-summon animation
        }
    }
}

