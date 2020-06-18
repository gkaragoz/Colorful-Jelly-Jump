using System;
using System.Collections;
using UnityEngine;

public class JumpIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject _joystickPrefab = null;

    [SerializeField]
    [Tooltip("Defines minumum jump power limit")]
    private float _minJumpPower = 0;

    [SerializeField]
    [Tooltip("Defines maximum jump power limit")]
    private float _maxJumpPower = 0;

    [SerializeField]
    [Tooltip("Defines JumpPower rate of change")]
    private float _indicatorRate = 0;

    [SerializeField]
    private float _jumpPower = 0;

    [SerializeField]
    [Tooltip("Defines JumpPower increase value per IndicatorRate")]
    private float _jumpIncreaseRate = 0;

    [SerializeField]
    [Tooltip("Defines JumpPower decrease value per IndicatorRate")]
    private float _jumpDecreaseRate = 0;

    private void Start()
    {
        // Clone the PJoystick Prefab
        Instantiate(_joystickPrefab);
    }

    // While button is holding
    private void OnDragged(float dragAxis)
    {
        // Rotate JumpIndicator
    }

    // When button is relased
    private void OnReleased()
    {
        // Hidden JumpIndicator

        // Stop JumpIndicator Action Coroutine
        StopCoroutine("IndicatorAction");
    }

    // When button is pressed
    private void OnPressed()   
    {
        // Visible JumpIndicator

        // Start JumpIndicator Action Coroutine
        StartCoroutine("IndicatorAction");
    }

    // Indicator rotation handler
    private void IndicatorRotator(float axis)
    {

    }

    private IEnumerator IndicatorAction()
    {
        bool increaseMode = true;

        _jumpPower = _minJumpPower;

        while (true)
        {
            yield return new WaitForSeconds(_indicatorRate);

            if(increaseMode)
            {
                // Increase Jump Power
                _jumpPower += _jumpIncreaseRate;

                // Controls the Mode
                if (_jumpPower > _maxJumpPower)
                {
                    increaseMode = false;

                    _jumpPower = _maxJumpPower;
                }
            }

            else
            {
                // Decrease Jump Power
                _jumpPower -= _jumpDecreaseRate;

                // Controls the Mode
                if (_jumpPower < _minJumpPower)
                {
                    increaseMode = true;

                    _jumpPower = _minJumpPower;
                }
            }
        }
    }

    private void OnEnable()
    {
        FloatingJoystick.OnJoystickPress += OnPressed;

        FloatingJoystick.OnJoystickRelease += OnReleased;

        FloatingJoystick.OnJoystickDrag += OnDragged;
    }

    private void OnDisable()
    {
        FloatingJoystick.OnJoystickPress -= OnPressed;

        FloatingJoystick.OnJoystickRelease -= OnReleased;

        FloatingJoystick.OnJoystickDrag -= OnDragged;
    }
}
