using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Movement playerMovement;
    [SerializeField] private Aiming aimingScript;
    [SerializeField] private WaterPickAttack playerAttack;
    [SerializeField] private GameObject lossScreen;

    private PlayerInput playerInputActions;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        playerInputActions = new PlayerInput();
        playerInputActions.Movement.Enable();
        playerInputActions.Movement.Jump.started += Jump;
        playerInputActions.Movement.Jump.canceled += StopJump;
        playerInputActions.Movement.AimViaMouse.performed += AimWithMouse;
        playerInputActions.Movement.AimViaDirection.performed += AimWithKeyboardOrController;


        playerInputActions.Movement.Attack.started += StartAttack;
        playerInputActions.Movement.Attack.canceled += StopAttack;

        HealthComponent health = GetComponent<HealthComponent>();
        health.OnDeath += OnDeath;

        // TODO: Add Player Controller or GameObject to a Service Locater
    }

    private void OnDisable()
    {
        playerInputActions.Movement.Disable();
        playerInputActions.Movement.Jump.started -= Jump;
        playerInputActions.Movement.Jump.canceled -= StopJump;
        playerInputActions.Movement.AimViaMouse.performed -= AimWithMouse;
        playerInputActions.Movement.AimViaDirection.performed -= AimWithKeyboardOrController;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        playerMovement.TryJump();
    }

    private void StopJump(InputAction.CallbackContext context)
    {
        playerMovement.HaltJump();
    }    

    private void AimWithMouse(InputAction.CallbackContext context)
    {
        aimingScript.AimWithMouse(context.ReadValue<Vector2>());
    }

    private void AimWithKeyboardOrController(InputAction.CallbackContext context)
    {
        aimingScript.Aim(context.ReadValue<Vector2>());
    }

    private void StartAttack(InputAction.CallbackContext context)
    {
        playerAttack.StartAttack();
    }

    private void StopAttack(InputAction.CallbackContext context)
    {
        playerAttack.StopAttack();
    }

    private void Update()
    {
        playerMovement.SetHorizontal(playerInputActions.Movement.Move.ReadValue<Vector2>().x);
    }

    private void OnDeath(object sender, System.EventArgs args) {
        AudioManager.Instance.StopAllSounds();
        AudioManager.Instance.Play("Blue Water Blues - Tutorial");
        lossScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
