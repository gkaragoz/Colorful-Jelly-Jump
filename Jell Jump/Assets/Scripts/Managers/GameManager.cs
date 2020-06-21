using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameState _gameState = GameState.ONPLAY;

    public static Character Character { get; private set; }

    [SerializeField]
    private CinemachineVirtualCamera vcam = null;

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

        vcam.LookAt = Character.transform;

        DOTween.To(FindObjectOfType<CamManager>().GET, FindObjectOfType<CamManager>().SET, 20, 1.25f).OnComplete(RestartLevel);
    }

    private void RestartLevel()
    {
        // Set GameState to ONPLAY
        _gameState = GameState.ONPLAY;

        LevelManager.RestartLevel();
    }
}
