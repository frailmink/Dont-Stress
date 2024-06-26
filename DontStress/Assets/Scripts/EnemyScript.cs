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

    public float speed;
    private float originalSpeed;
    public float maxHealth;
    private float health;
    public int damageToBase = 10;
    public int damage = 1;

    private PlayerHealthScript playerHealth;
    private BaseHealthScript baseHealth; 
    private EnemyHealthBar enemyHealthBar;
    private Coroutine slowCoroutine;
    public GameObject canvas;
    public GameObject coinPrefab;
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
        originalSpeed = speed;
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
            Vector2 dif = objective - (Vector2)transform.position;
            rb.velocity = dif.normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop the enemy's movement

            if (path != null && path.Count > 0)
            {
                Vector2 temp = path.Dequeue();
                objective = map.GetCellCenterWorld(new Vector3Int((int)temp.x, (int)temp.y, 0));
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
            baseHealth.TakeDamage(damageToBase);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        canvas.SetActive(true);
        health -= damageAmount;
        enemyHealthBar.UpdateHealth(health,maxHealth);
        if (health <= 0)
        {
            Die();

        }
    }

    public void ApplySlow(float slowAmount, float duration)
    {
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        slowCoroutine = StartCoroutine(SlowEffect(slowAmount, duration));
    }

    private IEnumerator SlowEffect(float slowAmount, float duration)
    {
        speed = originalSpeed * slowAmount;
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
    }

    void Die()
    {
        // GetComponent<LootBag>().InstantiateLoot(transform.position);
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
