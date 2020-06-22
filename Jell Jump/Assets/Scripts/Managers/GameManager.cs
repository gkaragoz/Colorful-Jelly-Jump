using UnityEngine;
using Cinemachine;
using System;

[RequireComponent(typeof(CameraManager), (typeof(LevelManager)), typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    public static GameState _gameState = GameState.ONPLAY;

    public static Character Character { get; private set; }

    private void Awake()
    {
        Character = FindObjectOfType<Character>();

        // TODO
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void Start()
    {
        Character.OnCharacterDeathState += GameOver;
    }

    private void GameOver()
    {
        // Set GameState to GameOver
        _gameState = GameState.GAMEOVER;

        GetComponent<CameraManager>().CameraAction(CameraAnimationState.ANIM_GAMEOVER, OnComplete);
    }

    private void OnComplete()
    {
        switch (_gameState)
        {
            case GameState.GAMEOVER:
                {
                    RestartLevel();

                    break;
                }

            case GameState.ONPLAY:
                {
                    // TODO

                    break;
                }

        }
    }

    private void RestartLevel()
    {
        // Set GameState to ONPLAY
        _gameState = GameState.ONPLAY;

        GetComponent<LevelManager>().RestartLevel();
    }
}
