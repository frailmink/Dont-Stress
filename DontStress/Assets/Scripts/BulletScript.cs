using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 10f;
    public float maxDistance = 20f; // Maximum distance before bullet disappears

    private Vector3 startPosition;

    public void Initialize(float damage)
    {
        this.damage = damage;
        Debug.Log("Bullet initialized in BulletScript with damage: " + damage);
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Check distance traveled
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject); // Destroy the bullet on collision
    }
}
