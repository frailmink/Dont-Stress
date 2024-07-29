using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerButtonPressScript : MonoBehaviour
{
    public delegate void PowerDelegate();
    public PowerDelegate CurrentPower;

    private PlayerInput PlayerControls;
    private InputAction usePower;

    private void Awake()
    {
        PlayerControls = new PlayerInput();
        CurrentPower = NoPower;
    }

    private void OnEnable()
    {
        usePower = PlayerControls.Player.Power;
        usePower.Enable();
        usePower.performed += RunPower;
    }

    private void OnDisable()
    {
        usePower.Disable();
    }

    private void RunPower(InputAction.CallbackContext context)
    {
        CurrentPower?.Invoke();
    }

    private void NoPower() {}

    public void SwitchPower(PowerDelegate function)
    {
        CurrentPower = function;
    }
}
