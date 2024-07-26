using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float rapidFireRate = 0f; // Time between each shot in rapid fire mode

    private Coroutine rapidFireCoroutine;
    private Vector2 mousePosition;
    private Rigidbody2D rb; // Reference to the player's Rigidbody2D
    private Vector2 aimDirection;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = (mousePosition - (Vector2)transform.position).normalized;
        //Calculate the aiming direction
        //Vector2 aimDirection = mousePosition - rb.position;

        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);
    }

    public Vector2 GetAimDirection()
    {
        return aimDirection;
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }

    public void StartRapidFire(InputAction.CallbackContext context)
    {
        if (rapidFireCoroutine == null)
        {
            rapidFireCoroutine = StartCoroutine(RapidFire());
        }
    }

    public void StopRapidFire(InputAction.CallbackContext context)
    {
        if (rapidFireCoroutine != null)
        {
            StopCoroutine(rapidFireCoroutine);
            rapidFireCoroutine = null;
        }
    }

    private IEnumerator RapidFire()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(rapidFireRate);
        }
    }
}

