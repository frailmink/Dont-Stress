using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTowerScript : TowerScript
{
    public float maxTimer = 0.1f;
    private float timer = 0f;
    public float bulletSpeed = 1f; 
    public GameObject bulletPrefab;
    public Transform firePoint;

    private List<GameObject> enemiesInRange = new List<GameObject>();

    void Start()
    {
        Debug.Log("Placed inside tower");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.gameObject);
            Debug.Log("Tower sees the enemy: " + collision.gameObject.name);  
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject);
            Debug.Log("Enemies left: " + collision.gameObject.name);
        }
    }

    void Update()
    {
        if (timer > maxTimer)
        {
            timer = 0;
            Shoot();
            Debug.Log("Shoot?");
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void Shoot()
    {
        if (enemiesInRange.Count > 0)
        {
            GameObject target = enemiesInRange[0];
            if (target != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                TowerBulletScript towerBulletScript = bullet.GetComponent<TowerBulletScript>();
                if (towerBulletScript != null)
                {
                    towerBulletScript.Initialize(target.transform, bulletSpeed);
                }

                Vector2 direction = (target.transform.position - firePoint.position).normalized;
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                if (bulletRb != null)
                {
                    bulletRb.velocity = direction * bulletSpeed;
                }
            }
        }
    }
}
//     void Shoot()
//     {
//         if (enemiesInRange.Count > 0)
//         {
//             GameObject target = enemiesInRange[0]; // Choose the first enemy in range
//             if (target != null)
//             {
//                 // Instantiate bullet and shoot it towards the target
//                 GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
//                 TowerBulletScript towerBulletScript = bullet.GetComponent<TowerBulletScript>();
//                 if (towerBulletScript != null)
//                 {
//                     towerBulletScript.Initialize(target.transform, bulletSpeed);
//                 }

//                 // Calculate direction towards the target
//                 Vector2 direction = (target.transform.position - firePoint.position).normalized;

//                 // Set bullet velocity
//                 Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
//                 if (bulletRb != null)
//                 {
//                     bulletRb.velocity = direction * bulletSpeed;
//                 }

//                 // Instantiate bullet and shoot it towards the target
//             //     GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
//             //     while (target.transform.position != null && enemiesInRange.Count > 0)
//             //     {
//             //         enemiesInRange.Remove(target);
//             //     }
//             //     bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(target.transform.position.x - 
//             //     firePoint.position.x, target.transform.position.y - firePoint.position.y).normalized*bulletSpeed;
//             //     // bullet.GetComponent<towerBulletScript>().Initialize(target.transform.position);
//             }
//         }
//     }
// }