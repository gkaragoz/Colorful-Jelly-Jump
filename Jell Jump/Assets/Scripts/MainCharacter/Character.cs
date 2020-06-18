using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CharacterMovement))]
public class Character : MonoBehaviour
{
    [SerializeField]
    private float Health = 100;

    [SerializeField]
    private int jumpLevel = 0;

    [SerializeField]
    private int healthLevel = 0;

    [SerializeField]
    private int totalGold = 0;

    // Health Level Increaser
    public void IncreaseHealthLevel()
    {
        healthLevel++;

        // Effect on Health
        Health += (Health * healthLevel * 10) / Health;
    }

    // Jump Level Increaser
    public void IncreaseJumpLevel()
    {
        jumpLevel++;
    }

    // Gold Increaser
    public void IncreaseGold(int earnedGold)
    {
        totalGold += earnedGold;
    }

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
