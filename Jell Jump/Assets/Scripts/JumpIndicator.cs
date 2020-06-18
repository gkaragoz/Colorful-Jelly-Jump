using System;
using UnityEngine;

public class JumpIndicator : MonoBehaviour
{
    // While button is holding
    private void OnDrag(float dragAxis)
    {
        // Rotate JumpIndicator
    }

    // When button is relased
    private void OnReleased()
    {
        // Hidden JumpIndicator

        // Stop JumpIndicator Action
    }

    // When button is pressed
    private void OnPressed()   
    {
        // Visible JumpIndicator

        // Start JumpIndicator Action
    }

    private void OnEnable()
    {
        FloatingJoystick.OnJoystickPress += OnPressed;

        FloatingJoystick.OnJoystickRelease += OnReleased;

        FloatingJoystick.OnJoystickDrag += OnDrag;
    }

    private void OnDisable()
    {
        FloatingJoystick.OnJoystickPress -= OnPressed;

        FloatingJoystick.OnJoystickRelease -= OnReleased;

        FloatingJoystick.OnJoystickDrag -= OnDrag;
    }
}
