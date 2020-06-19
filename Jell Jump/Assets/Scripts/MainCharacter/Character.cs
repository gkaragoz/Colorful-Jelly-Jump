using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController), typeof(CharacterMovement))]
public class Character : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Defines Health increase rate for each level-up as percentage\n" +
        "(Health += (Health * HealthLevel *_healthMultiplier) / Health)")]
    private float _healthMultiplier = 10f;

    public float Health { get; private set; }

    public int JumpLevel { get; private set; }

    public int HealthLevel { get; private set; }

    public int TotalGold { get; private set; }

    public int TotalPoint { get; private set; }

    // Invokes when character is dead
    public Action OnCharacterDeathState;

    // Health Level Increaser
    public void IncreaseHealthLevel()
    {
        HealthLevel++;

        // Effect on Health
        Health += (Health * HealthLevel * _healthMultiplier) / Health;
    }

    // Jump Level Increaser
    public void IncreaseJumpLevel()
    {
        JumpLevel++;
    }

    // Gold Increaser
    public void IncreaseGold(int earnedGold)
    {
        TotalGold += earnedGold;
    }

    // Gold Decreaser
    public void DecreaseGold(int earnedGold)
    {
        TotalGold -= earnedGold;
    }

    // Health Decreaser ( Damage etc. )
    public void DecreaseHealth(float damageCount)
    {
        Health -= damageCount;
    }

    // Health Increaser ( Recover etc. )
    public void IncreaseHealth(float recoveryCount)
    {
        Health += recoveryCount;
    }

    // Resets point counts to zero
    public void ResetPoint()
    {
        TotalPoint = 0;
    }

    // Increases point counts
    public void IncreasePoint(int earnedPoints)
    {
        TotalPoint += earnedPoints;
    }

    // Decreases point counts
    public void DecreasePoint(int lostPoints)
    {
        if(TotalPoint < lostPoints)
        {
            ResetPoint();

            return;
        }

        TotalPoint -= lostPoints;
    }
    
    // Character gets damage handler
    public void DoDamage(float damageCount)
    {
        // TODO
        // Decrease Health
        DecreaseHealth(damageCount);

        // Re-size cube scale

        // Control Death State
        IsCharacterDead();
    }

    // Check whether character is dead or
    public bool IsCharacterDead()
    {
        if (Health <= 0)
        {
            OnCharacterDeathState?.Invoke();

            return true;
        }
        else
            return false;
    }

    private void OnEnable()
    {
        GetComponent<CharacterController>().OnJumpDetect +=
            GetComponent<CharacterMovement>().Jump;

        GetComponent<CharacterController>().OnStretchDetect +=
            GetComponent<CharacterMovement>().Stretch;
    }

    private void OnDisable()
    {
        GetComponent<CharacterController>().OnJumpDetect -=
            GetComponent<CharacterMovement>().Jump;

        GetComponent<CharacterController>().OnStretchDetect -=
         GetComponent<CharacterMovement>().Stretch;
    }
}
