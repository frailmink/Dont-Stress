using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    Vector2 moveDirection = Vector2.zero;

    private PlayerInput PlayerControls;
    private InputAction move;

    private Rigidbody2D rb;

    private void Awake()
    {
        PlayerControls = new PlayerInput();

        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        move = PlayerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveDirection.x * MoveSpeed, moveDirection.y * MoveSpeed);
    }
}
