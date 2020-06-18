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
    private ForceMode _jumpForceMode;

    // Handle character jump
    public void Jump(float jumpPower, float jumpAxis)
    {
        GetComponent<Rigidbody>().isKinematic = false;

        Vector3 rotation = new Vector3(-jumpAxis, _jumpYMultiplier, 0);

        //TODO
        GetComponent<Rigidbody>().AddForce(rotation * jumpPower, _jumpForceMode);
    }

    // Handle character stretch
    public void Stretch(float stretchAxis)
    {
        if (IsGrounded())
        {
            GetComponent<Rigidbody>().isKinematic = true;

            //TODO
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, stretchAxis * _stretchMultiplier, transform.rotation.w);
        }

    }

    // Controls whether character is grounded or
    private bool IsGrounded()
    {
        float distToGround = GetComponent<BoxCollider>().bounds.extents.y;

        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
