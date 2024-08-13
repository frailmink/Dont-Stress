using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerScriptInVillage : MonoBehaviour
{
    public GameObject InteractionText;
    public float interactionRadius = 3.5f;
    private LayerMask interactionLayer;

    private Animator animator;

    public float MoveSpeed = 5.0f;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 mousePosition;

    private PlayerInput PlayerControls;
    private InputAction move;
    private InputAction interact;

    private Rigidbody2D rb;
    public Transform playerTransform;

    private InputAction teleport;
    public float teleportDistance = 5f; // Distance to teleport
    public float teleportCooldown = 1.5f; // Cooldown time between teleports
    public int teleportManaCost = 20;
    private bool canTeleport = true; 
    public ManaBarScript manaBar;
    
    private void Awake()
    {
        interactionLayer = LayerMask.GetMask("Tower");
        PlayerControls = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // Get the Animator component
        
        if (manaBar == null)
        {
            Debug.LogError("ManaBarScript is null. Teleportation failed. Please ensure ManaBarScript is assigned.");
            return;
        }
    }

    private void Start()
    {
        InteractionText.SetActive(false);
    }

    private void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        CheckIfInteractionPossible();
        UpdateAnimatorParameters();
    }

    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveDirection.x * MoveSpeed, moveDirection.y * MoveSpeed);
    }

    private void UpdateAnimatorParameters()
    {
        float horizontal = moveDirection.x;
        float vertical = moveDirection.y;

        // Normalize the move direction to handle movement speed consistency
        if (moveDirection.magnitude > 0)
        {
            moveDirection.Normalize();
        }

        // Set parameters for the Blend Tree
        animator.SetFloat("DirectionX", moveDirection.x);
        animator.SetFloat("DirectionY", moveDirection.y);
        // Debugging: Output direction values
    }

    private void OnEnable()
    {
        // shoot = PlayerControls.Player.Attack;
        // shoot.Enable();
        // shoot.performed += Fire;

        move = PlayerControls.Player.Move;
        move.Enable();

        interact = PlayerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;

        teleport = PlayerControls.Player.Teleport;
        teleport.Enable();
        teleport.performed += Teleport;

        // rapidFire = PlayerControls.Player.RapidFire; 
        // rapidFire.Enable();
        // rapidFire.performed += weapon.StartRapidFire; 
        // rapidFire.canceled += weapon.StopRapidFire; 
    }

    private void OnDisable()
    {
        move.Disable();
        teleport.Disable();
    }

    void CheckIfInteractionPossible()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionLayer);

        if (hit)
        {
            InteractionText.SetActive(true);
        } else
        {
            InteractionText.SetActive(false);
        }
    }

    void Interact(InputAction.CallbackContext context)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactionLayer);
        Collider2D closestHit = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHit = hit;
            }
        }
        GameObject g = new GameObject();
        closestHit?.GetComponentInChildren<IInteractable>()?.Interact(g);
    }

    public void Teleport(InputAction.CallbackContext context)
    {
        if (!canTeleport)
        {
            return;
        }

        if (!GlobalVariables.GetBuildingMode() && manaBar.HasEnoughMana(teleportManaCost))
        {
            // Vector2 aimDirection = (mousePosition - rb.position).normalized;
            // Vector2 teleportDirection = weapon.GetAimDirection();// Using aim (mouse)
            Vector2 teleportDirection = moveDirection.normalized;// Using move (keyboard)

            if (teleportDirection == Vector2.zero )
            {
                // aimDirection = transform.up;
                teleportDirection = transform.up;
            }

            // Vector2 teleportPosition = rb.position + aimDirection * teleportDistance;
            Vector2 teleportPosition = rb.position + teleportDirection * teleportDistance;

            int layerMask = LayerMask.GetMask("NonObstructing");
            int playerLayer = LayerMask.NameToLayer("Player");

            Collider2D hitCollider = Physics2D.OverlapCircle(teleportPosition, 0.5f, ~layerMask);
            if (hitCollider == null)
            {
                rb.position = teleportPosition;
                manaBar.SpendMana(teleportManaCost);
                StartCoroutine(TeleportCooldown());
            }
            else
            {
                // Debug.Log($"Teleportation failed: destination obstructed by {hitCollider.name}.");
                Debug.Log($"Teleportation failed: destination obstructed");
            }
        }
        else
        {
            Debug.Log("Teleportation failed: not enough mana or in building mode.");
        }
    }


    private IEnumerator TeleportCooldown()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }
}
