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

    // Layer mask for normal colliders
    private int normalColliderLayerMask;

    private void Awake()
    {
        PlayerControls = new PlayerInput();
        mainCamera = Camera.main;

        // Create a layer mask for only the normal collider layer
        normalColliderLayerMask = LayerMask.GetMask("Tower");
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
        // Perform raycast only on the normal collider layer
        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, normalColliderLayerMask);
        if (!rayHit) return;

        GameObject rayObject = rayHit.collider.gameObject;
        TowerScript script = rayObject.GetComponentInChildren<TowerScript>();
        if (script == null) return;

        script.OpenUpgradeUI(upgradeUI);
    }
}