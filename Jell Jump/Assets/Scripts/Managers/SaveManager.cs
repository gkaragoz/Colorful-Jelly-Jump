using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region Singleton

    public static SaveManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }

        else if (instance != this) { Destroy(gameObject); }
    }

    #endregion

    // Invokes when character stat is on load
    public Action<CharacterStatsPacket> OnCharacterLoad;

    public struct CharacterStatsPacket
    {
        public int _healthIncreaseRate;

        public float _health;

        public int _jumpLevel;

        public int _jumpLevelRate;

        public int _healthLevel;

        public int _totalGold;

        public int _totalPoint;

        public int _feverJumpLevelRate;

        public int _feverJumpLevel;

        public int _feverJumpDefaultRate;
    }

    public void SaveGame(Character character)
    {
        PlayerPrefs.SetString("SAVED", "TRUE");

        PlayerPrefs.SetInt("HEALTHINCREASERATE", character.GetHealthIncreaseRate());

        PlayerPrefs.SetFloat("HEALTH", character.GetHealth());

        PlayerPrefs.SetInt("JUMPLEVEL", character.GetJumpLevel());

        PlayerPrefs.SetInt("JUMPLEVELRATE", character.GetJumpLevelRate());

        PlayerPrefs.SetInt("HEALTHLEVEL", character.GetHealthLevel());

        PlayerPrefs.SetInt("TOTALGOLD", character.GetTotalGold());

        PlayerPrefs.SetInt("TOTALPOINT", character.GetTotalPoint());

        PlayerPrefs.SetInt("FEVERJUMPLEVELRATE", character.GetFeverJumpLevelRate());

        PlayerPrefs.SetInt("FEVERJUMPLEVEL", character.GetFeverJumpLevel());

        PlayerPrefs.SetInt("FEVERJUMPDEFAULTRATE", character.GetFeverJumpDefaultRate());

        Debug.Log("Character stats is SAVED...");
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SAVED"))
        {
            GameManager.instance.SaveGame();

            return;
        }

        CharacterStatsPacket loadedCharacter = new CharacterStatsPacket();

        // Update loadedCharacter with save object
        loadedCharacter._healthIncreaseRate = PlayerPrefs.GetInt("HEALTHINCREASERATE");

        loadedCharacter._health = PlayerPrefs.GetFloat("HEALTH");

        loadedCharacter._jumpLevel = PlayerPrefs.GetInt("JUMPLEVEL");

        loadedCharacter._jumpLevelRate = PlayerPrefs.GetInt("JUMPLEVELRATE");

        loadedCharacter._healthLevel = PlayerPrefs.GetInt("HEALTHLEVEL");

        loadedCharacter._totalGold = PlayerPrefs.GetInt("TOTALGOLD");

        loadedCharacter._totalPoint = PlayerPrefs.GetInt("TOTALPOINT");

        loadedCharacter._feverJumpLevelRate = PlayerPrefs.GetInt("FEVERJUMPLEVELRATE");

        loadedCharacter._feverJumpLevel = PlayerPrefs.GetInt("FEVERJUMPLEVEL");

        loadedCharacter._feverJumpDefaultRate = PlayerPrefs.GetInt("FEVERJUMPDEFAULTRATE");

        // Invokes OnCharacterLoad
        OnCharacterLoad?.Invoke(loadedCharacter);
    }
}
