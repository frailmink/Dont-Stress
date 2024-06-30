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
    public int damage = 1;

    private PlayerHealthScript playerHealth;
    private BaseHealthScript baseHealth; 
    private EnemyHealthBar enemyHealthBar;

    public GameObject canvas;

    private bool shot = false;
    private float timer = 0f;
    private float maxTimer = 1f;
    #endregion

    private void Awake() 
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthScript>();
        baseHealth = GameObject.FindGameObjectWithTag("Base").GetComponent<BaseHealthScript>();

        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        health = maxHealth;
        canvas.SetActive(false);
    }

    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        if (path != null && path.Count > 0)
        {
            Vector2 temp = path.Dequeue();
            objective = map.GetCellCenterWorld(new Vector3Int((int)temp.x, (int)temp.y, 0));
        }
        enemyHealthBar.UpdateHealth(health, maxHealth);
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, objective) > 0.1f)
        {
            if (timer > maxTimer)
            {
                rb.velocity = Vector2.zero;
                timer = 0;
                shot = false;
            } else if (shot)
            {
                timer += Time.deltaTime;
            }

            float maxDistanceDelta = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, objective, maxDistanceDelta);
            // Vector2 dif = objective - (Vector2)transform.position;
            // rb.velocity = dif.normalized * speed;
        }
        else
        {
            shot = false;
            timer = 0;
            rb.velocity = Vector2.zero; // Stop the enemy's movement which causes the wobble when shot

            if (path != null && path.Count > 1)
            {
                Vector2 temp = path.Dequeue();
                objective = map.GetCellCenterWorld(new Vector3Int((int)temp.x, (int)temp.y, 0));
                
            }
            else
            {
                baseHealth.TakeDamage(damageToBase);
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
            playerHealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        shot = true;
        canvas.SetActive(true);
        health -= damageAmount;
        enemyHealthBar.UpdateHealth(health,maxHealth);
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
