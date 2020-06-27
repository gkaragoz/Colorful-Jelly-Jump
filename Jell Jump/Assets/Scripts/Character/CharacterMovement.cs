using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Increase value for more stretching")]
    private float _stretchMultiplier = .25f;

    [SerializeField]
    [Tooltip("Increase value for more jumping on Y axis")]
    private float _jumpYMultiplier= 1f;

    [SerializeField]
    [Tooltip("Represents Force Mode for Character")]
    private ForceMode _jumpForceMode = ForceMode.Impulse;

    [Header("DO NOT CHANGE")]

    [SerializeField]
    public static bool _isLanded = true;

    private bool _canJump = true;

    public bool GetCanJump()
    {
        return _canJump;
    }

    // Handle character jump
    public void Jump(float jumpPower, float jumpAxis)
    {
        if (_canJump)
        {
            // FIX STRETCING ISSUE
            GetComponent<Rigidbody>().freezeRotation = false;

            float power = GetComponent<Character>().CalculateJumpPower(jumpPower);

            Vector3 rotation = new Vector3(jumpAxis, _jumpYMultiplier, 0);

            GetComponent<Rigidbody>().AddForce(rotation * power, _jumpForceMode);

            // Start Timer
            StartCoroutine("JumpTimer");
        }
    }

    // Handle character Fever jump
    public void Jump(float jumpPower)
    {
        // FIX STRETCING ISSUE
        GetComponent<Rigidbody>().freezeRotation = false;

        Vector3 rotation = new Vector3(0, _jumpYMultiplier, 0);

        GetComponent<Rigidbody>().AddForce(rotation * jumpPower, _jumpForceMode);
    }

    // Handle character stretch
    public void Stretch(float stretchAxis)
    {
        if (_isLanded)
        {
            GetComponent<Rigidbody>().freezeRotation = true;

            //ISSUE
            transform.rotation = new Quaternion(transform.localRotation.x, 
                transform.localRotation.y, 
                -stretchAxis * _stretchMultiplier, 
                transform.localRotation.w);
        }
    }

    // Controls whether character is grounded or
    public bool IsLanded()
    {
        float distToGround = GetComponent<BoxCollider>().bounds.extents.y;

        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    // JUMP TIMER
    private IEnumerator JumpTimer()
    {
        _canJump = false;

        yield return new WaitForSeconds(.25f);

        _canJump = true;
    }

    // Disables Character Movements
    public void DisableMovement()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    // Enables Character Movements
    public void EnableMovement()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isLanded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isLanded = false;
        }
    }
}
