using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Movement playerMovement;
    private PlayerInput playerInputActions;

    private void OnEnable()
    {
        playerInputActions = new PlayerInput();
        playerInputActions.Movement.Enable();
        playerInputActions.Movement.Jump.started += Jump;
        playerInputActions.Movement.Jump.canceled += StopJump;
        playerInputActions.Movement.Aim.performed += Aim;
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

    }

    private void Update()
    {
        playerMovement.SetHorizontal(playerInputActions.Movement.Move.ReadValue<Vector2>().x);
    }
}
