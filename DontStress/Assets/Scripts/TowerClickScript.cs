using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerClickScript : MonoBehaviour
{
    public GameObject upgradeUI;

    private Camera mainCamera;

    private PlayerInput PlayerControls;
    private InputAction click;

    private void Awake()
    {
        PlayerControls = new PlayerInput();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        click = PlayerControls.Player.Attack;
        click.Enable();
        click.performed += Clicked;
    }

    private void OnDisable()
    {
        click.Disable();
    }

    private void Clicked(InputAction.CallbackContext context)
    {
        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
        if (!rayHit) return;

        GameObject rayObject = rayHit.collider.gameObject;
        TowerScript script = rayObject.GetComponent<TowerScript>();
        if (script == null) return;

        script.OpenUpgradeUI(upgradeUI);
    }
}
