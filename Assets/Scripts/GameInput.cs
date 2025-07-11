using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDING_KEY = "BindingOverrides";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause,
        GamepadInteract, // Added for gamepad interaction
        GamepadInteractAlternate, // Added for gamepad interaction  
        GamepadPause // Added for gamepad interaction
    }

    private PlayerInputActions playerInputAcions;

    private void Awake()
    {
        Instance = this;
        playerInputAcions = new PlayerInputActions();

        // Load saved bindings from PlayerPrefs before enabling the actions
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING_KEY))
        {
            playerInputAcions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDING_KEY));
        }
        else
        {
            playerInputAcions.SaveBindingOverridesAsJson();
        }

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

        return inputVector.normalized;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            case Binding.MoveUp:
                return playerInputAcions.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return playerInputAcions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return playerInputAcions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return playerInputAcions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputAcions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputAcions.Player.InteractAlternative.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAcions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.GamepadInteract:
                return playerInputAcions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.GamepadInteractAlternate:
                return playerInputAcions.Player.InteractAlternative.bindings[1].ToDisplayString();
            case Binding.GamepadPause:
                return playerInputAcions.Player.Pause.bindings[1].ToDisplayString();
            default:
                return "";
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerInputAcions.Player.Disable();
        InputAction inputAction = null;
        int bindingIndex;

        switch (binding)
        {
            default:
                inputAction = null;
                bindingIndex = -1;
                break;
            case Binding.MoveUp:
                inputAction = playerInputAcions.Player.Move;
                bindingIndex = 1; // Assuming MoveUp is the second binding
                break;
            case Binding.MoveDown:
                inputAction = playerInputAcions.Player.Move;
                bindingIndex = 2; // Assuming MoveDown is the third binding
                break;
            case Binding.MoveLeft:
                inputAction = playerInputAcions.Player.Move;
                bindingIndex = 3; // Assuming MoveLeft is the fourth binding
                break;
            case Binding.MoveRight:
                inputAction = playerInputAcions.Player.Move;
                bindingIndex = 4; // Assuming MoveRight is the fifth binding
                break;
            case Binding.Interact:
                inputAction = playerInputAcions.Player.Interact;
                bindingIndex = 0; // Assuming Interact is the first binding
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputAcions.Player.InteractAlternative;
                bindingIndex = 0; // Assuming InteractAlternate is the first binding
                break;
            case Binding.Pause:
                inputAction = playerInputAcions.Player.Pause;
                bindingIndex = 0; // Assuming Pause is the first binding
                break;
            case Binding.GamepadInteract:
                inputAction = playerInputAcions.Player.Interact;
                bindingIndex = 1; // Assuming GamepadInteract is the second binding
                break;
            case Binding.GamepadInteractAlternate:
                inputAction = playerInputAcions.Player.InteractAlternative;
                bindingIndex = 1; // Assuming GamepadInteractAlternate is the second binding
                break;
            case Binding.GamepadPause:
                inputAction = playerInputAcions.Player.Pause;
                bindingIndex = 1; // Assuming GamepadPause is the second binding
                break;
        }

        if (inputAction != null && bindingIndex >= 0)
        {
            inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
            {
                callback.Dispose();
                playerInputAcions.Player.Enable();

                onActionRebound();

                // Save the new binding to PlayerPrefs
                PlayerPrefs.SetString(PLAYER_PREFS_BINDING_KEY, playerInputAcions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
        }
    }
}
