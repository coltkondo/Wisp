using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float normalBulletSpeed = 10f;
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private LayerMask whatDestroysBullet;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
        if (transform.rotation.y == 0)
            rb.velocity = transform.right * normalBulletSpeed;
        else
            rb.velocity = -transform.right * normalBulletSpeed;
    }

    private void SetDestroyTimer()
    {
        Destroy(gameObject, destroyTime);
    }
}
