using System;
using UnityEngine;

public static class InputManager
{
    private static InputActions inputs;

    public static event Action onInteraction;
    public static event Action OnPauseRequested, OnResumeRequested;
    public static Action OnPauseAllowed, OnResumeAllowed;

    static InputManager()
    {
        inputs = new InputActions();
        inputs.Enable();
        SwitchTo_PlayerInput();
        
        inputs.Player.Interact.performed += _ => onInteraction?.Invoke();
        inputs.Player.Pause.performed += _ => OnPauseRequested?.Invoke();
        inputs.UI.Resume.performed += _ => OnResumeRequested?.Invoke();
        OnPauseAllowed += SwitchTo_UI;
        OnResumeAllowed += SwitchTo_PlayerInput;
    }
    
    public static Vector2 GetMovementVector => inputs.Player.Movement.ReadValue<Vector2>();

    public static bool IsMoving(out Vector3 direction)
    {
        direction = new Vector3(GetMovementVector.x, 0f, GetMovementVector.y);
        return direction != Vector3.zero;
    }
    
    static void SwitchTo_UI()
    {
        inputs.Player.Disable();
        inputs.UI.Enable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    static void SwitchTo_PlayerInput()
    {
        inputs.UI.Disable();
        inputs.Player.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
