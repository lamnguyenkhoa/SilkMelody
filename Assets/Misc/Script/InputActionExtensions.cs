using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputActionExtensions
{
    public static bool IsPressed(this InputAction inputAction)
    {
        return inputAction.ReadValue<float>() > 0f;
    }

    public static bool WasPressedThisFrame(this InputAction inputAction)
    {
        return inputAction.triggered && inputAction.ReadValue<float>() > 0f;
    }

    public static bool WasReleasedThisFrame(this InputAction inputAction)
    {
        return inputAction.triggered && inputAction.ReadValue<float>() == 0f;
    }
}