using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyScript : MonoBehaviour
{
    public Queue<Vector2> path;

    public Vector2 objective;

    private Rigidbody2D rb;

    public Tilemap map;

    public float speed;
    public float maxHealth = 100f;
    private float health;
    private PlayerHealthScript playerHealth;
    public int damage = 1;

    private BaseHealthScript baseHealth; 
    private EnemyHealthBar EnemyHealthBar;

    public GameObject canvas;
    
    private void Awake() 
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthScript>();
        baseHealth = GameObject.FindGameObjectWithTag("Base").GetComponent<BaseHealthScript>();

        EnemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        health = maxHealth;
        canvas.SetActive(false);
    }

    public void TakeDamage(float damageAmount)
    {
        canvas.SetActive(true);
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

    private void Start()
    {
        
        EnemyHealthBar.UpdateHealth(health,maxHealth);
        rb = GetComponent<Rigidbody2D>();
        // Ensure the initial objective is set at the start
        if (path != null && path.Count > 0)
        {
            Vector2 temp = path.Dequeue();
            objective = map.CellToWorld(new Vector3Int((int)temp.x, (int)temp.y, 0));
        }
    }

    // Update is called once per frame
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(damage);
        }
        if(collision.gameObject.tag == "Base")
        {
            Destroy(gameObject);
            baseHealth.TakeDamage(damage);
        }
    }

}
