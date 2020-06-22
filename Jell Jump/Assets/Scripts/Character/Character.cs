using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController), typeof(CharacterMovement))]
public class Character : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Defines Health increase rate for each level-up as below formula\n" +
        "HealthLevel = HealthLevel + _healthIncreaseRate;")]
    private int _healthIncreaseRate = 25;

    [SerializeField]
    private float _health = 100;

    [SerializeField]
    private int _jumpLevel = 1;

    [SerializeField]
    private int _healthLevel = 1;

    [SerializeField]
    private int _totalGold = 0;

    [SerializeField]
    private int _totalPoint = 0;

    // Invokes when character is dead
    public Action OnCharacterDeathState;

    public float Health() { return _health; }

    public int JumpLevel() { return _jumpLevel; }

    public int HealthLevel() { return _healthLevel; }

    public int TotalGold() { return _totalGold; }

    public int TotalPoint() { return _totalPoint; }

    // Health Level Increaser
    public void IncreaseHealthLevel()
    {
        _healthLevel += _healthIncreaseRate;
    }

    // Jump Level Increaser
    public void IncreaseJumpLevel()
    {
        _jumpLevel++;
    }

    // Gold Increaser
    public void IncreaseGold(int earnedGold)
    {
        _totalGold += earnedGold;
    }

    // Gold Decreaser
    public void DecreaseGold(int earnedGold)
    {
        _totalGold -= earnedGold;
    }

    // Health Decreaser ( Damage etc. )
    public void DecreaseHealth(float damageCount)
    {
        _health -= damageCount;

        Debug.Log("Get " + damageCount + " damage by block");
    }

    // Health Increaser ( Recover etc. )
    public void IncreaseHealth(float recoveryCount)
    {
        _health += recoveryCount;
    }

    // Resets point counts to zero
    public void ResetPoint()
    {
        _totalPoint = 0;
    }

    // Increases point counts
    public void IncreasePoint(int earnedPoints)
    {
        _totalPoint += earnedPoints;

        Debug.Log("Get " + earnedPoints + " points by block");
    }

    // Decreases point counts
    public void DecreasePoint(int lostPoints)
    {
        if(_totalPoint < lostPoints)
        {
            ResetPoint();

            return;
        }

        _totalPoint -= lostPoints;
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

    // Causes character death
    public void DeadCharacter()
    {
        Debug.Log("Get " + _health + " damage by block");

        _health = 0;

        OnCharacterDeathState?.Invoke();

        // TEST
        GetComponent<Rigidbody>().isKinematic = true;
    }

    // Check whether character is dead or
    public bool IsCharacterDead()
    {
        if (_health <= 0)
        {
            OnCharacterDeathState?.Invoke();

            // TEST
            GetComponent<Rigidbody>().isKinematic = true;

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
