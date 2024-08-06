using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    private Animator animator;
    public List<GameObject> towers;

    public List<GameObject> books;
    private int currentBook = 0;

    public GameObject buildManager;
    public Tilemap map;
    public TileBase floor, taken;

    public float MoveSpeed = 5.0f;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 mousePosition;

    private PlayerInput PlayerControls;
    private InputAction move;
    private InputAction shoot;
    private InputAction build;
    private InputAction nextTower;
    private InputAction previousTower;

    private GameObject buildManagerInstance;

    private Rigidbody2D rb;
    public Transform playerTransform;
    public WeaponScript weapon;

    public int currentTowerIndex = 0;  // Track the current tower index

    private InputAction teleport;
    public float teleportDistance = 5f; // Distance to teleport
    public float teleportCooldown = 1.5f; // Cooldown time between teleports
    public int teleportManaCost = 20;
    private bool canTeleport = true; 
    public ManaBarScript manaBar;

    private InputAction rapidFire; 

    private void Awake()
    {

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
        books[0].GetComponent<BookClass>().SetSelectedTrue();
    }

    private void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        UpdateAnimatorParameters();
    }

    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveDirection.x * MoveSpeed, moveDirection.y * MoveSpeed);
        #region extraCode
        // animator.SetFloat("MoveX", moveDirection.x);
        // animator.SetFloat("MoveY", moveDirection.y);

        // Determine the direction and update the Animator parameter
        // int direction = 0;

        // if (moveDirection.y > 0)
        // {
        //     if (moveDirection.x > 0)
        //     {
        //         direction = 1; // UpRight
        //     }
        //     else if (moveDirection.x < 0)
        //     {
        //         direction = 2; // UpLeft
        //     }
        //     else
        //     {
        //         direction = 7; // Up
        //     }
        // }
        // else if (moveDirection.y < 0)
        // {
        //     if (moveDirection.x > 0)
        //     {
        //         direction = 3; // DownRight
        //     }
        //     else if (moveDirection.x < 0)
        //     {
        //         direction = 4; // DownLeft
        //     }
        //     else
        //     {
        //         direction = 8; // Down
        //     }
        // }
        // else if (moveDirection.x > 0)
        // {
        //     direction = 5; // Right
        // }
        // else if (moveDirection.x < 0)
        // {
        //     direction = 6; // Left
        // }

        // animator.SetInteger("Direction", direction);

        // Flip the player sprite based on the horizontal movement direction
        // if (moveDirection.x > 0)
        // {
        //     playerTransform.localScale = new Vector3(1, 1, 1); // Face right
        // }
        // else if (moveDirection.x < 0)
        // {
        //     playerTransform.localScale = new Vector3(-1, 1, 1); // Face left
        // }
        #endregion

    }
    //     Vector2 aimDirection = mousePosition - rb.position;
    //     float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
    //     rb.rotation = aimAngle;
    // }

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

        build = PlayerControls.Player.Build;
        build.Enable();
        build.performed += Build;

        nextTower = PlayerControls.Player.NextTower;
        nextTower.Enable();
        nextTower.performed += SwapToNextTower;
        nextTower.performed += SwapToNextBook;

        previousTower = PlayerControls.Player.PreviousTower;
        previousTower.Enable();
        previousTower.performed += SwapToPreviousTower;
        previousTower.performed += SwapToPreviousBook;

        teleport = PlayerControls.Player.Teleport;
        teleport.Enable();
        teleport.performed += Teleport;

        rapidFire = PlayerControls.Player.RapidFire; 
        rapidFire.Enable();
        rapidFire.performed += weapon.StartRapidFire; 
        rapidFire.canceled += weapon.StopRapidFire; 
    }

    private void OnDisable()
    {
        move.Disable();
        // shoot.Disable();
        build.Disable();
        nextTower.Disable();
        previousTower.Disable();
        teleport.Disable();
        rapidFire.Disable(); 
    }

    private void Build(InputAction.CallbackContext context)
    {
        if (!GlobalVariables.GetBuildingMode() && towers.Count != 0)
        {
            InstantiateBuildManager();
        }
        else if (!PlacementScript.placed && towers.Count != 0)
        {
            PlacementScript script = buildManagerInstance.GetComponent<PlacementScript>();
            script.DeleteTower();
            Destroy(buildManagerInstance);
            GlobalVariables.SetBuildingMode(false);
        }
    }

    private void SwapToNextTower(InputAction.CallbackContext context)
    {
        if (GlobalVariables.GetBuildingMode())
        {
            // Cycle to the next tower index
            currentTowerIndex = (currentTowerIndex + 1) % towers.Count;
            
            InstantiateBuildManager(); // Ensure build manager is updated
        }
    }

    private void SwapToPreviousTower(InputAction.CallbackContext context)
    {
        if (GlobalVariables.GetBuildingMode())
        {
            // Cycle to the previous tower index
            currentTowerIndex = (currentTowerIndex - 1 + towers.Count) % towers.Count;

            InstantiateBuildManager(); // Ensure build manager is updated
        }
    }

    private void SwapToNextBook(InputAction.CallbackContext context)
    {
        if (!GlobalVariables.GetBuildingMode())
        {
            books[currentBook].GetComponent<BookClass>().SetSelectedFalse();
            currentBook = (currentBook + 1) % books.Count;
            books[currentBook].GetComponent<BookClass>().SetSelectedTrue();
        }
    }

    private void SwapToPreviousBook(InputAction.CallbackContext context)
    {
        if (!GlobalVariables.GetBuildingMode())
        {
            books[currentBook].GetComponent<BookClass>().SetSelectedFalse();
            currentBook = (currentBook - 1 + books.Count) % books.Count;
            books[currentBook].GetComponent<BookClass>().SetSelectedTrue();
        }
    }

    private void InstantiateBuildManager()
    {
        if (buildManagerInstance != null)
        {
            PlacementScript script = buildManagerInstance.GetComponent<PlacementScript>();
            script.DeleteTower();
            Destroy(buildManagerInstance);
        }
        buildManagerInstance = Instantiate(buildManager, transform.position, Quaternion.Euler(0, 0, 0));
        PlacementScript newScript = buildManagerInstance.GetComponent<PlacementScript>();
        newScript.map = map;
        newScript.tower = towers[currentTowerIndex];
        newScript.ground = floor;
        newScript.taken = taken;
        newScript.playerScript = this;
        GlobalVariables.SetBuildingMode(true);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (!GlobalVariables.GetBuildingMode())
        {
            weapon.Fire();
        }
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

            // Define the layer mask to ignore the NonObstructing layer
            int layerMask = LayerMask.GetMask("NonObstructing");
            int playerLayer = LayerMask.NameToLayer("Player");

            // Check if the teleport position is valid
            Collider2D hitCollider = Physics2D.OverlapCircle(teleportPosition, 0.5f, ~layerMask);
            if (hitCollider == null && map.GetTile(new Vector3Int((int) teleportPosition.x, (int) teleportPosition.y, 0)) != null)
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
