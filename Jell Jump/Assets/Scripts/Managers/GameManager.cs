using UnityEngine;

[RequireComponent(typeof(CameraManager), (typeof(LevelManager)), typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }

        else if (instance != this) { Destroy(gameObject); }
    }

    #endregion

    public GameState _gameState = GameState.ONPLAY;

    public static Character MyCharacter { get; private set; }

    private void Start()
    {
        MyCharacter = FindObjectOfType<Character>();

        // TODO
        Screen.orientation = ScreenOrientation.Portrait;

        MyCharacter.OnCharacterDeathState += GameOver;
    }

    // Invokes when the character is dead
    public void GameOver()
    {
        // Set GameState to GameOver
        _gameState = GameState.ONGAMEOVER;

        GetComponent<CameraManager>().CameraAction(CameraAnimationState.ANIM_CLOSEFOCUS, OnComplete);
    }

    // Invokes when the current level is finished
    public void FinishGame()
    {
        // Set GameState to GameOver
        _gameState = GameState.ONFINISH;

        // Play the camera zoom animation
        GetComponent<CameraManager>().CameraAction(CameraAnimationState.ANIM_CLOSEFOCUS, OnComplete);

        // Show popup
        // TODO

        // Invokes Level Manager to say Finish Game
        LevelManager.instance.FinishLevel();
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

        }
    }
}
