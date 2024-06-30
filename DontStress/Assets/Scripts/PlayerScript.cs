using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    public List<GameObject> towers = new List<GameObject>();

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

    private GameObject buildManagerInstance;

    private Rigidbody2D rb;
    public WeaponScript weapon;

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
    }

    private void OnDisable()
    {
        move.Disable();
        shoot.Disable();
        build.Disable();
    }

    private void Build(InputAction.CallbackContext context)
    {
        if (!GlobalVariables.GetBuildingMode() && !GlobalVariables.Paused && towers.Count > 0)
        {
            buildManagerInstance = Instantiate(buildManager, transform.position, Quaternion.Euler(0, 0, 0));
            PlacementScript script = buildManagerInstance.GetComponent<PlacementScript>();
            script.map = map;
            script.tower = towers[0];
            script.ground = floor;
            script.taken = taken;
            script.playerScript = this;
            GlobalVariables.SetBuildingMode(true);
        }
        else if (!PlacementScript.placed && towers.Count > 0)
        {
            PlacementScript script = buildManagerInstance.GetComponent<PlacementScript>();
            script.DeleteTower();
            Destroy(buildManagerInstance);
            GlobalVariables.SetBuildingMode(false);
        }
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (!GlobalVariables.GetBuildingMode() && !GlobalVariables.Paused)
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

    public void IncreaseMoveSpeed(int differnce)
    {
        MoveSpeed += differnce;
    }
}
