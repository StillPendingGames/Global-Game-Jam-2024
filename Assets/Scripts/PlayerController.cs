using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Movement playerMovement;
    [SerializeField] private Aiming aimingScript;
    private PlayerInput playerInputActions;

    private void OnEnable()
    {
        playerInputActions = new PlayerInput();
        playerInputActions.Movement.Enable();
        playerInputActions.Movement.Jump.started += Jump;
        playerInputActions.Movement.Jump.canceled += StopJump;
        playerInputActions.Movement.Aim.performed += Aim;

        HealthComponent health = GetComponent<HealthComponent>();
        health.OnDeath += OnDeath;

        // TODO: Add Player Controller or GameObject to a Service Locater
    }

    private void OnDisable()
    {
        playerInputActions.Movement.Disable();
        playerInputActions.Movement.Jump.started -= Jump;
        playerInputActions.Movement.Jump.canceled -= StopJump;
        playerInputActions.Movement.Jump.performed -= Aim;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        playerMovement.TryJump();
    }

    private void StopJump(InputAction.CallbackContext context)
    {
        playerMovement.HaltJump();
    }    

    private void Aim(InputAction.CallbackContext context)
    {
        aimingScript.Aim(context.ReadValue<Vector2>());
    }

    private void Update()
    {
        playerMovement.SetHorizontal(playerInputActions.Movement.Move.ReadValue<Vector2>().x);
    }

    private void OnDeath(object sender, System.EventArgs args) {
        // TODO: Bring up Death screen UI and play death animation

        // For now just reload the player back into the main scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
