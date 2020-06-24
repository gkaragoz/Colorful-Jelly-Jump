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
    private int _jumpLevel = 0;

    [SerializeField]
    private int _jumpLevelRate = 10;

    [SerializeField]
    private int _healthLevel = 0;

    private int _totalGold = 0;

    private int _totalPoint = 0;

    private int _levelPoint = 0;

    [SerializeField]
    private int _feverJumpLevelRate = 10;

    [SerializeField]
    private int _feverJumpLevel = 0;

    [SerializeField]
    private int _feverJumpDefaultRate = 200;

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
        _healthLevel++;
    }

    // Jump Level Increaser
    public void IncreaseJumpLevel()
    {
        _jumpLevel++;
    }

    // Jump Level Increaser
    public void IncreaseFeverJumpLevel()
    {
        _feverJumpLevel++;
    }

    // Calculates gold rewards
    public void CalculateGoldReward(int levelIndex, int levelTier)
    {
        int reward = levelIndex * levelTier * _levelPoint;

        // Update Gold with reward
        IncreaseGold(reward);
    }

    // Gold Increaser
    public void IncreaseGold(int earnedGold)
    {
        _totalGold += earnedGold;

        // Update Gold on UI
        UIManager.instance.UpdateTotalGold(_totalGold);
    }

    // Gold Decreaser
    public void DecreaseGold(int lostGold)
    {
        _totalGold -= lostGold;

        // Update Gold on UI
        UIManager.instance.UpdateTotalGold(_totalGold);
    }

    // Health Decreaser ( Damage etc. )
    public void DecreaseHealth(float damageCount)
    {
        // TODO
        // Update Health on UI

        Debug.Log("Get " + damageCount + " damage by block");

        if (damageCount > _health)
        {
            _health = 0;

            // Update Health on UI
            UIManager.instance.UpdateHealthBar(_health, HealthMaxLimit());

            return;
        }

        _health -= damageCount;

        // Update Health on UI
        UIManager.instance.UpdateHealthBar(_health, HealthMaxLimit());
    }

    // Health Increaser ( Recover etc. )
    public void IncreaseHealth(float recoveryCount)
    {
        _health += recoveryCount;

        // Update Health on UI
        UIManager.instance.UpdateHealthBar(_health, HealthMaxLimit());
    }

    // Returns Health Max Limit
    public float HealthMaxLimit()
    {
        return _healthIncreaseRate * _healthLevel + 100;
    }

    // Increases point counts
    public void IncreaseTotalPoint()
    {
        _totalPoint += _levelPoint;

        // Update Point on UI
        UIManager.instance.UpdateTotalScore(_totalPoint);
    }

    // Resets level point counts to zero
    public void ResetLevelPoint()
    {
        _levelPoint = 0;

        // Update LevelPoint on UI
        UIManager.instance.UpdateLevelScore(_levelPoint);
    }

    // Increases level point counts
    public void IncreaseLevelPoint(int earnedPoints)
    {
        _levelPoint += earnedPoints;

        Debug.Log("Get " + earnedPoints + " points by block");

        // Update LevelPoint on UI
        UIManager.instance.UpdateLevelScore(_levelPoint);
    }

    // Decreases level point counts
    public void DecreaseLevelPoint(int lostPoints)
    {
        if (_levelPoint < lostPoints)
        {
            ResetLevelPoint();

            return;
        }

        _levelPoint -= lostPoints;

        // Update LevelPoint on UI
        UIManager.instance.UpdateLevelScore(_levelPoint);
    }

    // Character gets damage handler
    public void DoDamage(float damageCount)
    {
        // Decrease Health
        DecreaseHealth(damageCount);

        // Control Death State
        IsCharacterDead();
    }

    // Causes character death
    public void DeadCharacter()
    {
        Debug.Log("Get " + _health + " damage by block");

        _health = 0;

        // Update Health on UI
        UIManager.instance.UpdateHealthBar(_health, HealthMaxLimit());

        OnCharacterDeathState?.Invoke();

        // Stop cube movement immediately
        GetComponent<CharacterMovement>().DisableMovement();
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

    // Character's FeverJump Ability
    public void FeverJump()
    {
        int _jumpRate = 0;

        _jumpRate = ((_feverJumpLevelRate * _feverJumpLevel * _feverJumpDefaultRate) / 100) + _feverJumpDefaultRate;

        GetComponent<CharacterMovement>().Jump(_jumpRate);
    }

    // Calculates jump power via jumpLevel and jumpLevelRate
    public float CalculateJumpPower(float jPower)
    {
        float calculatedPower = ((_jumpLevelRate * _jumpLevel * jPower) / 100) + jPower;

        return calculatedPower;
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
