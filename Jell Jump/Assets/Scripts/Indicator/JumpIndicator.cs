using System;
using System.Collections;
using UnityEngine;

public class JumpIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject _indicatorPrefab = null;

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
    [Tooltip("Don't play with this value.")]
    private float _jumpPower = 0;

    [SerializeField]
    [Tooltip("Defines JumpPower increase value per IndicatorRate")]
    private float _jumpIncreaseRate = 0;

    [SerializeField]
    [Tooltip("Defines JumpPower decrease value per IndicatorRate")]
    private float _jumpDecreaseRate = 0;

    [SerializeField]
    [Tooltip("Defines JumpIndicator rotation scale")]
    private float _rotationScale = 0.5f;

    // Defines Indicator Rotation
    private Quaternion _indicatorRotation;

    // Defines Indicator Axis value
    private float _indicatorAxis;

    private bool _isPressed;

    // Fires when Jump Indicator released
    public Action<float, float> OnIndicatorReleased;

    // Fires while Jump Indicator dragging
    public Action<float> OnIndicatorDrag;

    // WORST - CASE
    private void Update()
    {
        // Locate JumpIndicator
        IndicatorLocator();

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    // Invokes OnIndicatorReleased with _jumpPower
        //    OnIndicatorReleased?.Invoke(_maxJumpPower, _indicatorAxis);
        //}
    }

    // While button is holding
    private void OnDragged(float dragAxis)
    {
        // Rotate JumpIndicator
        IndicatorRotator(dragAxis);

        // Invokes OnIndicatorDrag
        OnIndicatorDrag?.Invoke(dragAxis);

        // Assings indicatorAxis
        _indicatorAxis = dragAxis * _rotationScale;
    }

    // When button is relased
    private void OnReleased()
    {
        if (_isPressed)
        {
            // Hide JumpIndicator
            HideIndicator();

            // Stop JumpIndicator Action Coroutine
            StopCoroutine("IndicatorAction");

            // Invokes OnIndicatorReleased with _jumpPower
            OnIndicatorReleased?.Invoke(_jumpPower, _indicatorAxis);

            // CameraFocus Animation Reverse
            FindObjectOfType<CameraManager>().CameraAction(CameraAnimationState.ANIM_REVERSEFOCUS, null);

            _isPressed = false;
        }
    }

    // When button is pressed
    private void OnPressed()   
    {
        if (GameManager.instance.MyCharacter.GetComponent<CharacterMovement>().GetCanJump())
        {
            // Visible JumpIndicator
            ShowIndicator();

            // Start JumpIndicator Action Coroutine
            StartCoroutine("IndicatorAction");

            // CameraFocus Animation Forward
            FindObjectOfType<CameraManager>().CameraAction(CameraAnimationState.ANIM_FOCUS, null);

            _isPressed = true;
        }
    }

    // Indicator rotation handler
    private void IndicatorRotator(float axis)
    {
        _indicatorRotation = new Quaternion(_indicatorPrefab.transform.rotation.x,
         _indicatorPrefab.transform.rotation.y,
        -axis * _rotationScale,
        _indicatorPrefab.transform.rotation.w);

        _indicatorPrefab.transform.rotation = _indicatorRotation;
    }

    // Indicator location handler
    public void IndicatorLocator()
    {
        _indicatorPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + .1f, 0);
    }

    // Indicator color change handler
    private void ColorChanger(GameObject prefab)
    {
        Color color = Color.Lerp(Color.red, Color.green, _jumpPower / _maxJumpPower);

        prefab.GetComponent<SpriteRenderer>().color = color;
    }

    private IEnumerator IndicatorAction()
    {
        bool increaseMode = true;

        // Set jump power to min jump Limit
        _jumpPower = _minJumpPower;

        // Reset JumpIndicator Color to initial color
        ColorChanger(_indicatorPrefab);

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

            // Update JumpIndicator Color
            ColorChanger(_indicatorPrefab);
        }
    }

    // Makes visible to indicator
    public void ShowIndicator()
    {
        // Visible JumpIndicator
        _indicatorPrefab.SetActive(true);
    }

    // Makes hide to indicator
    public void HideIndicator()
    {
        // Hidden JumpIndicator
        _indicatorPrefab.SetActive(false);
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
