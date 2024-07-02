using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    public float health;
    public float maxHealth = 4;

    public SpriteRenderer playerSr;
    public GameOverManager gameOverManager;
    public PlayerScript playerMovement;

    public Collider2D playerCollider;
    public Rigidbody2D rb;
    // public WeaponScript weaponScript;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        gameOverManager.TriggerGameOverFalse();
    }

    public void TakeDamage(int damage)
    {
        health -= 1;
        if(health <= 0)
        {
            // Destroy(gameObject);
            playerSr.enabled = false;
            playerMovement.enabled = false;
            playerCollider.enabled = false;
            rb.velocity = Vector2.zero;
            gameOverManager.TriggerGameOver();
            // weaponScript.enabled = false;
        }
    }
}
