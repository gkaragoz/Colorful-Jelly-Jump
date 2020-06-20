using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    private Character _character;

    [SerializeField]
    private CinemachineVirtualCamera vcam ;

    private void Awake()
    {
        _character = FindObjectOfType<Character>();
    }

    private void Start()
    {
        _character.OnCharacterDeathState += GameOver;

        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void GameOver()
    {
        vcam.LookAt = _character.transform;

        DOTween.To(FindObjectOfType<CamManager>().GET, FindObjectOfType<CamManager>().SET, 20, 1.25f).OnComplete(RestartLevel);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
