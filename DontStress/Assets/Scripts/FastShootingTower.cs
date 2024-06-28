using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastShootingTower : ShootingTowerScript
{   
    // public GameObject FastBulletPrefab;
    private void Start()
    {
        bulletDamage = 1f;
        bulletSpeed = 10f;
        // bulletPrefab = FastBulletPrefab;
        SetTowerStats(bulletDamage, bulletSpeed);
        Debug.Log("FastShootingTower - bulletDamage: " + bulletDamage);
        Debug.Log("FastShootingTower - bulletSpeed: " + bulletSpeed);
        
    }
}
