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

    public float CurrentRemainingDistance { get; private set; }

    private void Start()
    {
        StartCoroutine("LevelStatusController");
    }

    private IEnumerator LevelStatusController()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            // Controls distance between character and end point
            DistanceController();
        }
    }

    // Calculates distance between character and EndPoint
    private void DistanceController()
    {
        CurrentRemainingDistance = Mathf.Abs(GameManager.MyCharacter.transform.position.y - _endPointPrefab.transform.position.y);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void FinishLevel()
    {
        // Stop "LevelStatusController" Coroutine
        StopCoroutine("LevelStatusController");

        Debug.Log("LEVEL FINISHED...");
    }

    public static void LoadNewLevel()
    {
        // TODO
    }
}
