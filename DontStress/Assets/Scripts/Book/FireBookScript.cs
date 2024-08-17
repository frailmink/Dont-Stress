using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireBookScript : BookClass
{
    public GameObject bulletPrefab;
    public int fireForce;

    public Vector3 offset = new Vector3(0, 0.5f, -1);

    private Vector2 aimDirection;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void UsePower()
    {
        animator.SetBool("IsHoldingFire", true);

        // Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // aimDirection = (mousePosition - (Vector2)transform.position).normalized;
        // //Calculate the aiming direction
        // //Vector2 aimDirection = mousePosition - rb.position;
        // 
        // float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90;
        // 
        // GameObject bullet = Instantiate(bulletPrefab, transform.position + offset, Quaternion.Euler(0f, 0f, aimAngle));
        // bullet.GetComponent<Rigidbody2D>().AddForce(aimDirection * fireForce, ForceMode2D.Impulse);
    }

    public override void ReleasePower()
    {
        animator.SetBool("IsHoldingFire", false);
        // animator.SetFloat("Blend", 1.0f);
        Fire();
    }

    private void Fire()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = (mousePosition - (Vector2)transform.position).normalized;

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90;

        GameObject bullet = Instantiate(bulletPrefab, transform.position + offset, Quaternion.Euler(0f, 0f, aimAngle));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(aimDirection * fireForce, ForceMode2D.Impulse);

        animator.SetTrigger("Fire"); // Trigger fire animation

        // BulletScript bulletScript = projectile.GetComponent<BulletScript>();
        // if (bulletScript != null)
        // {
        //     bulletScript.Shoot();
        // }
    }
}
