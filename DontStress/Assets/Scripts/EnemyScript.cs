using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyScript : MonoBehaviour
{
    #region declaring variables 
    public Queue<Vector2> path;
    public Vector2 objective;
    public Tilemap map;

    private Rigidbody2D rb;

    public GameObject enemyPrefab;
    public float speed;
    public float maxHealth;
    private float health;
    public int damageToBase = 10;

    private PlayerHealthScript playerHealth;
    private BaseHealthScript baseHealth; 
    private EnemyHealthBar EnemyHealthBar; 
    #endregion

    private void Awake() 
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthScript>();
        baseHealth = GameObject.FindGameObjectWithTag("Base").GetComponent<BaseHealthScript>();

        EnemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        if (path != null && path.Count > 0)
        {
            Vector2 temp = path.Dequeue();
            objective = map.CellToWorld(new Vector3Int((int)temp.x, (int)temp.y, 0));
        }
        EnemyHealthBar.UpdateHealth(health, maxHealth);
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, objective) > 0.1f)
        {
            Vector2 dif = objective - (Vector2)transform.position;
            rb.velocity = dif.normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop the enemy's movement

            if (path != null && path.Count > 0)
            {
                Vector2 temp = path.Dequeue();
                objective = map.CellToWorld(new Vector3Int((int)temp.x, (int)temp.y, 0));
            }
            else
            {   

                Die();
            }
        }
    }

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy.GetComponent<EnemyScript>().path = new Queue<Vector2>(path);
        enemy.GetComponent<EnemyScript>().map = map;
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(damageToBase);
        }
        if(collision.gameObject.tag == "Base")
        {
            Destroy(gameObject);
            baseHealth.TakeDamage(damageToBase);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        EnemyHealthBar.UpdateHealth(health,maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}
