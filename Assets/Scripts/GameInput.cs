using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    private PlayerInputActions playerInputAcions;

    private void Awake()
    {
        Instance = this;
        playerInputAcions = new PlayerInputActions();
        playerInputAcions.Player.Enable();

        playerInputAcions.Player.Interact.performed += Interact_performed;
        playerInputAcions.Player.InteractAlternative.performed += InteractAlternate_performed;
        playerInputAcions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        playerInputAcions.Player.Interact.performed -= Interact_performed;
        playerInputAcions.Player.InteractAlternative.performed -= InteractAlternate_performed;
        playerInputAcions.Player.Pause.performed -= Pause_performed;

        playerInputAcions.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(InputAction.CallbackContext context)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputAcions.Player.Move.ReadValue<Vector2>();

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }

        return inputVector;
    }
}
