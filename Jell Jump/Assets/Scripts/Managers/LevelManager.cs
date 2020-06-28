using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton

    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
            
        else if (instance != this) { Destroy(gameObject); }
    }

    #endregion

    [SerializeField]
    private GameObject _endPointPrefab = null;

    private int _lastLevelCount = 20;

    public int CurrentLevelTier = 1;

    public int CurrentLevelIndex = 1;

    public bool _isBonusLevel;

    public float CurrentRemainingDistance { get; private set; }

    private void Start()
    {
        StartCoroutine("LevelStatusController");
    }

    private IEnumerator LevelStatusController()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            // Controls distance between character and end point
            DistanceController();

            // Update Level Bar
            UIManager.instance.UpdateLevelBar(GameManager.instance.MyCharacter.transform.position.y, 
                _endPointPrefab.transform.position.y);
        }
    }

    // Calculates distance between character and EndPoint
    private void DistanceController()
    {
        CurrentRemainingDistance = Mathf.Abs(GameManager.instance.MyCharacter.transform.position.y - 
            _endPointPrefab.transform.position.y);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void FinishLevel()
    {
        // Stop "LevelStatusController" Coroutine
        StopCoroutine("LevelStatusController");

        // Fill Level Bar
        UIManager.instance.UpdateLevelBar(_endPointPrefab.transform.position.y,
            _endPointPrefab.transform.position.y);

        // Load new Level
        LoadNewLevel();
    }

    public void LoadNewLevel()
    {
        // Load new Level
        if (CurrentLevelIndex != _lastLevelCount)
        {
            SceneManager.LoadScene("Level" + (CurrentLevelIndex + 1));
        }
    }
}
