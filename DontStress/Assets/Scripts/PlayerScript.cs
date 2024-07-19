using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    public List<GameObject> towers;

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

    private int currentTowerIndex = 0;  // Track the current tower index

    private InputAction teleport;
    public float teleportDistance = 5f; // Distance to teleport
    public float teleportCooldown = 3f; // Cooldown time between teleports
    public int teleportManaCost = 20;
    private bool canTeleport = true; 
    public ManaBarScript manaBar;

    private InputAction rapidFire; 

    private void Awake()
    {
        PlayerControls = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        
        if (manaBar == null)
        {
            Debug.LogError("ManaBarScript is null. Teleportation failed. Please ensure ManaBarScript is assigned.");
            return;
        }
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnEnable()
    {
        shoot = PlayerControls.Player.Attack;
        shoot.Enable();
        shoot.performed += Fire;

        move = PlayerControls.Player.Move;
        move.Enable();

        build = PlayerControls.Player.Build;
        build.Enable();
        build.performed += Build;

        nextTower = PlayerControls.Player.NextTower;
        nextTower.Enable();
        nextTower.performed += SwapToNextTower;

        previousTower = PlayerControls.Player.PreviousTower;
        previousTower.Enable();
        previousTower.performed += SwapToPreviousTower; 

        teleport = PlayerControls.Player.Teleport;
        teleport.Enable();
        teleport.performed += Teleport;
        Debug.Log("Teleport action enabled and bound");

        rapidFire = PlayerControls.Player.RapidFire; 
        rapidFire.Enable();
        rapidFire.performed += weapon.StartRapidFire; 
        rapidFire.canceled += weapon.StopRapidFire; 
    }

    private void OnDisable()
    {
        move.Disable();
        shoot.Disable();
        build.Disable();
        nextTower.Disable();
        previousTower.Disable();
        teleport.Disable();
        rapidFire.Disable(); 
    }

    private void Build(InputAction.CallbackContext context)
    {
        if (!GlobalVariables.GetBuildingMode())
        {
            InstantiateBuildManager();
        }
        else if (!PlacementScript.placed)
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
            Debug.Log("Current Tower Index: " + currentTowerIndex);
            
            InstantiateBuildManager(); // Ensure build manager is updated
        }
    }

    private void SwapToPreviousTower(InputAction.CallbackContext context)
    {
        if (GlobalVariables.GetBuildingMode())
        {
            // Cycle to the previous tower index
            currentTowerIndex = (currentTowerIndex - 1 + towers.Count) % towers.Count;
            Debug.Log("Current Tower Index: " + currentTowerIndex);

            InstantiateBuildManager(); // Ensure build manager is updated
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
        GlobalVariables.SetBuildingMode(true);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (!GlobalVariables.GetBuildingMode())
        {
            weapon.Fire();
        }
    }

    private void Teleport(InputAction.CallbackContext context)
    {
        // if (manaBar == null)
        // {
        //     Debug.LogError("ManaBarScript is null. Teleportation failed. Please ensure ManaBarScript is assigned.");
        //     return;
        // }

        if (!GlobalVariables.GetBuildingMode() && manaBar.HasEnoughMana(teleportManaCost))
        {
            Vector2 teleportDirection = moveDirection.normalized;
            if (teleportDirection == Vector2.zero)
            {
                teleportDirection = transform.up; // Teleport forward if not moving
            }

            Vector2 teleportPosition = rb.position + teleportDirection * teleportDistance;

            // Debugging the teleport position
            Debug.Log($"Attempting to teleport to position: {teleportPosition}");

            // Define the layer mask to ignore the NonObstructing layer
            int layerMask = LayerMask.GetMask("NonObstructing");

            // Check if the teleport position is valid
            Collider2D hitCollider = Physics2D.OverlapCircle(teleportPosition, 0.5f, ~layerMask);
            if (hitCollider == null)
            {
                rb.position = teleportPosition;
                manaBar.SpendMana(teleportManaCost);
            }
            else
            {
                Debug.Log($"Teleportation failed: destination obstructed by {hitCollider.name}.");
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

    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveDirection.x * MoveSpeed, moveDirection.y * MoveSpeed);

        if (moveDirection.x > 0)
        {
            playerTransform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveDirection.x < 0)
        {
            playerTransform.localScale = new Vector3(1, 1, 1);
        }
    }
    //     Vector2 aimDirection = mousePosition - rb.position;
    //     float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
    //     rb.rotation = aimAngle;
    // }
}
