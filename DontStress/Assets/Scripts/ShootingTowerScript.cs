using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTowerScript : TowerScript
{
    private float timer = 0f;

    public  float maxTimer;
    public  float bulletSpeed = 10f;
    public  float bulletDamage;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float getTowerDamage (){
        return bulletDamage;
    }

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
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject);
        }
    }

    void Update()
    {
        if (timer > maxTimer)
        {
            timer = 0;
            Shoot();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    public void SetTowerStats(float damage, float speed)
    {
        bulletDamage = damage;
        bulletSpeed = speed;
        Debug.Log("SetTowerStats called. Damage: " + bulletDamage + ", Speed: " + bulletSpeed);
    }

    void Shoot()
    {
        if (enemiesInRange.Count > 0)
        {
            GameObject target = enemiesInRange[0];
            if (target != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                BulletScript towerBulletScript = bullet.GetComponent<BulletScript>();

                // FastShootingTower fastShootingTower = GetComponent<FastShootingTower>();
                // HighDamageShootingTower highDamageShootingTower = GetComponent<HighDamageShootingTower>();
                if (towerBulletScript != null)
                {
                    towerBulletScript.Initialize(bulletDamage * strength);
                    Debug.Log("Bullet initialized in ShootingScript with damage: " + bulletDamage);

                    // if (fastShootingTower != null)
                    // {
                    //     Debug.Log("Fast Bullet Damage: " + fastShootingTower.bulletDamage);
                    // }

                    // if (highDamageShootingTower != null)
                    // {
                    //     Debug.Log("High Damage Bullet Damage: " + highDamageShootingTower.bulletDamage);
                    // }
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
