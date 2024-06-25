using UnityEngine;

public class TowerBulletScript : MonoBehaviour
{
    public float damage = 10f; // Damage amount
    public float maxDistance = 20f; // Maximum distance before bullet disappears

    private Transform target;
    private float bulletSpeed;

    private Vector3 startPosition;

    public void Initialize(Transform target, float bulletSpeed)
    {
        this.target = target;
        this.bulletSpeed = bulletSpeed;
        startPosition = transform.position;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destroy bullet if the target is null
            return;
        }

        // Move bullet towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * bulletSpeed * Time.deltaTime;

        // Check distance traveled
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject); // Destroy the bullet on collision
    }
}
