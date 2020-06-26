using UnityEngine;

[RequireComponent(typeof(CameraManager), (typeof(LevelManager)), typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; 
        
            MyCharacter = FindObjectOfType<Character>();
        }

        else if (instance != this) { Destroy(gameObject); }
    }

    #endregion

    public GameState _gameState = GameState.ONSTART;

    public Character MyCharacter { get; private set; }

    private void Start()
    {
        // TODO
        Screen.orientation = ScreenOrientation.Portrait;

        MyCharacter.OnCharacterDeathState += GameOver;

        // Load saved game
        LoadGame();
    }

    // Invokes when start to game
    public void StartGame()
    {
        // Disable Start UI
        UIManager.instance.DisableStartGameUI();

        // Enable OnPlay UI
        UIManager.instance.EnableOnGameUI();

        // Enable Joystick
        UIManager.instance.InstantiateJoystick();

        // Save Current Game
        SaveGame();
    }

    // Invokes when the character is dead
    public void GameOver()
    {
        // Set GameState to GameOver
        _gameState = GameState.ONGAMEOVER;

        GetComponent<CameraManager>().CameraAction(CameraAnimationState.ANIM_CLOSEFOCUS, OnComplete);

        // AS GOLD
        MyCharacter.CalculateGoldReward(LevelManager.instance.CurrentLevelIndex,
            LevelManager.instance.CurrentLevelTier);

        // Save Current Game
        SaveGame();
    }

    // Invokes when the current level is finished
    public void FinishGame()
    {
        // Set GameState to GameOver
        _gameState = GameState.ONFINISH;

        // Play the camera zoom animation
        // GetComponent<CameraManager>().CameraAction(CameraAnimationState.ANIM_CLOSEFOCUS, OnComplete);

        // Reward To Character

        // AS POINTS
        MyCharacter.IncreaseTotalPoint();

        // AS GOLD
        MyCharacter.CalculateGoldReward(LevelManager.instance.CurrentLevelIndex,
            LevelManager.instance.CurrentLevelTier);

        // Show popup
        // TODO

        // Save Current Game
        SaveGame();

        // Invokes Level Manager to say Finish Game
        LevelManager.instance.FinishLevel();
    }

    public void SaveGame()
    {
        SaveManager.instance.SaveGame(MyCharacter);
    }

    public void LoadGame()
    {
        SaveManager.instance.LoadGame();
    }

    private void OnComplete()
    {
        switch (_gameState)
        {
            case GameState.ONGAMEOVER:
                {
                    // Set GameState to ONPLAY
                    _gameState = GameState.ONPLAY;

                    GetComponent<LevelManager>().RestartLevel();

                    break;
                }

            case GameState.ONPLAY:
                {
                    // TODO

                    break;
                }

            case GameState.ONFINISH:
                {
                    // TODO

                    break;
                }

            case GameState.ONSTART:
                {
                    // TODO

                    break;
                }

        }
    }
}
