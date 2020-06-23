using UnityEngine;
using System;

[RequireComponent(typeof(JumpIndicator))]
public class CharacterController : MonoBehaviour
{
    // Fires when character jump action detects
    public Action<float, float> OnJumpDetect;

    // Fires when character jump action detects
    public Action<float> OnStretchDetect;

    // Represents Character' Jump Action
    private void DetectJUMP(float jumpPower, float jumpRotation)
    {
        if (GameManager.instance._gameState == GameState.ONPLAY)
        {
            OnJumpDetect?.Invoke(jumpPower, jumpRotation);
        }

        else
        {
            GetComponent<JumpIndicator>().enabled = false;
        }
    }

    // Represents Character' Stretch Action
    private void DetectSTRETCH(float stretchAxis)
    {
        if (GameManager.instance._gameState == GameState.ONPLAY)
        {
            OnStretchDetect?.Invoke(stretchAxis);
        }

        else
        {
            GetComponent<JumpIndicator>().enabled = false;
        }
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
