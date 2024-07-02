using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingTowerScript : TowerScript
{
    public float maxTimer = 0.1f;
    private float timer = 0f;
    public float slowAmount = 0.2f;
    public float slowDuration = 2f;

    private List<GameObject> enemiesInRange = new List<GameObject>();

    void Start()
    {
        Debug.Log("Placed inside slowing tower");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.gameObject);
            Debug.Log("Slowing tower sees the enemy: " + collision.gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject);
            Debug.Log("No enemies: " + collision.gameObject.name);
        }
    }

    void Update()
    {
        if (timer > maxTimer)
        {
            timer = 0;
            ApplySlowToEnemies();
            Debug.Log("Apply slow?");
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void ApplySlowToEnemies()
    {
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
                if (enemyScript != null)
                {
                    enemyScript.ApplySlow(slowAmount, slowDuration);
                }
            }
        }
    }
}
