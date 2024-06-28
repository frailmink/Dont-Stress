using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighDamageShootingTower : ShootingTowerScript
{
    // public GameObject HighDamageBulletPrefab;
    private void Start()
    {
        bulletDamage = 100f;
        bulletSpeed = 10f;
        // bulletPrefab = HighDamageBulletPrefab;
        SetTowerStats(bulletDamage, bulletSpeed);
        Debug.Log("HighDamageShootingTower - bulletDamage: " + bulletDamage);
        Debug.Log("HighDamageShootingTower - bulletSpeed: " + bulletSpeed);
    }
}
