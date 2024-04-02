using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float normalBulletSpeed = 10f;
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private LayerMask whatDestroysBullet;
    private Rigidbody2D rb;
    private Transform playerTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        SetDestroyTimer();
        SetStraightVelocity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            //TODO: Add explosion effect
            //TODO: Add sound effect

            Destroy(gameObject);
        }
    }

private void SetStraightVelocity()
{
    // Get the player's scale
    Vector2 playerScale = playerTransform.localScale;

    // Determine the direction based on the player's scale
    Vector2 direction = playerScale.x > 0 ? Vector2.right : Vector2.left;
//
    // Apply velocity
    rb.velocity = direction * normalBulletSpeed;
}
    private void SetDestroyTimer()
    {
        Destroy(gameObject, destroyTime);
    }
}
