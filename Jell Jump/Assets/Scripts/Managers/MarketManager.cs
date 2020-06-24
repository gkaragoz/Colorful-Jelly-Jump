﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    #region Singleton

    public static MarketManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }

        else if (instance != this) { Destroy(gameObject); }
    }

    #endregion

    [SerializeField]
    private int[] _powerValuesByLevel = null;

    [SerializeField]
    private int[] _healthValuesByLevel = null;

    [SerializeField]
    private int[] _feverValuesByLevel = null;

    [Header("DO NOT CHANGE")]

    [SerializeField]
    private Button _btUpgradePower = null;

    [SerializeField]
    private Button _btUpgradeHealth = null;

    [SerializeField]
    private Button _btUpgradeFever = null;

    [SerializeField]
    private TextMeshProUGUI _txtUpgradePower = null;

    [SerializeField]
    private TextMeshProUGUI _txtUpgradePowerCost = null;

    [SerializeField]
    private TextMeshProUGUI _txtUpgradeHealth = null;

    [SerializeField]
    private TextMeshProUGUI _txtUpgradeHealthCost = null;

    [SerializeField]
    private TextMeshProUGUI _txtUpgradeFever = null;

    [SerializeField]
    private TextMeshProUGUI _txtUpgradeFeverCost = null;

    private void Start()
    {
        // Update Market Content
        UpdateMarketContents();
    }

    public void UpdateMarketContents()
    {
        Character character = FindObjectOfType<Character>();

        int powerLevel = character.JumpLevel();

        int healthLevel = character.HealthLevel();

        int feverLevel = character.FeverJumpLevel();

        if(_powerValuesByLevel.Length > powerLevel)
        {
            _btUpgradePower.interactable = true;

            _txtUpgradePower.text = "Power " + "Lv" + (powerLevel + 1);

            _txtUpgradePowerCost.text = _powerValuesByLevel[powerLevel].ToString() + " Gold";
        }

        else
        {
            _btUpgradePower.interactable = false;

            _txtUpgradePower.text = "Power " + "Lv" + powerLevel;

            _txtUpgradePowerCost.text = _powerValuesByLevel[powerLevel - 1].ToString() + " Gold";
        }

        if (_healthValuesByLevel.Length > healthLevel)
        {
            _btUpgradeHealth.interactable = true;

            _txtUpgradeHealth.text = "Health " + "Lv" + (healthLevel+1);

            _txtUpgradeHealthCost.text = _healthValuesByLevel[healthLevel].ToString() + " Gold";
        }

        else
        {
            _btUpgradeHealth.interactable = false;

            _txtUpgradeHealth.text = "Health " + "Lv" + healthLevel;

            _txtUpgradeHealthCost.text = _healthValuesByLevel[healthLevel - 1].ToString() + " Gold";
        }

        if (_feverValuesByLevel.Length > feverLevel)
        {
            _btUpgradeFever.interactable = true;

            _txtUpgradeFever.text = "Fever " + "Lv" + (feverLevel + 1);

            _txtUpgradeFeverCost.text = _feverValuesByLevel[feverLevel].ToString() + " Gold";
        }

        else
        {
            _btUpgradeFever.interactable = false;

            _txtUpgradeFever.text = "Fever " + "Lv" + feverLevel;

            _txtUpgradeFeverCost.text = _feverValuesByLevel[feverLevel - 1].ToString() + " Gold";
        }
    }

    public void BuyPower()
    {
        Character character = FindObjectOfType<Character>();

        int level = character.JumpLevel();

        if (character.TotalGold() >= _powerValuesByLevel[level])
        {
            character.IncreaseJumpLevel();

            character.DecreaseGold(_powerValuesByLevel[level]);
        }

        UpdateMarketContents();
    }

    public void BuyHealth()
    {
        Character character = FindObjectOfType<Character>();

        int level = character.HealthLevel();

        if (character.TotalGold() >= _healthValuesByLevel[level])
        {
            character.IncreaseHealthLevel();

            character.DecreaseGold(_healthValuesByLevel[level]);
        }

        UpdateMarketContents();
    }

    public void BuyFeverJump()
    {
        Character character = FindObjectOfType<Character>();

        int level = character.FeverJumpLevel();

        if (character.TotalGold() >= _feverValuesByLevel[level])
        {
            character.IncreaseFeverJumpLevel();

            character.DecreaseGold(_feverValuesByLevel[level]);
        }

        UpdateMarketContents();
    }
}