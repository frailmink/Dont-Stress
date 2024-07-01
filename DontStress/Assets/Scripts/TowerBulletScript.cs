using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBulletScript : MonoBehaviour
{
    private Transform target;
    private float speed;
    public float damage;

    // Initialize the bullet with its target, speed, and damage.
    public void Initialize(Transform target, float speed, float damage)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        Debug.Log("Bullet initialized in BulletScript with damage: " + damage);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(direction * distanceThisFrame, Space.World);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Bullet collided with enemy: " + collision.gameObject.name);
            EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}



// using UnityEngine;/////////////////////////////////////////////////////////////////////////////////////

// public class TowerBulletScript : MonoBehaviour
// {
//     public float damage = 10f; // Damage amount
//     public float maxDistance = 20f; // Maximum distance before bullet disappears

//     private Transform target;
//     private float bulletSpeed;

//     private Vector3 startPosition;

//     public void Initialize(Transform target, float bulletSpeed)
//     {
//         this.target = target;
//         this.bulletSpeed = bulletSpeed;
//         startPosition = transform.position;
//     }

//     void Update()
//     {
//         if (target == null)
//         {
//             Destroy(gameObject); // Destroy bullet if the target is null
//             return;
//         }

//         // Move bullet towards the target
//         Vector3 direction = (target.position - transform.position).normalized;
//         transform.position += direction * bulletSpeed * Time.deltaTime;

//         // Check distance traveled
//         if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
//         {
//             Destroy(gameObject);
//         }
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
//         if (enemy != null)
//         {
//             enemy.TakeDamage(damage);
//         }

//         Destroy(gameObject); // Destroy the bullet on collision
//     }
// }

