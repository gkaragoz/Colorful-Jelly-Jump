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

    public struct CharacterStatsPacket
    {
        public int _jumpLevel;

        public int _healthLevel;

        public int _totalGold;

        public int _totalPoint;

        public int _feverJumpLevel;
    }

    public void SaveGame(Character character)
    {
        PlayerPrefs.SetString("SAVED", "TRUE");

        PlayerPrefs.SetInt("JUMPLEVEL", character.GetJumpLevel());

        PlayerPrefs.SetInt("HEALTHLEVEL", character.GetHealthLevel());

        PlayerPrefs.SetInt("TOTALGOLD", character.GetTotalGold());

        PlayerPrefs.SetInt("TOTALPOINT", character.GetTotalPoint());

        PlayerPrefs.SetInt("FEVERJUMPLEVEL", character.GetFeverJumpLevel());

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

        loadedCharacter._jumpLevel = PlayerPrefs.GetInt("JUMPLEVEL");

        loadedCharacter._healthLevel = PlayerPrefs.GetInt("HEALTHLEVEL");

        loadedCharacter._totalGold = PlayerPrefs.GetInt("TOTALGOLD");

        loadedCharacter._totalPoint = PlayerPrefs.GetInt("TOTALPOINT");

        loadedCharacter._feverJumpLevel = PlayerPrefs.GetInt("FEVERJUMPLEVEL");

        GameManager.instance.MyCharacter.LoadCharacterStats(loadedCharacter);
    }
}
