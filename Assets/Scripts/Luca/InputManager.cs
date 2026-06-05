using System;
using UnityEngine;

public static class InputManager
{
    private static InputActions inputs;

    public static event Action onInteraction;

    static InputManager()
    {
        inputs = new InputActions();
        inputs.Enable();
        
        inputs.Player.Interact.performed += _ => onInteraction?.Invoke();
    }
    
    public static Vector2 GetMovementVector => inputs.Player.Movement.ReadValue<Vector2>();

    public static bool IsMoving(out Vector3 direction)
    {
        direction = new Vector3(GetMovementVector.x, 0f, GetMovementVector.y);
        return direction != Vector3.zero;
    }
}
