using UnityEngine;
using System;

[RequireComponent(typeof(JumpIndicator))]
public class CharacterController : MonoBehaviour
{
    // Fires when character jump action detects
    public Action<float, Quaternion> OnJumpDetect;

    // Fires when character jump action detects
    public Action<float> OnStretchDetect;

    // Represents Character' Jump Action
    private void DetectJUMP(float jumpPower, Quaternion jumpRotation)
    {
        OnJumpDetect?.Invoke(jumpPower, jumpRotation);
    }

    // Represents Character' Stretch Action
    private void DetectSTRETCH(float stretchAxis)
    {
        OnStretchDetect?.Invoke(stretchAxis);
    }

    private void OnEnable()
    {
        GetComponent<JumpIndicator>().OnIndicatorReleased += DetectJUMP;

        GetComponent<JumpIndicator>().OnIndicatorDrag += DetectSTRETCH;
    }

    private void OnDisable()
    {
        GetComponent<JumpIndicator>().OnIndicatorReleased -= DetectJUMP;

        GetComponent<JumpIndicator>().OnIndicatorDrag -= DetectSTRETCH;
    }
}
