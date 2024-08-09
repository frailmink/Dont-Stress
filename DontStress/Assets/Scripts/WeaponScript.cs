using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float rapidFireRate = 0.1f; // Time between each shot in rapid fire mode

    private Coroutine rapidFireCoroutine;
    private Vector2 mousePosition;
    private Rigidbody2D rb; // Reference to the player's Rigidbody2D
    private Vector2 aimDirection;
    private Animator animator;
    private PlayerInput inputActions;
    private InputAction fire;


    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        inputActions = new PlayerInput(); 
    }

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void OnEnable()
    {
        fire = inputActions.Player.Attack;
        fire.Enable();
        fire.started += StartHoldingFire;
        fire.canceled += ReleaseFire;
    }

    void OnDisable()
    {
        fire.Disable();
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

    public void StartHoldingFire(InputAction.CallbackContext context)
    {
        animator.SetBool("IsHoldingFire", true);
        // animator.SetFloat("Blend", 0f);
    }

    public void Fire()
    {
        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        }

        animator.SetTrigger("Fire"); // Trigger fire animation

        // BulletScript bulletScript = projectile.GetComponent<BulletScript>();
        // if (bulletScript != null)
        // {
        //     bulletScript.Shoot();
        // }
    }

    public void ReleaseFire(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            animator.SetBool("IsHoldingFire", false);
            // animator.SetFloat("Blend", 1.0f);
            Fire();
        }
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

