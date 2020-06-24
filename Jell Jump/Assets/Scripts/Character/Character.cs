using UnityEngine;
using System;
using UnityEditor.UIElements;
using static SaveManager;

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

    [SerializeField]
    private int _totalGold = 1000;

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

    // Invokes when character' stat is on load
    public Action OnCharacterStatLoad;

    public int GetHealthIncreaseRate() { return _healthIncreaseRate; }

    public float GetHealth() { return _health; }

    public int GetJumpLevel() { return _jumpLevel; }

    public int GetJumpLevelRate() { return _jumpLevelRate; }

    public int GetHealthLevel() { return _healthLevel; }

    public int GetTotalGold() { return _totalGold; }

    public int GetTotalPoint() { return _totalPoint; }

    public int GetFeverJumpLevelRate() { return _feverJumpLevelRate; }

    public int GetFeverJumpLevel() { return _feverJumpLevel; }

    public int GetFeverJumpDefaultRate() { return _feverJumpDefaultRate; }

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
            return;
        }

        _health -= damageCount;
    }

    // Health Increaser ( Recover etc. )
    public void IncreaseHealth(float recoveryCount)
    {
        _health += recoveryCount;
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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        FeverJump();
    //    }
    //}

    // Calculates jump power via jumpLevel and jumpLevelRate
    public float CalculateJumpPower(float jPower)
    {
        float calculatedPower = ((_jumpLevelRate * _jumpLevel * jPower) / 100) + jPower;

        return calculatedPower;
    }

    #region SAVE-LOAD

    public void LoadCharacterStats(CharacterStatsPacket packet)
    {
        _healthIncreaseRate = packet._healthIncreaseRate;

        _health = packet._health;

        _jumpLevel = packet._jumpLevel;

        _jumpLevelRate = packet._jumpLevelRate;

        _healthLevel = packet._healthLevel;

        _totalGold = packet._totalGold;

        _totalPoint = packet._totalPoint;

        _feverJumpLevelRate = packet._feverJumpLevelRate;

        _feverJumpLevel = packet._feverJumpLevel;

        _feverJumpDefaultRate = packet._feverJumpDefaultRate;

        Debug.Log("Character stats is loaded...");
    }

    #endregion

    private void OnEnable()
    {
        GetComponent<CharacterController>().OnJumpDetect +=
            GetComponent<CharacterMovement>().Jump;

        GetComponent<CharacterController>().OnStretchDetect +=
            GetComponent<CharacterMovement>().Stretch;

        //SaveManager.instance.OnCharacterLoad += LoadCharacterStats;
    }

    private void OnDisable()
    {
        GetComponent<CharacterController>().OnJumpDetect -=
            GetComponent<CharacterMovement>().Jump;

        GetComponent<CharacterController>().OnStretchDetect -=
         GetComponent<CharacterMovement>().Stretch;

        //SaveManager.instance.OnCharacterLoad -= LoadCharacterStats;
    }
}
