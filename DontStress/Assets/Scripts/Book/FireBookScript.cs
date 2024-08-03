using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBookScript : BookClass
{
    public GameObject bulletPrefab;
    public int fireForce;

    private Vector2 aimDirection;

    public override void UsePower()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = (mousePosition - (Vector2)transform.position).normalized;
        //Calculate the aiming direction
        //Vector2 aimDirection = mousePosition - rb.position;

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, aimAngle));
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * fireForce, ForceMode2D.Impulse);
    }
}
