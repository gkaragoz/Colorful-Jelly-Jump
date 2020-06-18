using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Increase value for more stretching")]
    private float stretchMultiplier = 1f;

    // Handle character jump
    public void Jump(float jumpPower, Quaternion jumpRotation)
    {
        Vector3 rotation = new Vector3(jumpRotation.x, jumpRotation.w, jumpRotation.z);

        //TODO
        GetComponent<Rigidbody>().AddForce(rotation * jumpPower * 2 + transform.position);
    }

    // Handle character stretch
    public void Stretch(float stretchAxis)
    {
        //TODO
    }


}
