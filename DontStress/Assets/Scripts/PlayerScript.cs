using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 mousePosition;

    private PlayerInput PlayerControls;
    private InputAction move;
    private InputAction shoot;

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
    }

    private void OnDisable()
    {
        move.Disable();
        shoot.Disable();
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
