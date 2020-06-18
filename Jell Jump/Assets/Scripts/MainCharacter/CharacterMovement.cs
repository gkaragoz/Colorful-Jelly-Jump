using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Increase value for more stretching")]
    private float stretchMultiplier = 1f;

    // Handle character jump
    public void Jump(float jumpPower, Quaternion jumpRotation)
    {
        //TODO
    }

    // Handle character stretch
    public void Stretch(float stretchAxis)
    {
        //TODO
    }


}
