using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BookClass : MonoBehaviour
{
    protected static int numBooks = 0;

    protected PlayerInput PlayerControls;
    protected InputAction shoot;

    private float angleDif = 0;
    private static float angleDifStatic = 0;

    private int bookIndex;

    private Vector2 objective;
    private Rigidbody2D rb;

    private Vector2 usualPos;

    private bool selected = false;
    private bool canShoot = true;

    private Vector3 mousePosition;

    private Transform player;
    private GameObject playerObject;

    public bool active = false;

    public delegate void MovementDelegate();
    public MovementDelegate CurrentMovementFunc;
    public MovementDelegate CurrentFollowFunc;

    public static float raidus = 2f;
    public static float minimum = 1.5f;
    public static float usualRadius = 1;

    public static float moveSpeed = 5;
    public static float rotationSpeed = 100f;

    public float cooldown = 0.1f;
    protected virtual void OnEnable()
    {
        shoot = PlayerControls.Player.Attack;
        shoot.Enable();
        shoot.performed += Fire;
    }

    protected virtual void OnDisable()
    {
        shoot.Disable();
    }

    protected virtual void Awake()
    {
        PlayerControls = new PlayerInput();

        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerObject.GetComponent<PlayerScript>().books.Add(gameObject);
    }

    protected virtual void Start()
    {
        numBooks++;
        bookIndex = numBooks;
        angleDif = angleDifStatic;
        CurrentFollowFunc = FollowCircle;
        CurrentMovementFunc = MoveNoDelay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalcUsualPos();
        CurrentFollowFunc?.Invoke();
        CurrentMovementFunc?.Invoke();
    }

    private void MoveWithDelay()
    {
        Vector2 speed = objective - (Vector2) transform.position;
        Vector2 line = player.position - transform.position;

        // Calculate the perpendicular vector (rotate by 90 degrees)
        Vector3 perpendicularDirection = line.normalized;

        // Set the rotation angle based on the perpendicular vector
        // float angle = Mathf.Atan2(perpendicularDirection.y, perpendicularDirection.x) * Mathf.Rad2Deg;

        // transform.rotation = Quaternion.Euler(0, 0, angle + 180);
        rb.velocity = new Vector2(speed.x * moveSpeed, speed.y * moveSpeed);
    }

    private void MoveNoDelay()
    {
        Vector2 line = player.position - transform.position;

        // Calculate the perpendicular vector (rotate by 90 degrees)
        Vector3 perpendicularDirection = line.normalized;

        // Set the rotation angle based on the perpendicular vector
        // float angle = Mathf.Atan2(perpendicularDirection.y, perpendicularDirection.x) * Mathf.Rad2Deg;
        
        // transform.rotation = Quaternion.Euler(0, 0, angle + 180);

        transform.position = objective;
    }

    private void CalcUsualPos()
    {
        angleDif += Time.deltaTime * rotationSpeed;
        angleDif = angleDif % 360;
        angleDifStatic = angleDif;
        float angle = angleDif + bookIndex * (360f / numBooks);
        float radians = angle * Mathf.Deg2Rad;
        float x = usualRadius * Mathf.Cos(radians);
        float y = usualRadius * Mathf.Sin(radians);
        usualPos = new Vector2(x, y);
    }

    private void FollowMouse()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 temp = new Vector3(player.position.x, player.position.y, 0);
        Vector3 direction = mousePosition - temp;
        objective = ClampMagnitudeInRange(direction, minimum, raidus) + player.position;
    }

    private void FollowCircle()
    {
        // Vector2 direction = ((Vector2) transform.position) - usualPos;
        // objective = ClampMagnitudeInRange(direction, usualRadius, usualRadius) + player.position;
        objective = usualPos + ((Vector2) player.position);
    }

    Vector3 ClampMagnitudeInRange(Vector3 vector, float min, float max)
    {
        float currentMagnitude = vector.magnitude;
        float clampedMagnitude = Mathf.Clamp(currentMagnitude, min, max);

        // Preserve the direction and multiply by the clamped magnitude
        return vector.normalized * clampedMagnitude;
    }

    protected void Fire(InputAction.CallbackContext context)
    {
        if (canShoot && selected && !active)
        {
            UsePower();
            StartCoroutine(PowerCooldown());
        }
    }

    public void SetSelectedTrue()
    {
        selected = true;
        CurrentFollowFunc = FollowMouse;
        CurrentMovementFunc = MoveWithDelay;
    }

    public void SetSelectedFalse()
    {
        selected = false;
        CurrentFollowFunc = FollowCircle;
        CurrentMovementFunc = MoveNoDelay;
    }

    private IEnumerator PowerCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    public abstract void UsePower();
}