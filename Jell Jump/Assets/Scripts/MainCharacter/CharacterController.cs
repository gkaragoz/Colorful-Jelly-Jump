using UnityEngine;
using System;

[RequireComponent(typeof(JumpIndicator))]
public class CharacterController : MonoBehaviour
{
    // Fires when character jump action detects
    public Action OnJumpDetect;

    // Represents Character' Jump Action
    private void Jump(float jumpPower, Quaternion jumpRotation)
    {
        OnJumpDetect?.Invoke();
    }

    private void OnEnable()
    {
        GetComponent<JumpIndicator>().OnIndicatorReleased += Jump;
    }

    private void OnDisable()
    {
        GetComponent<JumpIndicator>().OnIndicatorReleased -= Jump;
    }
}
