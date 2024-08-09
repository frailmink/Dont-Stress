using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 10f;
    public float maxDistance = 20f; // Maximum distance before bullet disappears

    private Vector3 startPosition;
    // public Animator animator;

    public void Initialize(float damage)
    {
        this.damage = damage;
    }

    private void Start()
    {
        startPosition = transform.position;
        // animator = GetComponent<Animator>();
        // animator = GetComponent<Animator>();
        // Debug.Log("PlayingFireHold");
        // animator.Play("FireHold");
    }

    // public void PlayHoldAnim(string FireHold)
    // {
    //     if (animator == null)
    //     {
    //         Debug.LogError("Animator is not assigned, cannot play animation.");
    //     }
    //     else
    //     {
    //         animator.Play(FireHold);
    //     }
    // }

    private void Update()
    {
        // Check distance traveled
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    // public void Shoot()
    // {
    //     animator.SetTrigger("Fire");
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject); // Destroy the bullet on collision
    }
}
