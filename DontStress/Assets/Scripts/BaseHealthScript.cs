using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealthScript : MonoBehaviour
{
    [SerializeField] private Image barImage; // Use SerializeField to ensure it can be set in the inspector
    public float maxHealth = 100f;
    private float health;
    public GameOverManager gameOverManager;

    private void Awake()
    {
        if (barImage == null)
        {
            barImage = transform.Find("bar").GetComponent<Image>();
        }

        health = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Base Taking damage !!!");
        if (health <= 0)
        {
            health = 0;
            // Optional: Add logic for when the base is destroyed
            gameOverManager.TriggerGameOver();
            Debug.Log("Base Destroyed");
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (barImage != null)
        {
            barImage.fillAmount = health / maxHealth;
        }
        // else
        // {
        //     Debug.Log("No image");
        // }
    }
}
