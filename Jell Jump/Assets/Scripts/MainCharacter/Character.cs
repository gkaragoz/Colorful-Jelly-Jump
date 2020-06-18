using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CharacterMovement))]
public class Character : MonoBehaviour
{


    private void OnEnable()
    {
        GetComponent<JumpIndicator>().OnIndicatorReleased +=
            GetComponent<CharacterMovement>().Jump;
    }

    private void OnDisable()
    {
        GetComponent<JumpIndicator>().OnIndicatorReleased -=
            GetComponent<CharacterMovement>().Jump;
    }
}
