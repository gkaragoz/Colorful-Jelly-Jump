﻿using UnityEngine;
using System;
using static SaveManager;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(CharacterController), typeof(CharacterMovement))]
public class Character : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _VFXFeverJump = null;

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

    [SerializeField]
    private int _totalPoint = 0;

    private int _levelPoint = 0;

    [SerializeField]
    private int _feverJumpLevelRate = 10;

    [SerializeField]
    private int _feverJumpLevel = 0;

    [SerializeField]
    private int _feverJumpDefaultRate = 200;

    [SerializeField]
    private float _maxScale = 3.5f;

    [SerializeField]
    private float _minScale = 2.5f;

    [SerializeField]
    private float _comboHoldTime = 3;

    private bool _onCombo;

    private int _comboCount;

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

    private void Start()
    {
        // Update health via healthLevel
        UpdateHealthViaLevel();
    }

    // Health Level Increaser
    public void IncreaseHealthLevel()
    {
        _healthLevel++;

        // Update health via healthLevel
        UpdateHealthViaLevel();
    }

    private void UpdateHealthViaLevel()
    {
        _health = _health + _healthIncreaseRate * _healthLevel;
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
        Debug.Log("Get " + damageCount + " damage by block");

        if (damageCount > _health)
        {
            _health = 0;

            // Scales character by health
            ScaleCharacterByHealth();

            return;
        }

        _health -= damageCount;

        // Scales character by health
        ScaleCharacterByHealth();
    }

    // Health Increaser ( Recover etc. )
    public void IncreaseHealth(float recoveryCount)
    {
        _health += recoveryCount;

        // Scales character by health
        ScaleCharacterByHealth();
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

        // Scales character by health
        ScaleCharacterByHealth();

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

        // FEVER VFX.
        PlayFeverVFX();

    }

    private void PlayFeverVFX()
    {
        _VFXFeverJump[0].gameObject.transform.position = new Vector3(
            transform.position.x, 
            transform.position.y, 
            _VFXFeverJump[0].gameObject.transform.position.z);

        _VFXFeverJump[0].Emit(3);
        _VFXFeverJump[1].Emit(16);
        _VFXFeverJump[2].Emit(6);
        _VFXFeverJump[3].Emit(1);
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

    // Scales character when get damage
    public void ScaleCharacterByHealth()
    {
        float rate = _health / HealthMaxLimit();

        float endValue = Mathf.Lerp(_minScale, _maxScale, rate);

        transform.DOScale(endValue, .25f);
    }

    // Handle combo stuff
    public void ComboActivater(int point)
    {
        if (_onCombo)
        {
            // Reset Timer
            StopCoroutine("ComboTimer");

            StartCoroutine("ComboTimer");

            // Increase Combo count
            _comboCount += point * LevelManager.instance.CurrentLevelIndex;

            // Update UI FOR COMBO
            UIManager.instance.UpdateComboPoint(_comboCount);

            // Camera Animation For Combo

            Debug.Log("COMBO: " + _comboCount);
        }

        else
        {
            // Start Timer
            StartCoroutine("ComboTimer");

            _comboCount = point;

            // Update UI FOR COMBO
            UIManager.instance.UpdateComboPoint(_comboCount);
        }
    }

    // Combo Timer
    private IEnumerator ComboTimer()
    {
        _onCombo = true;

        yield return new WaitForSeconds(_comboHoldTime);

        // Increase level point via combocount
        IncreaseLevelPoint(_comboCount);

        // Reset Combo

        _onCombo = false;

        _comboCount = 0;

        // Update UI FOR COMBO
        UIManager.instance.DisableComboText();
    }

    #region SAVE-LOAD

    public void LoadCharacterStats(CharacterStatsPacket packet)
    {
        _jumpLevel = packet._jumpLevel;

        _healthLevel = packet._healthLevel;

        _totalGold = packet._totalGold;

        _totalPoint = packet._totalPoint;

        _feverJumpLevel = packet._feverJumpLevel;

        Debug.Log("Character stats is loaded...");
    }

    #endregion

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
