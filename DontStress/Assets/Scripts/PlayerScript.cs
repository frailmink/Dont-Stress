using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    public GameObject[] towers;

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
    public WeaponScript weapon;

    private int currentTowerIndex = 0;  // Track the current tower index

    private void Awake()
    {
        PlayerControls = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
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
        nextTower.performed += SwapToNextTower;  // Add listener for next tower swapping

        previousTower = PlayerControls.Player.PreviousTower;
        previousTower.Enable();
        previousTower.performed += SwapToPreviousTower;  // Add listener for previous tower swapping
    }

    private void OnDisable()
    {
        move.Disable();
        shoot.Disable();
        build.Disable();
        nextTower.Disable();
        previousTower.Disable();
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
        // Cycle to the next tower index
        currentTowerIndex = (currentTowerIndex + 1) % towers.Length;
        Debug.Log("Current Tower Index: " + currentTowerIndex);
        
        if (GlobalVariables.GetBuildingMode())
        {
            InstantiateBuildManager();
        }
    }

    private void SwapToPreviousTower(InputAction.CallbackContext context)
    {
        // Cycle to the previous tower index
        currentTowerIndex = (currentTowerIndex - 1 + towers.Length) % towers.Length;
        Debug.Log("Current Tower Index: " + currentTowerIndex);

        if (GlobalVariables.GetBuildingMode())
        {
            InstantiateBuildManager();
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

    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveDirection.x * MoveSpeed, moveDirection.y * MoveSpeed);

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
}
